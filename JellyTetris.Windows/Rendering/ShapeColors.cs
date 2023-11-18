using System;
using System.Windows.Media;
using JellyTetris.Model;

namespace JellyTetris.Windows.Rendering;

internal static class ShapeColors
{
    public static readonly Color CubeColor = new Color { A = 255, R = 148, G = 0, B = 52 };
    public static readonly Color LineColor = new Color { A = 255, R = 91, G = 0, B = 148 };
    public static readonly Color TColor = new Color { A = 255, R = 148, G = 94, B = 0 };
    public static readonly Color L1Color = new Color { A = 255, R = 0, G = 84, B = 148 };
    public static readonly Color L2Color = new Color { A = 255, R = 0, G = 148, B = 113 };
    public static readonly Color S1Color = new Color { A = 255, R = 10, G = 148, B = 0 };
    public static readonly Color S2Color = new Color { A = 255, R = 146, G = 148, B = 0 };

    public static readonly Brush CubeBrush = new SolidColorBrush(CubeColor);
    public static readonly Brush LineBrush = new SolidColorBrush(LineColor);
    public static readonly Brush TBrush = new SolidColorBrush(TColor);
    public static readonly Brush L1Brush = new SolidColorBrush(L1Color);
    public static readonly Brush L2Brush = new SolidColorBrush(L2Color);
    public static readonly Brush S1Brush = new SolidColorBrush(S1Color);
    public static readonly Brush S2Brush = new SolidColorBrush(S2Color);

    public static Brush GetBrush(ShapeKind shapeKind)
    {
        return shapeKind switch
        {
            ShapeKind.Cube => CubeBrush,
            ShapeKind.Line => LineBrush,
            ShapeKind.T => TBrush,
            ShapeKind.L1 => L1Brush,
            ShapeKind.L2 => L2Brush,
            ShapeKind.S1 => S1Brush,
            ShapeKind.S2 => S2Brush,
            _ => throw new ArgumentException()
        };
    }
}
