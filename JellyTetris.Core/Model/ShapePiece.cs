using SoftBodyPhysics.Model;

namespace JellyTetris.Model;

internal class ShapePiece
{
    public readonly IMassPoint MiddlePoint;

    public readonly IMassPoint[] PerimeterPoints;

    public ShapePiece(IMassPoint middlePoint, IMassPoint[] perimeterPoints)
    {
        MiddlePoint = middlePoint;
        PerimeterPoints = perimeterPoints;
    }
}
