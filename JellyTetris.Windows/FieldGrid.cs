using System.Windows.Controls;
using System.Windows.Media;
using JellyTetris.Core;
using JellyTetris.Windows.Rendering;

namespace JellyTetris.Windows;

internal class FieldGrid : Grid
{
    private readonly IRenderLogic _renderLogic;

    public IGame? Game { get; set; }

    public FieldGrid()
    {
        //_renderLogic = new RenderLogic();
        _renderLogic = new DebugRenderLogic();
        Width = GameConstants.FieldWidth * GameConstants.PieceSize;
        Height = GameConstants.FieldHeight * GameConstants.PieceSize;
    }

    protected override void OnRender(DrawingContext dc)
    {
        base.OnRender(dc);
        _renderLogic.Render(dc, Game!, ActualWidth, ActualHeight);
    }
}
