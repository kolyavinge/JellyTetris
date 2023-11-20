using System;
using System.Collections.Generic;
using System.Linq;
using JellyTetris.Model;
using SoftBodyPhysics.Core;

namespace JellyTetris.Core;

internal class Game : IGame
{
    private readonly IPhysicsWorld _physicsWorld;
    private readonly IShapeGenerator _shapeGenerator;
    private readonly IShapeMovingLogic _shapeMovingLogic;
    private readonly IShapeRotationLogic _shapeRotationLogic;
    private readonly IShapeCollisionChecker _shapeCollisionChecker;
    private readonly ILineEraseLogic _lineEraseLogic;
    private readonly List<Shape> _shapes;
    private Shape _currentShape;
    private DateTime _dropShapeTimestamp;

    public GameState State { get; private set; }

    public IShape CurrentShape => _currentShape;

    public IShape NextShape { get; private set; }

    public IReadOnlyCollection<IShape> Shapes => _shapes;

    public Game(
        IPhysicsWorld physicsWorld,
        IGameInitializer gameInitializer,
        IShapeGenerator shapeGenerator,
        IShapeMovingLogic shapeMovingLogic,
        IShapeRotationLogic shapeRotationLogic,
        IShapeCollisionChecker shapeCollisionChecker,
        ILineEraseLogic lineEraseLogic)
    {
        _physicsWorld = physicsWorld;
        _shapeGenerator = shapeGenerator;
        _shapeMovingLogic = shapeMovingLogic;
        _shapeRotationLogic = shapeRotationLogic;
        _shapeCollisionChecker = shapeCollisionChecker;
        _lineEraseLogic = lineEraseLogic;
        gameInitializer.Init();
        _currentShape = _shapeGenerator.GetRandomShape();
        //NextShape = _shapeGenerator.GetRandomShape();
        _shapes = new List<Shape> { _currentShape };
        State = GameState.Default;
    }

    public void Update()
    {
        if (State == GameState.Default)
        {
            _currentShape.ForAllPoints(p => p.Velocity = new(0, -5));
        }
        else if (State == GameState.DropShape)
        {
            if ((DateTime.Now - _dropShapeTimestamp).TotalSeconds >= 2)
            {
                _lineEraseLogic.EraseLineIfNeeded(_shapes);
                _currentShape = _shapeGenerator.GetRandomShape();
                _shapes.Add(_currentShape);
                State = GameState.Default;
            }
        }
        else if (State == GameState.Over) return;

        _physicsWorld.Update();

        if (State == GameState.Default)
        {
            if (_shapeCollisionChecker.IsShapeCollided(_currentShape))
            {
                if (IsOver())
                {
                    State = GameState.Over;
                }
                else
                {
                    State = GameState.DropShape;
                    _dropShapeTimestamp = DateTime.Now;
                }
            }
        }
    }

    public void MoveCurrentShapeLeft()
    {
        if (State == GameState.Default)
        {
            _shapeMovingLogic.MoveLeft(_currentShape);
        }
    }

    public void MoveCurrentShapeRight()
    {
        if (State == GameState.Default)
        {
            _shapeMovingLogic.MoveRight(_currentShape);
        }
    }

    public void DropCurrentShape()
    {
        if (State == GameState.Default)
        {
            _currentShape.ForAllPoints(p => p.Velocity = new(0, -10));
            State = GameState.DropShape;
            _dropShapeTimestamp = DateTime.Now;
        }
    }

    public void RotateCurrentShape()
    {
        if (State == GameState.Default)
        {
            _shapeRotationLogic.Rotate(_currentShape);
        }
    }

    private bool IsOver()
    {
        return _shapes.SelectMany(x => x.SoftBody.MassPoints).Max(x => x.Position.Y) > GameConstants.FieldHeight * GameConstants.PieceSize;
    }
}
