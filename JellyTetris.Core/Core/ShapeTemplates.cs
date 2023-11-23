using System;
using JellyTetris.Model;

namespace JellyTetris.Core;

internal interface IShapeTemplates
{
    (int row, int col)[] GetTemplateFor(ShapeKind shapeKind);
}

internal class ShapeTemplates : IShapeTemplates
{
    public readonly (int row, int col)[] CubeTemplate = { (0, 0), (0, 1), (1, 1), (1, 0) };
    public readonly (int row, int col)[] LineTemplate = { (0, 0), (1, 0), (2, 0), (3, 0) };
    public readonly (int row, int col)[] TTemplate = { (0, 0), (0, 1), (0, 2), (1, 1) };
    public readonly (int row, int col)[] L1Template = { (2, 0), (1, 0), (0, 0), (0, 1) };
    public readonly (int row, int col)[] L2Template = { (2, 1), (1, 1), (0, 1), (0, 0) };
    public readonly (int row, int col)[] S1Template = { (2, 0), (1, 0), (1, 1), (0, 1) };
    public readonly (int row, int col)[] S2Template = { (2, 1), (1, 1), (1, 0), (0, 0) };

    public (int row, int col)[] GetTemplateFor(ShapeKind shapeKind)
    {
        return shapeKind switch
        {
            ShapeKind.Cube => CubeTemplate,
            ShapeKind.Line => LineTemplate,
            ShapeKind.T => TTemplate,
            ShapeKind.L1 => L1Template,
            ShapeKind.L2 => L2Template,
            ShapeKind.S1 => S1Template,
            ShapeKind.S2 => S2Template,
            _ => throw new ArgumentException()
        };
    }
}
