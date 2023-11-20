using JellyTetris.Model;

namespace JellyTetris.Core;

internal interface IShapeEdgeDetector
{
    ShapePoint[] GetEdgePoints(ShapePiece piece);
}

internal class ShapeEdgeDetector : IShapeEdgeDetector
{
    public ShapePoint[] GetEdgePoints(ShapePiece piece)
    {
        return new ShapePoint[]
        {
            new(piece.DownLeft),
            new(piece.MiddleLeft),
            new(piece.UpLeft),
            new(piece.MiddleUp),
            new(piece.UpRight),
            new(piece.MiddleRight),
            new(piece.DownRight),
            new(piece.MiddleDown)
        };
    }
}
