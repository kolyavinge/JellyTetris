using SoftBodyPhysics.Calculations;

namespace JellyTetris.Core;

public class Point
{
    public readonly float X;
    public readonly float Y;

    public Point(float x, float y)
    {
        X = x;
        Y = y;
    }

    public Point(Vector v)
    {
        X = v.X;
        Y = v.Y;
    }
}
