using System.Collections.Generic;
using SoftBodyPhysics.Core;

namespace JellyTetris.Core;

public interface IGame
{
    IShape CurrentShape { get; }
    IShape NextShape { get; }
    IReadOnlyCollection<IShape> Shapes { get; }
    void Update();
    void MoveCurrentShapeLeft();
    void MoveCurrentShapeRight();
    void DropCurrentShape();
    void RotateCurrentShape();
}

internal class Game : IGame
{
    private readonly IPhysicsWorld _physicsWorld;
    private readonly IShapeGenerator _shapeGenerator;
    private readonly List<IShape> _shapes;

    public IShape CurrentShape { get; private set; }

    public IShape NextShape { get; private set; }

    public IReadOnlyCollection<IShape> Shapes => _shapes;

    public Game(
        IPhysicsWorld physicsWorld,
        IShapeGenerator shapeGenerator)
    {
        _physicsWorld = physicsWorld;
        _shapeGenerator = shapeGenerator;
        InitWorld();
        CurrentShape = _shapeGenerator.GetRandomShape();
        //NextShape = _shapeGenerator.GetRandomShape();
        _shapes = new List<IShape> { CurrentShape };
    }

    private void InitWorld()
    {
        InitUnits();
        MakeFieldEdges();
    }

    private void InitUnits()
    {
        _physicsWorld.Units.Time = 0.1f;
        _physicsWorld.Units.SpringStiffness = 250.0f;
        _physicsWorld.Units.SpringDamper = 15f;
        _physicsWorld.Units.Sliding = 0.9f;
    }

    private void MakeFieldEdges()
    {
        var width = GameConstants.FieldWidth * GameConstants.PieceSize;
        var delta = 1;

        var editor = _physicsWorld.MakEditor();

        var hardBody = editor.AddHardBody();
        editor.AddEdge(hardBody, new(-delta, 0), new(width + delta, 0));
        editor.Complete();

        hardBody = editor.AddHardBody();
        editor.AddEdge(hardBody, new(-delta, 0), new(-delta, 10000));
        editor.Complete();

        hardBody = editor.AddHardBody();
        editor.AddEdge(hardBody, new(width + delta, 0), new(width + delta, 10000));
        editor.Complete();
    }

    public void Update()
    {
        _physicsWorld.Update();
        if (!CurrentShape.IsMoving)
        {
            CurrentShape = _shapeGenerator.GetRandomShape();
            _shapes.Add(CurrentShape);
        }
    }

    public void MoveCurrentShapeLeft()
    {

    }

    public void MoveCurrentShapeRight()
    {

    }

    public void DropCurrentShape()
    {

    }

    public void RotateCurrentShape()
    {

    }
}
