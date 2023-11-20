using System.Globalization;
using System.Windows;
using System.Windows.Media;
using JellyTetris.Core;

namespace JellyTetris.Windows.Rendering;

internal class RenderLogic : IRenderLogic
{
    private readonly Pen _axisPen = new(Brushes.DimGray, 0.1);
    private readonly Pen _shapeBorderPen = new(Brushes.Black, 0.5);

    public void Render(DrawingContext dc, IGame game, double actualWidth, double actualHeight)
    {
        DrawGrid(dc);
        DrawShapes(dc, game, actualHeight);
        if (game.State == GameState.Over)
        {
            DrawGameOver(dc, actualWidth, actualHeight);
        }
    }

    private void DrawShapes(DrawingContext dc, IGame game, double actualHeight)
    {
        foreach (var shape in game.Shapes)
        {
            foreach (var part in shape.Parts)
            {
                var geo = new StreamGeometry();
                using (var ctx = geo.Open())
                {
                    ctx.BeginFigure(new(part.EdgePoints[0].X, actualHeight - part.EdgePoints[0].Y), true, true);
                    for (var i = 1; i < part.EdgePoints.Length; i++)
                    {
                        ctx.LineTo(new(part.EdgePoints[i].X, actualHeight - part.EdgePoints[i].Y), true, false);
                    }
                }
                geo.Freeze();
                dc.DrawGeometry(ShapeColors.GetBrush(shape.Kind), _shapeBorderPen, geo);
            }
        }
    }

    private void DrawGrid(DrawingContext dc)
    {
        for (int i = 1; i < GameConstants.FieldWidth; i++)
        {
            dc.DrawLine(_axisPen, new(i * GameConstants.PieceSize, 0), new(i * GameConstants.PieceSize, GameConstants.FieldHeight * GameConstants.PieceSize));
        }

        for (int i = 1; i < GameConstants.FieldHeight; i++)
        {
            dc.DrawLine(_axisPen, new(0, i * GameConstants.PieceSize), new(GameConstants.FieldWidth * GameConstants.PieceSize, i * GameConstants.PieceSize));
        }
    }

    private void DrawGameOver(DrawingContext dc, double actualWidth, double actualHeight)
    {
        var text = new FormattedText(
            "GAME\nOVER", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new(new("Consolas"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal), 120.0, Brushes.Black, 1.0);
        dc.DrawText(text, new((actualWidth - text.Width) / 2.0 + 4.0, (actualHeight - text.Height) / 2.0 + 4.0));

        text = new FormattedText(
            "GAME\nOVER", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new(new("Consolas"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal), 120.0, Brushes.DarkGray, 1.0);
        dc.DrawText(text, new((actualWidth - text.Width) / 2.0, (actualHeight - text.Height) / 2.0));
    }
}
