using SoftBodyPhysics.Calculations;
using SoftBodyPhysics.Model;

namespace JellyTetris.Model;

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
