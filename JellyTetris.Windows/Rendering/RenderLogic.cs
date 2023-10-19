using System.Linq;
using System.Windows.Media;
using JellyTetris.Core;

namespace JellyTetris.Windows.Rendering;

internal class RenderLogic : IRenderLogic
{
    private readonly Pen _pen = new(Brushes.Black, 1.0);

    public void Render(DrawingContext dc, IGame game, double actualHeight)
    {
        foreach (var shape in game.Shapes)
        {
            var start = new System.Windows.Point(shape.EdgePoints[0].Position.X, actualHeight - shape.EdgePoints[0].Position.Y);
            var segments = shape.EdgePoints.Skip(1).Select(p => new LineSegment(new(p.Position.X, actualHeight - p.Position.Y), true)).ToList();
            var figure = new PathFigure(start, segments, true);
            var geo = new PathGeometry(new[] { figure });
            dc.DrawGeometry(ShapeColors.GetBrush(shape.Kind), _pen, geo);
        }
    }
}
