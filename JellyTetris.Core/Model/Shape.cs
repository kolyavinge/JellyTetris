using System;
using System.Collections.Generic;
using System.Linq;
using SoftBodyPhysics.Calculations;
using SoftBodyPhysics.Model;

namespace JellyTetris.Model;

public interface IShape
{
    ShapeKind Kind { get; }

    IShapePart[] Parts { get; }
}

internal class Shape : IShape
{
    public ShapeKind Kind { get; }

    public ISoftBody SoftBody => Parts.First().SoftBody;

    public ShapePiece[] Pieces { get; set; }

    IShapePart[] IShape.Parts => Parts;

    public ShapePart[] Parts { get; set; }

    public InitMassPointPosition[] InitMassPoints { get; }

    public Vector InitMiddlePoint { get; }

    public float CurrentAngle { get; set; }

    public bool IsRotateEnable => Kind != ShapeKind.Cube;

    public Shape(ShapeKind kind, ISoftBody softBody, IEnumerable<ShapePiece> pieces, IEnumerable<ShapePart> parts)
    {
        Kind = kind;
        Pieces = pieces.ToArray();
        Parts = parts.ToArray();
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
