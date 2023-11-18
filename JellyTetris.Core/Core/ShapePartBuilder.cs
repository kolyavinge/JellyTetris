using System;
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
        var softBodies = pieces.Select(p => _physicsWorld.GetSoftBodyByMassPoint(p.MiddlePoint) ?? throw new ArgumentException()).Distinct();
        foreach (var softBody in softBodies)
        {
            var edgePoints = _shapeEdgeDetector.GetEdgePoints(softBody).ToArray();
            var lines = softBody.Springs.Select(x => new ShapeLine(new(x.PointA), new(x.PointB), x.IsEdge)).ToArray();

            yield return new ShapePart(edgePoints, lines);
        }
    }
}
