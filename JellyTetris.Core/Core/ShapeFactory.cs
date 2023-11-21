using System;
using JellyTetris.Model;

namespace JellyTetris.Core;

internal interface IShapeFactory
{
    Shape Make(ShapeKind shapeKind);
    Shape MakeCube();
    Shape MakeLine();
    Shape MakeT();
    Shape MakeL1();
    Shape MakeL2();
    Shape MakeS1();
    Shape MakeS2();
}

internal class ShapeFactory : IShapeFactory
{
    private readonly IShapeBuilder _shapeBuilder;
    private readonly IShapePartBuilder _shapePartBuilder;

    public ShapeFactory(
        IShapeBuilder shapeBuilder,
        IShapePartBuilder shapePartBuilder)
    {
        _shapeBuilder = shapeBuilder;
        _shapePartBuilder = shapePartBuilder;
    }

    public Shape Make(ShapeKind shapeKind)
    {
        return shapeKind switch
        {
            ShapeKind.Cube => MakeCube(),
            ShapeKind.Line => MakeLine(),
            ShapeKind.T => MakeT(),
            ShapeKind.L1 => MakeL1(),
            ShapeKind.L2 => MakeL2(),
            ShapeKind.S1 => MakeS1(),
            ShapeKind.S2 => MakeS2(),
            _ => throw new ArgumentException()
        };
    }

    public Shape MakeCube()
    {
        var body = _shapeBuilder.StartPoint(GameConstants.FieldHeight, 4).MakeShape((0, 0), (0, 1), (1, 1), (1, 0));

        return new(ShapeKind.Cube, body, _shapeBuilder.Pieces, _shapePartBuilder.GetParts(_shapeBuilder.Pieces));
    }

    public Shape MakeLine()
    {
        var body = _shapeBuilder.StartPoint(GameConstants.FieldHeight, 4).MakeShape((0, 0), (1, 0), (2, 0), (3, 0));

        return new(ShapeKind.Line, body, _shapeBuilder.Pieces, _shapePartBuilder.GetParts(_shapeBuilder.Pieces));
    }

    public Shape MakeT()
    {
        var body = _shapeBuilder.StartPoint(GameConstants.FieldHeight, 4).MakeShape((0, 0), (0, 1), (0, 2), (1, 1));

        return new(ShapeKind.T, body, _shapeBuilder.Pieces, _shapePartBuilder.GetParts(_shapeBuilder.Pieces));
    }

    public Shape MakeL1()
    {
        var body = _shapeBuilder.StartPoint(GameConstants.FieldHeight, 4).MakeShape((2, 0), (1, 0), (0, 0), (0, 1));

        return new(ShapeKind.L1, body, _shapeBuilder.Pieces, _shapePartBuilder.GetParts(_shapeBuilder.Pieces));
    }

    public Shape MakeL2()
    {
        var body = _shapeBuilder.StartPoint(GameConstants.FieldHeight, 4).MakeShape((2, 1), (1, 1), (0, 1), (0, 0));

        return new(ShapeKind.L2, body, _shapeBuilder.Pieces, _shapePartBuilder.GetParts(_shapeBuilder.Pieces));
    }

    public Shape MakeS1()
    {
        var body = _shapeBuilder.StartPoint(GameConstants.FieldHeight, 4).MakeShape((2, 0), (1, 0), (1, 1), (0, 1));

        return new(ShapeKind.S1, body, _shapeBuilder.Pieces, _shapePartBuilder.GetParts(_shapeBuilder.Pieces));
    }

    public Shape MakeS2()
    {
        var body = _shapeBuilder.StartPoint(GameConstants.FieldHeight, 4).MakeShape((2, 1), (1, 1), (1, 0), (0, 0));

        return new(ShapeKind.S2, body, _shapeBuilder.Pieces, _shapePartBuilder.GetParts(_shapeBuilder.Pieces));
    }
}
