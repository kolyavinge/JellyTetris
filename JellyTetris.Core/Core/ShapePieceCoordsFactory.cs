using SoftBodyPhysics.Calculations;

namespace JellyTetris.Core;

internal class ShapePieceCoords
{
    public readonly Vector DownLeft;
    public readonly Vector UpLeft;
    public readonly Vector DownRight;
    public readonly Vector UpRight;
    public readonly Vector MiddleLeft;
    public readonly Vector MiddleUp;
    public readonly Vector MiddleRight;
    public readonly Vector MiddleDown;
    public readonly Vector Middle;

    public ShapePieceCoords(
        Vector downLeft,
        Vector upLeft,
        Vector downRight,
        Vector upRight,
        Vector middleLeft,
        Vector middleUp,
        Vector middleRight,
        Vector middleDown,
        Vector middle)
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
    }
}

internal interface IShapePieceCoordsFactory
{
    ShapePieceCoords GetPieceCoords(int row, int col);
}

internal class ShapePieceCoordsFactory : IShapePieceCoordsFactory
{
    public ShapePieceCoords GetPieceCoords(int row, int col)
    {
        var downLeft = new Vector(GameConstants.PieceSize * col, GameConstants.PieceSize * row);
        var upLeft = downLeft + new Vector(0, GameConstants.PieceSize);
        var downRight = downLeft + new Vector(GameConstants.PieceSize, 0);
        var upRight = downLeft + new Vector(GameConstants.PieceSize, GameConstants.PieceSize);

        var middleLeft = downLeft + new Vector(0, GameConstants.PieceSizeHalf);
        var middleUp = downLeft + new Vector(GameConstants.PieceSizeHalf, GameConstants.PieceSize);
        var middleRight = downLeft + new Vector(GameConstants.PieceSize, GameConstants.PieceSizeHalf);
        var middleDown = downLeft + new Vector(GameConstants.PieceSizeHalf, 0);

        var middle = downLeft + new Vector(GameConstants.PieceSizeHalf, GameConstants.PieceSizeHalf);

        return new(
            downLeft,
            upLeft,
            downRight,
            upRight,
            middleLeft,
            middleUp,
            middleRight,
            middleDown,
            middle);
    }
}
