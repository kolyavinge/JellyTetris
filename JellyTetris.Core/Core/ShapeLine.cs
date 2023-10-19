namespace JellyTetris.Core;

public interface IShapeLine
{
    Point From { get; }

    Point To { get; }

    bool IsEdge { get; }
}

internal class ShapeLine : IShapeLine
{
    public Point From { get; }

    public Point To { get; }

    public bool IsEdge { get; }

    public ShapeLine(Point from, Point to, bool isEdge)
    {
        From = from;
        To = to;
        IsEdge = isEdge;
    }
}
