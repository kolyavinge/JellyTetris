using System.Diagnostics;
using JellyTetris.Core;

namespace ProfilerApp.Profiles;

internal class MaxTimeProfile
{
    public void Run()
    {
        Update(4000, 12);

        Console.WriteLine("done.");
        Console.ReadKey();
    }

    private void Update(int frames, double maxTime)
    {
        var game = GameFactory.Make();

        var result = new List<TimeResult>();
        for (int i = 0; i < frames; i++)
        {
            var sw = Stopwatch.StartNew();
            game.Update();
            sw.Stop();
            if (sw.Elapsed.TotalMilliseconds > maxTime)
            {
                result.Add(new() { Frame = i, Time = sw.Elapsed.TotalMilliseconds });
            }
        }

        result = result.OrderByDescending(x => x.Time).ToList();
        Console.WriteLine($"Count: {result.Count}");
        Console.WriteLine("");
        if (result.Count > 100) result = result.Take(100).ToList();
        foreach (var item in result)
        {
            Console.WriteLine($"Time: {item.Time:F3}\tFrame: {item.Frame}");
        }
    }

    struct TimeResult
    {
        public int Frame;
        public double Time;
    }
}
