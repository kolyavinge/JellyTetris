namespace JellyTetris.Model;

public interface IShapeLine
{
    ShapePoint From { get; }

    ShapePoint To { get; }

    bool IsEdge { get; }
}

internal class ShapeLine : IShapeLine
{
    public ShapePoint From { get; }

    public ShapePoint To { get; }

    public bool IsEdge { get; }

    public ShapeLine(ShapePoint from, ShapePoint to, bool isEdge)
    {
        From = from;
        To = to;
        IsEdge = isEdge;
    }
}
