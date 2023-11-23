using System.Collections.Generic;
using System.Linq;

namespace JellyTetris.Model;

public interface IShape
{
    ShapeKind Kind { get; }

    IShapePart[] Parts { get; }
}

internal class Shape : IShape
{
    public ShapeKind Kind { get; }

    public ShapePiece[] Pieces { get; set; }

    IShapePart[] IShape.Parts => Parts;

    public ShapePart[] Parts { get; set; }

    public bool IsRotateEnable => Kind != ShapeKind.Cube;

    public Shape(ShapeKind kind, IEnumerable<ShapePiece> pieces, IEnumerable<ShapePart> parts)
    {
        Kind = kind;
        Pieces = pieces.ToArray();
        Parts = parts.ToArray();
    }
}
