using SoftBodyPhysics.Core;

namespace JellyTetris.Core;

internal interface IGameInitializer
{
    void Init();
}

internal class GameInitializer : IGameInitializer
{
    private readonly IPhysicsWorld _physicsWorld;

    public GameInitializer(
        IPhysicsWorld physicsWorld)
    {
        _physicsWorld = physicsWorld;
    }

    public void Init()
    {
        InitUnits();
        MakeFieldEdges();
    }

    private void InitUnits()
    {
        _physicsWorld.Units.Time = 0.1f;
        _physicsWorld.Units.SpringStiffness = 120.0f;
        _physicsWorld.Units.SpringDamper = 30f;
        _physicsWorld.Units.Sliding = 0.8f;
    }

    private void MakeFieldEdges()
    {
        var width = GameConstants.FieldWidth * GameConstants.PieceSize;

        var editor = _physicsWorld.MakeHardBodyEditor();

        var hardBody = editor.AddHardBody();
        editor.AddEdge(hardBody, new(0, 0), new(width, 0));
        editor.Complete();

        hardBody = editor.AddHardBody();
        editor.AddEdge(hardBody, new(0, 0), new(0, 10000));
        editor.Complete();

        hardBody = editor.AddHardBody();
        editor.AddEdge(hardBody, new(width, 0), new(width, 10000));
        editor.Complete();
    }
}
