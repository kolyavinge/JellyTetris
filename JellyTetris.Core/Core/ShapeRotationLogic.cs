using System;
using System.Linq;
using JellyTetris.Model;
using SoftBodyPhysics.Calculations;

namespace JellyTetris.Core;

internal interface IShapeRotationLogic
{
    void Rotate(Shape shape);
}

internal class ShapeRotationLogic : IShapeRotationLogic
{
    private const float _angleToRotate = (float)(-Math.PI / 2.0);
    private readonly IShapeCollisionChecker _shapeCollisionChecker;

    public ShapeRotationLogic(IShapeCollisionChecker shapeCollisionChecker)
    {
        _shapeCollisionChecker = shapeCollisionChecker;
    }

    public void Rotate(Shape shape)
    {
        if (!shape.IsRotateEnable) return;

        var offset = shape.SoftBody.MiddlePoint - shape.InitMiddlePoint;

        var points = shape.InitMassPoints
            .Select(x => new MovedPoint(x.MassPoint, GeoCalcs.RotatePoint(x.Position, shape.InitMiddlePoint, shape.CurrentAngle + _angleToRotate) + offset))
            .ToList();

        if (!_shapeCollisionChecker.AreMovedPointsCollided(shape, points))
        {
            points.ForEach(x => x.MassPoint.Position = x.MovedPosition);
            shape.CurrentAngle += _angleToRotate;
        }
    }
}
