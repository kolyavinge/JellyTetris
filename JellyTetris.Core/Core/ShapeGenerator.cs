using System;

namespace JellyTetris.Core;

internal interface IShapeGenerator
{
    Shape GetRandomShape();
}

internal class ShapeGenerator : IShapeGenerator
{
    private readonly IShapeFactory _shapeFactory;
    private readonly Random _rand;

    public ShapeGenerator(IShapeFactory shapeFactory)
    {
        _shapeFactory = shapeFactory;
        _rand = new Random();
    }

    public Shape GetRandomShape()
    {
        var shapeKind = (ShapeKind)_rand.Next(GameConstants.ShapeKindsCount);
        var shape = _shapeFactory.Make(shapeKind);

        return shape;
    }
}
