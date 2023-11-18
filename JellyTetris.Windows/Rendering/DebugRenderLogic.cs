using System.Windows.Media;
using JellyTetris.Core;
using JellyTetris.Model;

namespace JellyTetris.Windows.Rendering;

internal class DebugRenderLogic : IRenderLogic
{
    private readonly Pen _axisPen = new(Brushes.DimGray, 0.1);

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
            DrawShape(dc, shape, actualHeight);
        }
    }

    private void DrawShape(DrawingContext dc, IShape shape, double actualHeight)
    {
        foreach (var part in shape.Parts)
        {
            foreach (var line in part.Lines)
            {
                var pen = line.IsEdge ? new Pen(Brushes.Green, 0.8) : new Pen(Brushes.DimGray, 0.8);
                dc.DrawLine(pen, new(line.From.X, actualHeight - line.From.Y), new(line.To.X, actualHeight - line.To.Y));
                dc.DrawEllipse(Brushes.Red, null, new(line.From.X, actualHeight - line.From.Y), 1.0, 1.0);
                dc.DrawEllipse(Brushes.Red, null, new(line.To.X, actualHeight - line.To.Y), 1.0, 1.0);
            }
        }
    }
}
