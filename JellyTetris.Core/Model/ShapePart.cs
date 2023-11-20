using SoftBodyPhysics.Model;

namespace JellyTetris.Model;

public interface IShapePart
{
    ShapePoint[] EdgePoints { get; }

    IShapeLine[] Lines { get; }
}

internal class ShapePart : IShapePart
{
    public ISoftBody SoftBody { get; }

    public ShapePoint[] EdgePoints { get; }

    IShapeLine[] IShapePart.Lines => Lines;

    public ShapeLine[] Lines { get; }

    public ShapePart(ISoftBody softBody, ShapePoint[] edgePoints, ShapeLine[] lines)
    {
        SoftBody = softBody;
        EdgePoints = edgePoints;
        Lines = lines;
    }
}
