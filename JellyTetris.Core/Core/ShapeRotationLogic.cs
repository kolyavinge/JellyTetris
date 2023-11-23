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

    private readonly ICurrentShapeContext _currentShapeContext;
    private readonly IShapeCollisionChecker _shapeCollisionChecker;

    public ShapeRotationLogic(
        ICurrentShapeContext currentShapeContext,
        IShapeCollisionChecker shapeCollisionChecker)
    {
        _currentShapeContext = currentShapeContext;
        _shapeCollisionChecker = shapeCollisionChecker;
    }

    public void Rotate(Shape shape)
    {
        if (!shape.IsRotateEnable) return;

        var offset = _currentShapeContext.SoftBody!.MiddlePoint - _currentShapeContext.InitMiddlePoint;

        var points = _currentShapeContext.InitMassPoints
            .Select(x => new MovedPoint(x.MassPoint, GeoCalcs.RotatePoint(x.Position, _currentShapeContext.InitMiddlePoint, _currentShapeContext.CurrentAngle + _angleToRotate) + offset))
            .ToList();

        if (!_shapeCollisionChecker.AreMovedPointsCollided(shape, points))
        {
            points.ForEach(x => x.MassPoint.Position = x.MovedPosition);
            _currentShapeContext.CurrentAngle += _angleToRotate;
        }
    }
}
