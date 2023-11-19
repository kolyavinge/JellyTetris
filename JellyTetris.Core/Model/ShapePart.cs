using SoftBodyPhysics.Model;

namespace JellyTetris.Model;

public interface IShapePart
{
    ISoftBody SoftBody { get; }

    ShapePoint[] EdgePoints { get; }

    IShapeLine[] Lines { get; }
}

internal class ShapePart : IShapePart
{
    public ISoftBody SoftBody { get; }

    public ShapePoint[] EdgePoints { get; }

    public IShapeLine[] Lines { get; }

    public ShapePart(ISoftBody softBody, ShapePoint[] edgePoints, IShapeLine[] lines)
    {
        SoftBody = softBody;
        EdgePoints = edgePoints;
        Lines = lines;
    }
}
