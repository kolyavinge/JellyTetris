using System;
using System.Collections.Generic;
using SoftBodyPhysics.Ancillary;
using SoftBodyPhysics.Geo;
using SoftBodyPhysics.Model;

namespace JellyTetris.Core;

internal interface IShapeBuilder
{
    IShapeBuilder StartPoint(int row, int col);
    ISoftBody MakeShape(IReadOnlyCollection<(int row, int col)> shapeCoords, IBodyEditor editor);
}

internal class ShapeBuilder : IShapeBuilder
{
    private readonly Dictionary<Vector, IMassPoint> _massPoints;
    private readonly HashSet<(IMassPoint, IMassPoint)> _springs;
    private int _startRow, _startCol;
    private IBodyEditor? _editor;
    private ISoftBody? _body;

    public ShapeBuilder()
    {
        _massPoints = new Dictionary<Vector, IMassPoint>();
        _springs = new HashSet<(IMassPoint, IMassPoint)>();
    }

    public IShapeBuilder StartPoint(int row, int col)
    {
        _startRow = row;
        _startCol = col;

        return this;
    }

    public ISoftBody MakeShape(IReadOnlyCollection<(int row, int col)> shapeCoords, IBodyEditor editor)
    {
        _editor = editor;
        _massPoints.Clear();
        _springs.Clear();
        _body = editor.MakeSoftBody();
        foreach (var (row, col) in shapeCoords)
        {
            MakePiece(row + _startRow, col + _startCol);
        }
        _editor.Complete();

        return _body;
    }

    private void MakePiece(int row, int col)
    {
        if (_editor is null) throw new InvalidOperationException();

        var piece = GetPieceCoords(row, col);

        var downLeft = GetMassPointOrCreateNew(piece.DownLeft);
        var upLeft = GetMassPointOrCreateNew(piece.UpLeft);
        var downRight = GetMassPointOrCreateNew(piece.DownRight);
        var upRight = GetMassPointOrCreateNew(piece.UpRight);

        var middleLeft = GetMassPointOrCreateNew(piece.MiddleLeft);
        var middleUp = GetMassPointOrCreateNew(piece.MiddleUp);
        var middleRight = GetMassPointOrCreateNew(piece.MiddleRight);
        var middleDown = GetMassPointOrCreateNew(piece.MiddleDown);

        var middle = GetMassPointOrCreateNew(piece.Middle);

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
        MakeSpringIfNotExist(middleLeft, upLeft);
        MakeSpringIfNotExist(upLeft, middleUp);
        MakeSpringIfNotExist(middleUp, upRight);
        MakeSpringIfNotExist(upRight, middleRight);
        MakeSpringIfNotExist(middleRight, downRight);
        MakeSpringIfNotExist(downRight, middleDown);
        MakeSpringIfNotExist(middleDown, downLeft);

        MakeSpringIfNotExist(middleLeft, middle);
        MakeSpringIfNotExist(middleUp, middle);
        MakeSpringIfNotExist(middleRight, middle);
        MakeSpringIfNotExist(middleDown, middle);

        MakeSpringIfNotExist(downLeft, middle);
        MakeSpringIfNotExist(middleLeft, middleDown);

        MakeSpringIfNotExist(middleLeft, middleUp);
        MakeSpringIfNotExist(upLeft, middle);

        MakeSpringIfNotExist(middle, upRight);
        MakeSpringIfNotExist(middleUp, middleRight);

        MakeSpringIfNotExist(middleDown, middleRight);
        MakeSpringIfNotExist(middle, downRight);
    }

    private void MakeSpringIfNotExist(IMassPoint a, IMassPoint b)
    {
        if (!_springs.Contains((a, b)))
        {
            _editor!.AddSpring(_body!, a, b);
            _springs.Add((a, b));
        }
    }

    private IMassPoint GetMassPointOrCreateNew(Vector position)
    {
        if (_massPoints.ContainsKey(position))
        {
            return _massPoints[position];
        }
        else
        {
            var massPoint = _editor!.AddMassPoint(_body!, position);
            _massPoints.Add(position, massPoint);

            return massPoint;
        }
    }

    private PieceCoords GetPieceCoords(int row, int col)
    {
        var downLeft = new Vector(GameConstants.PieceSize * col, GameConstants.PieceSize * row);
        var upLeft = downLeft + new Vector(0, GameConstants.PieceSize);
        var downRight = downLeft + new Vector(GameConstants.PieceSize, 0);
        var upRight = downLeft + new Vector(GameConstants.PieceSize, GameConstants.PieceSize);

        var middleLeft = downLeft + new Vector(0, GameConstants.PieceSizeHalf);
        var middleUp = downLeft + new Vector(GameConstants.PieceSizeHalf, GameConstants.PieceSize);
        var middleRight = downLeft + new Vector(GameConstants.PieceSize, GameConstants.PieceSizeHalf);
        var middleDown = downLeft + new Vector(GameConstants.PieceSizeHalf, 0);

        var middle = downLeft + new Vector(GameConstants.PieceSizeHalf, GameConstants.PieceSizeHalf);

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

    class PieceCoords
    {
        public readonly Vector DownLeft;
        public readonly Vector UpLeft;
        public readonly Vector DownRight;
        public readonly Vector UpRight;
        public readonly Vector MiddleLeft;
        public readonly Vector MiddleUp;
        public readonly Vector MiddleRight;
        public readonly Vector MiddleDown;
        public readonly Vector Middle;

        public PieceCoords(
            Vector downLeft, Vector upLeft, Vector downRight, Vector upRight, Vector middleLeft, Vector middleUp, Vector middleRight, Vector middleDown, Vector middle)
        {
            DownLeft = downLeft;
            UpLeft = upLeft;
            DownRight = downRight;
            UpRight = upRight;
            MiddleLeft = middleLeft;
            MiddleUp = middleUp;
            MiddleRight = middleRight;
            MiddleDown = middleDown;
            Middle = middle;
        }
    }
}
