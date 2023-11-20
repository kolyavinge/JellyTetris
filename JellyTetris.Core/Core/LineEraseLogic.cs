using System.Collections.Generic;
using System.Linq;
using JellyTetris.Model;
using SoftBodyPhysics.Core;

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
        shapes.RemoveAll(x => x.Pieces.Length == 0);
    }

    private IEnumerable<ShapeToErase> GetShapesToErase(List<Shape> shapes)
    {
        int maxRow = (int)_physicsWorld.SoftBodies.SelectMany(x => x.MassPoints).Max(x => x.Position.Y / GameConstants.PieceSize);
        for (int row = 0; row < maxRow; row++)
        {
            var matchedShapes = new HashSet<Shape>();
            for (int col = 0; col < GameConstants.FieldWidth; col++)
            {
                var matchedShape = shapes.Find(shape => ShapeIn(shape, row, col));
                if (matchedShape is not null)
                {
                    matchedShapes.Add(matchedShape);
                }
                else
                {
                    matchedShapes.Clear();
                    break;
                }
            }
            if (matchedShapes.Any())
            {
                foreach (var shape in matchedShapes)
                {
                    yield return new(row, shape);
                }
            }
        }
    }

    private void ErasePieces(IReadOnlyCollection<ShapeToErase> shapesToErase)
    {
        var editor = _physicsWorld.MakeSoftBodyEditor();
        foreach (var shapeToErase in shapesToErase)
        {
            var notErasedPieces = shapeToErase.Shape.Pieces
                .Where(piece => !PieceIn(piece, shapeToErase.Row))
                .ToArray();

            var erasedPieces = shapeToErase.Shape.Pieces
                .Where(piece => PieceIn(piece, shapeToErase.Row))
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
                .Distinct()
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

    private bool ShapeIn(Shape shape, int row, int col)
    {
        return shape.Pieces.Any(piece =>
            piece.AllPoints.Any(p => row == (int)(p.Position.Y / GameConstants.PieceSize) &&
                                     col == (int)(p.Position.X / GameConstants.PieceSize)));
    }

    private bool PieceIn(ShapePiece piece, int row)
    {
        return piece.AllPoints.Any(p => row == (int)(p.Position.Y / GameConstants.PieceSize));
    }

    private readonly struct ShapeToErase
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
