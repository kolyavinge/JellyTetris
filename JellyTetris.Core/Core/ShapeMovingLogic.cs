using System.Linq;
using JellyTetris.Model;

namespace JellyTetris.Core;

internal interface IShapeMovingLogic
{
    void MoveLeft(IShapeInternal shape);

    void MoveRight(IShapeInternal shape);
}

internal class ShapeMovingLogic : IShapeMovingLogic
{
    private readonly IShapeCollisionChecker _shapeCollisionChecker;

    public ShapeMovingLogic(IShapeCollisionChecker shapeCollisionChecker)
    {
        _shapeCollisionChecker = shapeCollisionChecker;
    }

    public void MoveLeft(IShapeInternal shape)
    {
        for (int i = 0; i < 20; i++)
        {
            if (!Move(shape, -0.5f)) return;
        }
    }

    public void MoveRight(IShapeInternal shape)
    {
        for (int i = 0; i < 20; i++)
        {
            if (!Move(shape, 0.5f)) return;
        }
    }

    private bool Move(IShapeInternal shape, float step)
    {
        var points = shape.SoftBody.MassPoints
            .Select(x => new MovedPoint(x, new(x.Position.X + 2.0f * step, x.Position.Y)))
            .ToList();

        if (!_shapeCollisionChecker.AreMovedPointsCollided(shape, points))
        {
            points.ForEach(x => x.MassPoint.Position = new(x.MassPoint.Position.X + step, x.MassPoint.Position.Y));
            return true;
        }
        else
        {
            return false;
        }
    }
}
