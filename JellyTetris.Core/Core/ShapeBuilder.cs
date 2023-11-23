using System.Collections.Generic;
using JellyTetris.Model;
using SoftBodyPhysics.Ancillary;
using SoftBodyPhysics.Calculations;
using SoftBodyPhysics.Core;
using SoftBodyPhysics.Model;

namespace JellyTetris.Core;

internal interface IShapeBuilder
{
    IShapeBuilder StartPoint(int row, int col);
    List<ShapePiece> MakeShape((int row, int col)[] template);
}

internal class ShapeBuilder : IShapeBuilder
{
    private readonly Dictionary<Vector, IMassPoint> _massPoints;
    private readonly HashSet<(IMassPoint, IMassPoint)> _springs;
    private readonly IPhysicsWorld _physicsWorld;
    private readonly IShapePieceCoordsFactory _shapePieceCoordsFactory;
    private int _startRow, _startCol;
    private ISoftBodyEditor? _softBodyEditor;

    public ShapeBuilder(
        IPhysicsWorld physicsWorld,
        IShapePieceCoordsFactory shapePieceCoordsFactory)
    {
        _physicsWorld = physicsWorld;
        _shapePieceCoordsFactory = shapePieceCoordsFactory;
        _massPoints = new Dictionary<Vector, IMassPoint>();
        _springs = new HashSet<(IMassPoint, IMassPoint)>();
    }

    public IShapeBuilder StartPoint(int row, int col)
    {
        _startRow = row;
        _startCol = col;

        return this;
    }

    public List<ShapePiece> MakeShape((int row, int col)[] template)
    {
        _softBodyEditor = _physicsWorld.MakeSoftBodyEditor();
        _massPoints.Clear();
        _springs.Clear();
        var pieces = new List<ShapePiece>();
        foreach (var (row, col) in template)
        {
            var piece = MakePiece(row + _startRow, col + _startCol);
            pieces.Add(piece);
        }
        _softBodyEditor.Complete();
        _physicsWorld.GetSoftBodyByMassPoint(pieces[0].Middle);

        return pieces;
    }

    private ShapePiece MakePiece(int row, int col)
    {
        var coords = _shapePieceCoordsFactory.GetPieceCoords(row, col);

        var downLeft = GetMassPointOrCreateNew(coords.DownLeft);
        var upLeft = GetMassPointOrCreateNew(coords.UpLeft);
        var downRight = GetMassPointOrCreateNew(coords.DownRight);
        var upRight = GetMassPointOrCreateNew(coords.UpRight);
        var middleLeft = GetMassPointOrCreateNew(coords.MiddleLeft);
        var middleUp = GetMassPointOrCreateNew(coords.MiddleUp);
        var middleRight = GetMassPointOrCreateNew(coords.MiddleRight);
        var middleDown = GetMassPointOrCreateNew(coords.MiddleDown);
        var middle = GetMassPointOrCreateNew(coords.Middle);

        Join(
            downLeft,
            upLeft,
            downRight,
            upRight,
            middleLeft,
            middleUp,
            middleRight,
            middleDown,
            middle);

        return new(
            downLeft,
            upLeft,
            downRight,
            upRight,
            middleLeft,
            middleUp,
            middleRight,
            middleDown,
            middle);
    }

    private void Join(
        IMassPoint downLeft,
        IMassPoint upLeft,
        IMassPoint downRight,
        IMassPoint upRight,
        IMassPoint middleLeft,
        IMassPoint middleUp,
        IMassPoint middleRight,
        IMassPoint middleDown,
        IMassPoint middle)
    {
        MakeSpringIfNotExist(downLeft, middleLeft);
        MakeSpringIfNotExist(upLeft, middleUp);
        MakeSpringIfNotExist(upRight, middleRight);
        MakeSpringIfNotExist(downRight, middleDown);

        MakeSpringIfNotExist(middleLeft, upLeft);
        MakeSpringIfNotExist(middleRight, downRight);
        MakeSpringIfNotExist(middleUp, upRight);
        MakeSpringIfNotExist(middleDown, downLeft);

        MakeSpringIfNotExist(middleLeft, middle);
        MakeSpringIfNotExist(middleUp, middle);
        MakeSpringIfNotExist(middleRight, middle);
        MakeSpringIfNotExist(middleDown, middle);

        MakeSpringIfNotExist(downLeft, middle);
        MakeSpringIfNotExist(upLeft, middle);
        MakeSpringIfNotExist(upRight, middle);
        MakeSpringIfNotExist(downRight, middle);

        MakeSpringIfNotExist(middleLeft, middleDown);
        MakeSpringIfNotExist(middleLeft, middleUp);
        MakeSpringIfNotExist(middleRight, middleDown);
        MakeSpringIfNotExist(middleRight, middleUp);
    }

    private void MakeSpringIfNotExist(IMassPoint a, IMassPoint b)
    {
        if (!_springs.Contains((a, b)) && !_springs.Contains((b, a)))
        {
            _softBodyEditor!.AddSpring(a, b);
            _springs.Add((a, b));
        }
    }

    private IMassPoint GetMassPointOrCreateNew(Vector position)
    {
        if (_massPoints.TryGetValue(position, out IMassPoint? existMassPoint))
        {
            return existMassPoint;
        }

        var newMassPoint = _softBodyEditor!.AddMassPoint(position);
        _massPoints.Add(position, newMassPoint);

        return newMassPoint;
    }
}
