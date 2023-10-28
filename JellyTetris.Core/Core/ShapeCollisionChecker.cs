using System.Collections.Generic;
using System.Linq;
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
    bool IsCollided(IShapeInternal shape, IReadOnlyCollection<MovedPoint> points);
}

internal class ShapeCollisionChecker : IShapeCollisionChecker
{
    private readonly IPhysicsWorld _physicsWorld;

    public ShapeCollisionChecker(IPhysicsWorld physicsWorld)
    {
        _physicsWorld = physicsWorld;
    }

    public bool IsCollided(IShapeInternal shape, IReadOnlyCollection<MovedPoint> points)
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

    private bool IsCollidedInShape(IShapeInternal shape, IReadOnlyCollection<MovedPoint> points)
    {
        foreach (var point in points)
        {
            var result = _physicsWorld.GetSoftBodyByPosition(point.MovedPosition).ToArray();
            var noCollision = result.Length == 0 || (result.Length == 1 && result[0] == shape.SoftBody);
            if (!noCollision) return true;
        }

        return false;
    }
}
