using System;
using System.Linq;
using SoftBodyPhysics.Calculations;

namespace JellyTetris.Core;

internal interface IShapeRotationLogic
{
    void Rotate(IShapeInternal shape);
}

internal class ShapeRotationLogic : IShapeRotationLogic
{
    private const float _angleToRotate = (float)(-Math.PI / 2.0);
    private readonly IShapeCollisionChecker _shapeCollisionChecker;

    public ShapeRotationLogic(IShapeCollisionChecker shapeCollisionChecker)
    {
        _shapeCollisionChecker = shapeCollisionChecker;
    }

    public void Rotate(IShapeInternal shape)
    {
        var offset = shape.SoftBody.MiddlePoint - shape.InitMiddlePoint;

        var points = shape.InitMassPoints
            .Select(x => new MovedPoint(x.MassPoint, GeoCalcs.RotatePoint(x.Position, shape.InitMiddlePoint, shape.CurrentAngle + _angleToRotate) + offset))
            .ToList();

        if (!_shapeCollisionChecker.IsCollided(shape, points))
        {
            points.ForEach(x => x.MassPoint.Position = x.MovedPosition);
            shape.CurrentAngle += _angleToRotate;
        }
    }
}
