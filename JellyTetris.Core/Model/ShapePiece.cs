using SoftBodyPhysics.Model;

namespace JellyTetris.Model;

internal class ShapePiece
{
    public readonly IMassPoint DownLeft;
    public readonly IMassPoint UpLeft;
    public readonly IMassPoint DownRight;
    public readonly IMassPoint UpRight;
    public readonly IMassPoint MiddleLeft;
    public readonly IMassPoint MiddleUp;
    public readonly IMassPoint MiddleRight;
    public readonly IMassPoint MiddleDown;
    public readonly IMassPoint Middle;

    public readonly IMassPoint[] AllPoints;

    public ShapePiece(
       IMassPoint downLeft,
       IMassPoint upLeft,
       IMassPoint downRight,
       IMassPoint upRight,
       IMassPoint middleLeft,
       IMassPoint middleUp,
       IMassPoint middleRight,
       IMassPoint middleDown,
       IMassPoint middle)
    {
        DownLeft = downLeft;
        UpLeft = upLeft;
        DownRight = downRight;
        UpRight = upRight;
        MiddleLeft = middleLeft;
        MiddleUp = middleUp;
        MiddleRight = middleRight;
        MiddleDown = middleDown;
        Middle = middle;
        AllPoints = new[]
        {
            downLeft,
            upLeft,
            downRight,
            upRight,
            middleLeft,
            middleUp,
            middleRight,
            middleDown,
            middle
        };
    }
}
