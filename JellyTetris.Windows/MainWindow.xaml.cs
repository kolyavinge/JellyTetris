using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using JellyTetris.Core;

namespace JellyTetris.Windows;

public partial class MainWindow : Window
{
    private readonly IGame _game;
    private readonly DispatcherTimer _timer;

    public MainWindow()
    {
        InitializeComponent();
        _game = GameFactory.Make();
        _fieldGrid.Game = _game;
        _timer = new DispatcherTimer(DispatcherPriority.Render);
        _timer.Interval = TimeSpan.FromMilliseconds(10);
        _timer.Tick += OnGameUpdate;
        _timer.Start();
    }

    private void OnGameUpdate(object? sender, EventArgs e)
    {
#if DEBUG
        var sw = System.Diagnostics.Stopwatch.StartNew();
        _game.Update();
        sw.Stop();
        if (sw.Elapsed.TotalMilliseconds > _timer.Interval.TotalMilliseconds)
        {
            ModelingTextBlock.Text = $"Modeling time: {sw.Elapsed.TotalMilliseconds:F0}";
        }
        else
        {
            ModelingTextBlock.Text = "";
        }
        _fieldGrid.InvalidateVisual();
#endif
#if RELEASE
        _game.Update();
        _fieldGrid.InvalidateVisual();
#endif
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Left)
        {
            _game.MoveCurrentShapeLeft();
        }
        else if (e.Key == Key.Right)
        {
            _game.MoveCurrentShapeRight();
        }
        else if (e.Key == Key.Down)
        {
            _game.DropCurrentShape();
        }
        else if (e.Key == Key.Up)
        {
            _game.RotateCurrentShape();
        }
    }
}
