using System.Collections.Generic;
using System.Linq;
using JellyTetris.Model;
using SoftBodyPhysics.Calculations;
using SoftBodyPhysics.Core;
using SoftBodyPhysics.Model;

namespace JellyTetris.Core;

internal class MovedPoint
{
    public readonly IMassPoint MassPoint;
    public readonly Vector MovedPosition;

    public MovedPoint(IMassPoint massPoint, Vector movedPosition)
    {
        MassPoint = massPoint;
        MovedPosition = movedPosition;
    }
}

internal interface IShapeCollisionChecker
{
    bool IsShapeCollided(Shape shape);
    bool AreMovedPointsCollided(Shape shape, IReadOnlyCollection<MovedPoint> points);
}

internal class ShapeCollisionChecker : IShapeCollisionChecker
{
    private readonly IPhysicsWorld _physicsWorld;
    private readonly ICurrentShapeContext _currentShapeContext;

    public ShapeCollisionChecker(
        IPhysicsWorld physicsWorld,
        ICurrentShapeContext currentShapeContext)
    {
        _physicsWorld = physicsWorld;
        _currentShapeContext = currentShapeContext;
    }

    public bool IsShapeCollided(Shape shape)
    {
        return _physicsWorld.IsCollidedToAnySoftBody(_currentShapeContext.SoftBody!) || _physicsWorld.IsCollidedToAnyHardBody(_currentShapeContext.SoftBody!);
    }

    public bool AreMovedPointsCollided(Shape shape, IReadOnlyCollection<MovedPoint> points)
    {
        return IsCollidedInField(points) || IsCollidedInShape(shape, points);
    }

    private bool IsCollidedInField(IReadOnlyCollection<MovedPoint> points)
    {
        var minX = points.Min(x => x.MovedPosition.X);
        var maxX = points.Max(x => x.MovedPosition.X);

        var collided = minX <= 1f || maxX >= GameConstants.FieldWidth * GameConstants.PieceSize - 1f;

        return collided;
    }

    private bool IsCollidedInShape(Shape shape, IReadOnlyCollection<MovedPoint> points)
    {
        foreach (var point in points)
        {
            var result = _physicsWorld.GetSoftBodyByPosition(point.MovedPosition).ToArray();
            var noCollision = result.Length == 0 || (result.Length == 1 && result[0] == _currentShapeContext.SoftBody!);
            if (!noCollision) return true;
        }

        return false;
    }
}
