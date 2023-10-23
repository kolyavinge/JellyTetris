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
            var geo = new StreamGeometry();
            using (var ctx = geo.Open())
            {
                var edgePoints = shape.EdgePoints;
                ctx.BeginFigure(new(edgePoints[0].Position.X, actualHeight - edgePoints[0].Position.Y), true, true);
                for (var i = 1; i < edgePoints.Length; i++)
                {
                    ctx.LineTo(new(edgePoints[i].Position.X, actualHeight - edgePoints[i].Position.Y), true, false);
                }
            }
            geo.Freeze();
            dc.DrawGeometry(ShapeColors.GetBrush(shape.Kind), _pen, geo);
        }
    }
}
