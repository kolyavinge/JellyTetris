using System;
using System.Linq;
using SoftBodyPhysics.Calculations;
using SoftBodyPhysics.Model;

namespace JellyTetris.Model;

public interface IShape
{
    ShapeKind Kind { get; }

    IShapePart[] Parts { get; }

    void ForAllPoints(Action<IMassPoint> action);
}

internal interface IShapeInternal : IShape
{
    ISoftBody SoftBody { get; }

    InitMassPointPosition[] InitMassPoints { get; }

    Vector InitMiddlePoint { get; }

    float CurrentAngle { get; set; }

    bool IsRotateEnable { get; }
}

internal class Shape : IShapeInternal
{
    public ShapeKind Kind { get; }

    public ISoftBody SoftBody { get; }

    public ShapePiece[] Pieces { get; set; }

    public IShapePart[] Parts { get; set; }

    public InitMassPointPosition[] InitMassPoints { get; }

    public Vector InitMiddlePoint { get; }

    public float CurrentAngle { get; set; }

    public bool IsRotateEnable => Kind != ShapeKind.Cube;

    public Shape(ShapeKind kind, ISoftBody softBody, ShapePiece[] pieces, IShapePart[] parts)
    {
        Kind = kind;
        SoftBody = softBody;
        Pieces = pieces;
        Parts = parts;
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
