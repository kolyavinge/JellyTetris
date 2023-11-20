using System.Collections.Generic;
using System.Linq;
using JellyTetris.Model;
using SoftBodyPhysics.Core;

namespace JellyTetris.Core;

internal interface IShapePartBuilder
{
    IEnumerable<ShapePart> GetParts(IEnumerable<ShapePiece> pieces);
}

internal class ShapePartBuilder : IShapePartBuilder
{
    private readonly IPhysicsWorld _physicsWorld;
    private readonly IShapeEdgeDetector _shapeEdgeDetector;

    public ShapePartBuilder(
        IPhysicsWorld physicsWorld,
        IShapeEdgeDetector shapeEdgeDetector)
    {
        _physicsWorld = physicsWorld;
        _shapeEdgeDetector = shapeEdgeDetector;
    }

    public IEnumerable<ShapePart> GetParts(IEnumerable<ShapePiece> pieces)
    {
        foreach (var piece in pieces)
        {
            var softBody = _physicsWorld.GetSoftBodyByMassPoint(piece.Middle)!;
            var edgePoints = _shapeEdgeDetector.GetEdgePoints(piece).ToArray();
            var lines = softBody.Springs.Select(s => new ShapeLine(s, new(s.PointA), new(s.PointB), s.IsEdge)).ToArray();

            yield return new ShapePart(softBody, edgePoints, lines);
        }
    }
}
