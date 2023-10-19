using System.Windows.Media;
using JellyTetris.Core;

namespace JellyTetris.Windows.Rendering;

internal class DebugRenderLogic : IRenderLogic
{
    public void Render(DrawingContext dc, IGame game, double actualHeight)
    {
        foreach (var shape in game.Shapes)
        {
            DrawShape(dc, shape, actualHeight);
        }
    }

    private void DrawShape(DrawingContext dc, IShape shape, double actualHeight)
    {
        foreach (var line in shape.Lines)
        {
            var pen = line.IsEdge ? new Pen(Brushes.Green, 0.8) : new Pen(Brushes.DimGray, 0.8);
            dc.DrawLine(pen, new(line.From.X, actualHeight - line.From.Y), new(line.To.X, actualHeight - line.To.Y));
        }
        foreach (var point in shape.Points)
        {
            dc.DrawEllipse(Brushes.Red, null, new(point.X, actualHeight - point.Y), 1.5, 1.5);
        }
    }
}
