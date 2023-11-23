using System;
using System.Linq;
using JellyTetris.Model;
using SoftBodyPhysics.Calculations;
using SoftBodyPhysics.Model;

namespace JellyTetris.Core;

internal interface ICurrentShapeContext
{
    ISoftBody? SoftBody { get; }

    InitMassPointPosition[] InitMassPoints { get; }

    Vector InitMiddlePoint { get; }

    float CurrentAngle { get; set; }

    void Init(Shape shape);

    void ForAllPoints(Action<IMassPoint> action);
}

internal class CurrentShapeContext : ICurrentShapeContext
{
    public ISoftBody? SoftBody { get; private set; }

    public InitMassPointPosition[] InitMassPoints { get; private set; }

    public Vector InitMiddlePoint { get; private set; }

    public float CurrentAngle { get; set; }

    public CurrentShapeContext()
    {
        InitMassPoints = Array.Empty<InitMassPointPosition>();
        InitMiddlePoint = new(0, 0);
    }

    public void Init(Shape shape)
    {
        SoftBody = shape.Parts.First().SoftBody;
        InitMassPoints = SoftBody.MassPoints.Select(mp => new InitMassPointPosition(mp)).ToArray();
        InitMiddlePoint = SoftBody.MiddlePoint.Clone();
        CurrentAngle = 0;
    }

    public void ForAllPoints(Action<IMassPoint> action)
    {
        foreach (var massPoint in SoftBody!.MassPoints)
        {
            action(massPoint);
        }
    }
}
