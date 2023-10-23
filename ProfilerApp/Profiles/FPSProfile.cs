using System.Diagnostics;
using JellyTetris.Core;

namespace ProfilerApp.Profiles;

internal class FPSProfile
{
    public void Run()
    {
        Update(100);
        Update(250);
        Update(500);
        Update(750);
        Update(1000);
        Update(1250);
        Update(1500);
        Update(2000);
        Update(3000);

        Console.WriteLine("done.");
        Console.ReadKey();
    }

    void Update(int frames)
    {
        var game = GameFactory.Make();
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < frames; i++)
        {
            game.Update();
        }
        sw.Stop();
        Console.WriteLine($"Frames: {frames}\tTime: {sw.Elapsed}\tFPS: {1000 * frames / (int)sw.Elapsed.TotalMilliseconds}");
    }
}
