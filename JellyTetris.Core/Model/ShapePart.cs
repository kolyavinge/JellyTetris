namespace JellyTetris.Model;

public interface IShapePart
{
    ShapePoint[] EdgePoints { get; }

    IShapeLine[] Lines { get; }
}

internal class ShapePart : IShapePart
{
    public ShapePoint[] EdgePoints { get; }

    public IShapeLine[] Lines { get; }

    public ShapePart(ShapePoint[] edgePoints, IShapeLine[] lines)
    {
        EdgePoints = edgePoints;
        Lines = lines;
    }
}
