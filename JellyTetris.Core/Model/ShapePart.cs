using SoftBodyPhysics.Model;

namespace JellyTetris.Model;

public interface IShapePart
{
    ShapePoint[] EdgePoints { get; }

    IShapeLine[] Lines { get; }
}

internal interface IShapePartInternal : IShapePart
{
    ISoftBody SoftBody { get; }

    IShapeLineInternal[] Lines { get; }
}

internal class ShapePart : IShapePartInternal
{
    public ISoftBody SoftBody { get; }

    public ShapePoint[] EdgePoints { get; }

    IShapeLine[] IShapePart.Lines => Lines;

    public IShapeLineInternal[] Lines { get; }

    public ShapePart(ISoftBody softBody, ShapePoint[] edgePoints, IShapeLineInternal[] lines)
    {
        SoftBody = softBody;
        EdgePoints = edgePoints;
        Lines = lines;
    }
}
