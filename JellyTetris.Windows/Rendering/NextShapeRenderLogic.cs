using System.Linq;
using System.Windows.Media;
using JellyTetris.Core;

namespace JellyTetris.Windows.Rendering;

internal class NextShapeRenderLogic
{
    private readonly Pen _pen = new Pen(Brushes.Black, 0.5);

    public void Render(DrawingContext dc, IGame game, double actualWidth, double actualHeight)
    {
        var pieceSize = actualWidth / 4.0;
        var shapeKind = game.NextShapeKind;
        var shapeTemplate = game.GetShapeTemplateFor(shapeKind);
        var shapeBrush = ShapeColors.GetBrush(shapeKind);
        var maxRow = shapeTemplate.Max(x => x.row);
        for (int i = 0; i < 4; i++)
        {
            dc.DrawRectangle(shapeBrush, _pen, new(shapeTemplate[i].col * pieceSize, (3 - (3 - maxRow) - shapeTemplate[i].row) * pieceSize, pieceSize, pieceSize));
        }
    }
}
