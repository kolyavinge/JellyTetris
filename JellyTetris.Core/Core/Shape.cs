using System;
using System.Collections.Generic;
using System.Linq;
using SoftBodyPhysics.Calculations;
using SoftBodyPhysics.Model;

namespace JellyTetris.Core;

public interface IShape
{
    ShapeKind Kind { get; }

    IMassPoint[] EdgePoints { get; }

    IEnumerable<Point> Points { get; }

    IEnumerable<IShapeLine> Lines { get; }

    bool IsMoving { get; }

    void ForAllPoints(Action<IMassPoint> action);
}

internal class InitMassPointPosition
{
    public readonly IMassPoint MassPoint;
    public readonly Vector Position;

    public InitMassPointPosition(IMassPoint massPoint)
    {
        MassPoint = massPoint;
        Position = massPoint.Position.Clone();
    }
}

internal interface IShapeInternal : IShape
{
    ISoftBody SoftBody { get; }

    InitMassPointPosition[] InitMassPoints { get; }

    Vector InitMiddlePoint { get; }

    float CurrentAngle { get; set; }
}

internal class Shape : IShapeInternal
{
    public ShapeKind Kind { get; }

    public ISoftBody SoftBody { get; }

    public IMassPoint[] EdgePoints { get; set; }

    public IEnumerable<Point> Points => SoftBody.MassPoints.Select(p => new Point(p.Position.X, p.Position.Y));

    public IEnumerable<IShapeLine> Lines => SoftBody.Springs.Select(s => new ShapeLine(new(s.PointA.Position), new(s.PointB.Position), s.IsEdge));

    public InitMassPointPosition[] InitMassPoints { get; }

    public Vector InitMiddlePoint { get; }

    public float CurrentAngle { get; set; }

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
        InitMassPoints = SoftBody.MassPoints.Select(mp => new InitMassPointPosition(mp)).ToArray();
        InitMiddlePoint = SoftBody.MiddlePoint.Clone();
    }

    public void ForAllPoints(Action<IMassPoint> action)
    {
        foreach (var massPoint in SoftBody.MassPoints)
        {
            action(massPoint);
        }
    }
}
