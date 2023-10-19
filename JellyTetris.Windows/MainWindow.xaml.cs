using System;
using System.Windows;
using System.Windows.Media;
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
        var sw = System.Diagnostics.Stopwatch.StartNew();
        _game.Update();
        sw.Stop();
        ModelingTextBlock.Text = $"Modeling time: {sw.Elapsed.TotalMilliseconds:F0}";
        if (sw.Elapsed.TotalMilliseconds > _timer.Interval.TotalMilliseconds)
        {
            ModelingTextBlock.Foreground = Brushes.Red;
        }
        else
        {
            ModelingTextBlock.Foreground = Brushes.Gray;
        }
        _fieldGrid.InvalidateVisual();
    }
}
