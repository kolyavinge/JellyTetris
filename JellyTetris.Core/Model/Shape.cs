using System;
using System.Linq;
using SoftBodyPhysics.Calculations;
using SoftBodyPhysics.Model;

namespace JellyTetris.Model;

public interface IShape
{
    ShapeKind Kind { get; }

    IShapePart[] Parts { get; }
}

internal interface IShapeInternal : IShape
{
    ISoftBody SoftBody { get; }

    InitMassPointPosition[] InitMassPoints { get; }

    Vector InitMiddlePoint { get; }

    float CurrentAngle { get; set; }

    bool IsRotateEnable { get; }

    void ForAllPoints(Action<IMassPoint> action);
}

internal class Shape : IShapeInternal
{
    public ShapeKind Kind { get; }

    public ISoftBody SoftBody => Parts.First().SoftBody;

    public ShapePiece[] Pieces { get; set; }

    IShapePart[] IShape.Parts => Parts;

    public ShapePart[] Parts;

    public InitMassPointPosition[] InitMassPoints { get; }

    public Vector InitMiddlePoint { get; }

    public float CurrentAngle { get; set; }

    public bool IsRotateEnable => Kind != ShapeKind.Cube;

    public Shape(ShapeKind kind, ISoftBody softBody, ShapePiece[] pieces, ShapePart[] parts)
    {
        Kind = kind;
        Pieces = pieces;
        Parts = parts;
        InitMassPoints = softBody.MassPoints.Select(mp => new InitMassPointPosition(mp)).ToArray();
        InitMiddlePoint = softBody.MiddlePoint.Clone();
    }

    public void ForAllPoints(Action<IMassPoint> action)
    {
        foreach (var massPoint in SoftBody.MassPoints)
        {
            action(massPoint);
        }
    }
}
