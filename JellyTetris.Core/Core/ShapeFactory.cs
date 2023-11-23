using JellyTetris.Model;

namespace JellyTetris.Core;

internal interface IShapeFactory
{
    Shape Make(ShapeKind shapeKind);
}

internal class ShapeFactory : IShapeFactory
{
    private readonly IShapeTemplates _shapeTemplates;
    private readonly IShapeBuilder _shapeBuilder;
    private readonly IShapePartBuilder _shapePartBuilder;

    public ShapeFactory(
        IShapeTemplates shapeTemplates,
        IShapeBuilder shapeBuilder,
        IShapePartBuilder shapePartBuilder)
    {
        _shapeTemplates = shapeTemplates;
        _shapeBuilder = shapeBuilder;
        _shapePartBuilder = shapePartBuilder;
    }

    public Shape Make(ShapeKind shapeKind)
    {
        var template = _shapeTemplates.GetTemplateFor(shapeKind);
        var pieces = _shapeBuilder.StartPoint(GameConstants.FieldHeight, 4).MakeShape(template);

        return new(shapeKind, pieces, _shapePartBuilder.GetParts(pieces));
    }
}
