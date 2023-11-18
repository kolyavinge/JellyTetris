using System.Globalization;
using System.Windows;
using System.Windows.Media;
using JellyTetris.Core;

namespace JellyTetris.Windows.Rendering;

internal class RenderLogic : IRenderLogic
{
    private readonly Pen _axisPen = new(Brushes.DimGray, 0.1);
    private readonly Pen _shapeBorderPen = new(Brushes.Black, 1.0);

    public void Render(DrawingContext dc, IGame game, double actualWidth, double actualHeight)
    {
        for (int i = 1; i < GameConstants.FieldWidth; i++)
        {
            dc.DrawLine(_axisPen, new(i * GameConstants.PieceSize, 0), new(i * GameConstants.PieceSize, GameConstants.FieldHeight * GameConstants.PieceSize));
        }

        for (int i = 1; i < GameConstants.FieldHeight; i++)
        {
            dc.DrawLine(_axisPen, new(0, i * GameConstants.PieceSize), new(GameConstants.FieldWidth * GameConstants.PieceSize, i * GameConstants.PieceSize));
        }

        foreach (var shape in game.Shapes)
        {
            var geo = new StreamGeometry();
            using (var ctx = geo.Open())
            {
                var edgePoints = shape.EdgePoints;
                ctx.BeginFigure(new(edgePoints[0].X, actualHeight - edgePoints[0].Y), true, true);
                for (var i = 1; i < edgePoints.Length; i++)
                {
                    ctx.LineTo(new(edgePoints[i].X, actualHeight - edgePoints[i].Y), true, false);
                }
            }
            geo.Freeze();
            dc.DrawGeometry(ShapeColors.GetBrush(shape.Kind), _shapeBorderPen, geo);
        }

        if (game.State == GameState.Over)
        {
            var text = new FormattedText(
                "GAME\nOVER", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new(new("Consolas"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal), 120.0, Brushes.Black, 1.0);
            dc.DrawText(text, new((actualWidth - text.Width) / 2.0 + 4.0, (actualHeight - text.Height) / 2.0 + 4.0));

            text = new FormattedText(
                "GAME\nOVER", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new(new("Consolas"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal), 120.0, Brushes.DarkGray, 1.0);
            dc.DrawText(text, new((actualWidth - text.Width) / 2.0, (actualHeight - text.Height) / 2.0));
        }
    }
}
