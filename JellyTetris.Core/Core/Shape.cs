using System.Collections.Generic;
using System.Linq;
using SoftBodyPhysics.Model;

namespace JellyTetris.Core;

public interface IShape
{
    ShapeKind Kind { get; }

    IMassPoint[] EdgePoints { get; }

    IEnumerable<Point> Points { get; }

    IEnumerable<IShapeLine> Lines { get; }

    bool IsMoving { get; }
}

internal class Shape : IShape
{
    public ShapeKind Kind { get; }

    public ISoftBody SoftBody { get; }

    public IMassPoint[] EdgePoints { get; set; }

    public IEnumerable<Point> Points => SoftBody.MassPoints.Select(p => new Point(p.Position.X, p.Position.Y));

    public IEnumerable<IShapeLine> Lines => SoftBody.Springs.Select(s => new ShapeLine(new(s.PointA.Position), new(s.PointB.Position), s.IsEdge));

    public bool IsMoving
    {
        get
        {
            var v = SoftBody.MassPoints.Sum(m => m.Velocity.Length);
            return v > 20;
        }
    }

    public Shape(ShapeKind kind, ISoftBody softBody, IEnumerable<IMassPoint> edgePoints)
    {
        Kind = kind;
        SoftBody = softBody;
        EdgePoints = edgePoints.ToArray();
    }
}
