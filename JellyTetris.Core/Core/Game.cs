﻿using System;
using System.Collections.Generic;
using System.Linq;
using JellyTetris.Model;
using SoftBodyPhysics.Core;

namespace JellyTetris.Core;

internal class Game : IGame
{
    private readonly IPhysicsWorld _physicsWorld;
    private readonly IShapeTemplates _shapeTemplates;
    private readonly IShapeGenerator _shapeGenerator;
    private readonly ICurrentShapeContext _currentShapeContext;
    private readonly IShapeMovingLogic _shapeMovingLogic;
    private readonly IShapeRotationLogic _shapeRotationLogic;
    private readonly IShapeCollisionChecker _shapeCollisionChecker;
    private readonly ILineEraseLogic _lineEraseLogic;
    private readonly List<Shape> _shapes;
    private Shape _currentShape;
    private DateTime _dropShapeTimestamp;

    public GameState State { get; private set; }

    public IShape CurrentShape => _currentShape;

    public ShapeKind NextShapeKind => _shapeGenerator.NextShapeKind;

    public IReadOnlyCollection<IShape> Shapes => _shapes;

    public Game(
        IPhysicsWorld physicsWorld,
        IShapeTemplates shapeTemplates,
        IShapeGenerator shapeGenerator,
        ICurrentShapeContext currentShapeContext,
        IShapeMovingLogic shapeMovingLogic,
        IShapeRotationLogic shapeRotationLogic,
        IShapeCollisionChecker shapeCollisionChecker,
        ILineEraseLogic lineEraseLogic)
    {
        _physicsWorld = physicsWorld;
        _shapeTemplates = shapeTemplates;
        _shapeGenerator = shapeGenerator;
        _currentShapeContext = currentShapeContext;
        _shapeMovingLogic = shapeMovingLogic;
        _shapeRotationLogic = shapeRotationLogic;
        _shapeCollisionChecker = shapeCollisionChecker;
        _lineEraseLogic = lineEraseLogic;
        _currentShape = _shapeGenerator.GetCurrentShape();
        _currentShapeContext.Init(_currentShape);
        _shapes = new List<Shape> { _currentShape };
        State = GameState.Default;
    }

    public void Update()
    {
        if (State == GameState.Default)
        {
            _currentShapeContext.ForAllPoints(p => p.Velocity = new(0, -5));
        }
        else if (State == GameState.DropShape)
        {
            if ((DateTime.Now - _dropShapeTimestamp).TotalSeconds >= 2)
            {
                _lineEraseLogic.EraseLineIfNeeded(_shapes);
                _currentShape = _shapeGenerator.GetCurrentShape();
                _currentShapeContext.Init(_currentShape);
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
            _currentShapeContext.ForAllPoints(p => p.Velocity = new(0, -10));
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

    public (int row, int col)[] GetShapeTemplateFor(ShapeKind shapeKind)
    {
        return _shapeTemplates.GetTemplateFor(shapeKind);
    }

    private bool IsOver()
    {
        return _physicsWorld.SoftBodies.SelectMany(x => x.MassPoints).Max(x => x.Position.Y) > GameConstants.FieldHeight * GameConstants.PieceSize;
    }
}
