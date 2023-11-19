using System.Collections.Generic;
using System.Linq;
using JellyTetris.Model;
using SoftBodyPhysics.Core;
using SoftBodyPhysics.Model;

namespace JellyTetris.Core;

internal interface ILineEraseLogic
{
    void EraseLineIfNeeded(List<Shape> shapes);
}

internal class LineEraseLogic : ILineEraseLogic
{
    private readonly IPhysicsWorld _physicsWorld;
    private readonly IShapePartBuilder _shapePartBuilder;

    public LineEraseLogic(
        IPhysicsWorld physicsWorld,
        IShapePartBuilder shapePartBuilder)
    {
        _physicsWorld = physicsWorld;
        _shapePartBuilder = shapePartBuilder;
    }

    public void EraseLineIfNeeded(List<Shape> shapes)
    {
        var shapesToErase = GetShapesToErase(shapes).ToArray();
        if (!shapesToErase.Any()) return;
        ErasePieces(shapesToErase);
        DeleteEmptyShapes(shapes);
    }

    private IEnumerable<ShapeToErase> GetShapesToErase(List<Shape> shapes)
    {
        var shapeSoftBodyDictionary = new Dictionary<ISoftBody, Shape>();
        foreach (var shape in shapes)
        {
            foreach (var softBody in shape.Parts.Select(x => x.SoftBody))
            {
                shapeSoftBodyDictionary.Add(softBody, shape);
            }
        }
        var maxY = _physicsWorld.SoftBodies.SelectMany(x => x.MassPoints).Max(x => x.Position.Y);
        for (float y = GameConstants.PieceSizeHalf; y <= maxY; y += GameConstants.PieceSize)
        {
            var matchedShapes = new HashSet<Shape>();
            for (float x = 0; x < GameConstants.FieldWidth * GameConstants.PieceSize; x += GameConstants.PieceSize)
            {
                var matchedBody = _physicsWorld
                    .GetSoftBodyByPosition(new(x, y))
                    .Union(_physicsWorld.GetSoftBodyByPosition(new(x + GameConstants.PieceSizeQuarter, y)))
                    .Union(_physicsWorld.GetSoftBodyByPosition(new(x + GameConstants.PieceSizeHalf, y)))
                    .Union(_physicsWorld.GetSoftBodyByPosition(new(x + GameConstants.PieceSizeHalf + GameConstants.PieceSizeQuarter, y)))
                    .Union(_physicsWorld.GetSoftBodyByPosition(new(x + GameConstants.PieceSize, y)))
                    .FirstOrDefault();
                if (matchedBody is not null)
                {
                    var shape = shapeSoftBodyDictionary[matchedBody];
                    matchedShapes.Add(shape);
                }
                else
                {
                    matchedShapes.Clear();
                    break;
                }
            }
            if (matchedShapes.Any())
            {
                var row = (int)(y / GameConstants.PieceSize);
                foreach (var shape in matchedShapes)
                {
                    yield return new(row, shape);
                }
            }
        }
    }

    private void ErasePieces(IEnumerable<ShapeToErase> shapesToErase)
    {
        var editor = _physicsWorld.MakeSoftBodyEditor();
        foreach (var shapeToErase in shapesToErase)
        {
            var notErasedPieces = shapeToErase.Shape.Pieces
                .Where(piece => (int)(piece.Middle.Position.Y / GameConstants.PieceSize) != shapeToErase.Row)
                .ToArray();

            var erasedPieces = shapeToErase.Shape.Pieces
                .Where(piece => (int)(piece.Middle.Position.Y / GameConstants.PieceSize) == shapeToErase.Row)
                .ToArray();

            var notErasedPiecePoints = notErasedPieces.SelectMany(piece => piece.AllPoints).ToHashSet();

            var erasedPiecePoints = erasedPieces
                .SelectMany(piece => piece.AllPoints)
                .Where(p => !notErasedPiecePoints.Contains(p))
                .ToHashSet();

            var springs =
                shapeToErase.Shape.Parts
                .SelectMany(x => x.Lines)
                .Select(x => x.Spring)
                .ToArray();

            var erasedSprings =
                from spring in springs
                from piece in erasedPieces
                where
                    (spring.PointA == piece.MiddleLeft && spring.PointB == piece.MiddleUp) ||
                    (spring.PointB == piece.MiddleLeft && spring.PointA == piece.MiddleUp) ||
                    (spring.PointA == piece.MiddleLeft && spring.PointB == piece.MiddleDown) ||
                    (spring.PointB == piece.MiddleLeft && spring.PointA == piece.MiddleDown) ||
                    (spring.PointA == piece.MiddleRight && spring.PointB == piece.MiddleUp) ||
                    (spring.PointB == piece.MiddleRight && spring.PointA == piece.MiddleUp) ||
                    (spring.PointA == piece.MiddleRight && spring.PointB == piece.MiddleDown) ||
                    (spring.PointB == piece.MiddleRight && spring.PointA == piece.MiddleDown)
                select spring;

            editor.DeleteMassPoints(erasedPiecePoints);
            editor.DeleteSprings(erasedSprings);

            shapeToErase.Shape.Pieces = notErasedPieces;
        }
        editor.Complete();

        foreach (var shapeToErase in shapesToErase)
        {
            shapeToErase.Shape.Parts = _shapePartBuilder.GetParts(shapeToErase.Shape.Pieces).ToArray();
        }
    }

    private void DeleteEmptyShapes(List<Shape> shapes)
    {
        shapes.RemoveAll(x => x.Pieces.Length == 0);
    }

    readonly struct ShapeToErase
    {
        public readonly int Row;
        public readonly Shape Shape;

        public ShapeToErase(int row, Shape shape)
        {
            Row = row;
            Shape = shape;
        }
    }
}
