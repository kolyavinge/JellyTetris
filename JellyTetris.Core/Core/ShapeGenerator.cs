using System;
using JellyTetris.Model;

namespace JellyTetris.Core;

internal interface IShapeGenerator
{
    ShapeKind NextShapeKind { get; }
    Shape GetCurrentShape();
}

internal class ShapeGenerator : IShapeGenerator
{
    private readonly IShapeFactory _shapeFactory;
    private readonly Random _rand;

    public ShapeKind NextShapeKind { get; private set; }

    public ShapeGenerator(IShapeFactory shapeFactory)
    {
        _shapeFactory = shapeFactory;
        _rand = new Random();
        UpdateNextShapeKind();
    }

    public Shape GetCurrentShape()
    {
        var shape = _shapeFactory.Make(NextShapeKind);
        UpdateNextShapeKind();

        return shape;
    }

    private void UpdateNextShapeKind()
    {
        NextShapeKind = (ShapeKind)_rand.Next(GameConstants.ShapeKindsCount);
    }
}
