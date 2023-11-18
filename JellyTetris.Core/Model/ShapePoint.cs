using SoftBodyPhysics.Calculations;

namespace JellyTetris.Model;

public class ShapePoint
{
    private readonly Vector _vector;

    public float X => _vector.X;

    public float Y => _vector.Y;

    internal ShapePoint(Vector vector)
    {
        _vector = vector;
    }
}
