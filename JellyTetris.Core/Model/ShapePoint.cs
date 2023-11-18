using SoftBodyPhysics.Model;

namespace JellyTetris.Model;

public class ShapePoint
{
    private readonly IMassPoint _massPoint;

    public float X => _massPoint.Position.X;

    public float Y => _massPoint.Position.Y;

    internal ShapePoint(IMassPoint massPoint)
    {
        _massPoint = massPoint;
    }
}
