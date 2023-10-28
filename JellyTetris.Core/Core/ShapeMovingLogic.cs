using System.Linq;

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
        for (int i = 0; i < 10; i++)
        {
            if (!Move(shape, -1)) return;
        }
    }

    public void MoveRight(IShapeInternal shape)
    {
        for (int i = 0; i < 10; i++)
        {
            if (!Move(shape, 1)) return;
        }
    }

    private bool Move(IShapeInternal shape, float step)
    {
        var points = shape.SoftBody.MassPoints
          .Select(x => new MovedPoint(x, new(x.Position.X + step, x.Position.Y)))
          .ToList();

        if (!_shapeCollisionChecker.IsCollided(shape, points))
        {
            points.ForEach(x => x.MassPoint.Position = x.MovedPosition);
            return true;
        }
        else
        {
            return false;
        }
    }
}
