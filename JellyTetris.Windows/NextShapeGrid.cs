using System.Windows.Controls;
using System.Windows.Media;
using JellyTetris.Core;
using JellyTetris.Windows.Rendering;

namespace JellyTetris.Windows;

internal class NextShapeGrid : Grid
{
    private readonly NextShapeRenderLogic _renderLogic = new NextShapeRenderLogic();

    public IGame? Game { get; set; }

    protected override void OnRender(DrawingContext dc)
    {
        if (Game is null) return;
        _renderLogic.Render(dc, Game!, ActualWidth, ActualHeight);
    }
}
