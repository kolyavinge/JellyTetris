using System.Collections.Generic;
using SoftBodyPhysics.Core;

namespace JellyTetris.Core;

internal class Game : IGame
{
    private readonly IPhysicsWorld _physicsWorld;
    private readonly IShapeGenerator _shapeGenerator;
    private readonly IShapeMovingLogic _shapeMovingLogic;
    private readonly IShapeRotationLogic _shapeRotationLogic;
    private readonly List<IShapeInternal> _shapes;
    private IShapeInternal _currentShape;
    private GameState _state;

    public IShape CurrentShape => _currentShape;

    public IShape NextShape { get; private set; }

    public IReadOnlyCollection<IShape> Shapes => _shapes;

    public Game(
        IPhysicsWorld physicsWorld,
        IGameInitializer gameInitializer,
        IShapeGenerator shapeGenerator,
        IShapeMovingLogic shapeMovingLogic,
        IShapeRotationLogic shapeRotationLogic)
    {
        _physicsWorld = physicsWorld;
        _shapeGenerator = shapeGenerator;
        _shapeMovingLogic = shapeMovingLogic;
        _shapeRotationLogic = shapeRotationLogic;
        gameInitializer.Init();
        _currentShape = _shapeGenerator.GetRandomShape();
        //NextShape = _shapeGenerator.GetRandomShape();
        _shapes = new List<IShapeInternal> { _currentShape };
        _state = GameState.Default;
    }

    public void Update()
    {
        if (_state == GameState.Default)
        {
            _currentShape.ForAllPoints(p => p.Velocity = new(0, -5));
        }
        else if (_state == GameState.DropShape)
        {
            if (!_currentShape.IsMoving)
            {
                _currentShape = _shapeGenerator.GetRandomShape();
                _shapes.Add(_currentShape);
                _state = GameState.Default;
            }
        }

        _physicsWorld.Update();
    }

    public void MoveCurrentShapeLeft()
    {
        if (_state == GameState.Default)
        {
            _shapeMovingLogic.MoveLeft(_currentShape);
        }
    }

    public void MoveCurrentShapeRight()
    {
        if (_state == GameState.Default)
        {
            _shapeMovingLogic.MoveRight(_currentShape);
        }
    }

    public void DropCurrentShape()
    {
        if (_state == GameState.Default)
        {
            _currentShape.ForAllPoints(p => p.Velocity = new(0, -10));
            _state = GameState.DropShape;
        }
    }

    public void RotateCurrentShape()
    {
        if (_state == GameState.Default)
        {
            _shapeRotationLogic.Rotate(_currentShape);
        }
    }
}
