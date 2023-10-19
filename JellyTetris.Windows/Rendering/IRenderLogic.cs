using System.Windows.Media;
using JellyTetris.Core;

namespace JellyTetris.Windows.Rendering;

internal interface IRenderLogic
{
    void Render(DrawingContext dc, IGame game, double actualHeight);
}
