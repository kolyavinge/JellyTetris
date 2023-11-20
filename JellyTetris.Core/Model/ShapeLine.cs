using SoftBodyPhysics.Model;

namespace JellyTetris.Model;

public interface IShapeLine
{
    ShapePoint From { get; }

    ShapePoint To { get; }

    bool IsEdge { get; }
}

internal class ShapeLine : IShapeLine
{
    public ISpring Spring { get; }

    public ShapePoint From { get; }

    public ShapePoint To { get; }

    public bool IsEdge { get; }

    public ShapeLine(ISpring spring, ShapePoint from, ShapePoint to, bool isEdge)
    {
        Spring = spring;
        From = from;
        To = to;
        IsEdge = isEdge;
    }
}
