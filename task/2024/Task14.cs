// Day 14

using System.Text.RegularExpressions;

namespace AdventOfCode.task._2024;

public class Task14 : ITask
{
    public static (int w, int h) _gridSize;

    public class Robot
    {
        private const string Pattern = @"p=(\d+),(\d+) v=(-?\d+),(-?\d+)";
        public (int x, int y) Start { get; set; }
        public (int x, int y) Position { get; set; }
        public (int dx, int dy) Velocity { get; set; }

        public Robot(string input)
        {
            var a = Regex.Match(input, Pattern);
            Start = (int.Parse(a.Groups[1].Value), int.Parse(a.Groups[2].Value));
            Velocity = (int.Parse(a.Groups[3].Value), int.Parse(a.Groups[4].Value));
            Position = Start;
            Predict(100);
        }

        public (int x, int y) Predict(int steps)
        {
            Position = ((((Start.x + Velocity.dx * steps) % _gridSize.w) + _gridSize.w) % _gridSize.w,
                (((Start.y + Velocity.dy * steps) % _gridSize.h) + _gridSize.h) % _gridSize.h);
            return Position;
        }
    }

    public void Solve(string[] lines)
    {
        _gridSize = lines.Length < 20 ? (11, 7) : (101, 103);

        var robots = lines.Select(l => new Robot(l));

        var q1 = robots.Count(r => r.Position.x < _gridSize.w / 2 && r.Position.y < _gridSize.h / 2);
        var q2 = robots.Count(r => r.Position.x > _gridSize.w / 2 && r.Position.y < _gridSize.h / 2);
        var q3 = robots.Count(r => r.Position.x < _gridSize.w / 2 && r.Position.y > _gridSize.h / 2);
        var q4 = robots.Count(r => r.Position.x > _gridSize.w / 2 && r.Position.y > _gridSize.h / 2);

        Console.WriteLine(q1 * q2 * q3 * q4);
    }

    public void Solve2(string[] lines)
    {
        _gridSize = lines.Length < 20 ? (11, 7) : (101, 103);

        var robots = lines.Select(l => new Robot(l)).ToList();
        // I gave this up because I did not expect it to happen so late
        var suspects = new HashSet<int>();

        for (int t = 7500; t < 7600; t++)
        {
            var pos = robots.Select(r => r.Predict(t)).ToList();
            const int cluster = 5;
            for (int i = 0; i < _gridSize.h; i++)
            {
                for (int j = 0; j < _gridSize.w - cluster; j++)
                {
                    var line = true;
                    for (int k = 0; k < cluster; k++)
                    {
                        var c = pos.Count(r => r.x == j + k && r.y == i);
                        if (c == 0)
                        {
                            line = false;
                            break;
                        }
                    }

                    if (line)
                    {
                        suspects.Add(t);
                        Console.WriteLine("x" + j + "y" + i);
                    }
                }
            }
        }

        foreach (var suspect in suspects)
        {
            var pos = robots.Select(r => r.Predict(suspect)).ToList();
            Console.WriteLine("\n\nseconds " + suspect);
            for (int i = 0; i < _gridSize.h; i++)
            {
                for (int j = 0; j < _gridSize.w; j++)
                {
                    var c = pos.Count(r => r.x == j && r.y == i);
                    Console.Write(c > 0 ? c : ".");
                }

                Console.WriteLine();
            }
        }
    }

/*
helper code to visualise y2024d14
int step = 0;
while (true)
{
    var a = Console.ReadLine();
    if (int.TryParse(a, out var i) && i > 0)
    {
        step += i;
    }
    else
    {
        step++;
    }

    task.Solve2(lines, step);
}
*/

    // method to print state in second in argument
    public void Solve2(string[] lines, int step = 0)
    {
        _gridSize = lines.Length < 20 ? (11, 7) : (101, 103);

        var robots = lines.Select(l => new Robot(l)).ToList();

        var pos = robots.Select(r => r.Predict(step)).ToList();
        Console.WriteLine("\n\nseconds " + step);
        for (int i = 0; i < _gridSize.h; i++)
        {
            for (int j = 0; j < _gridSize.w; j++)
            {
                var c = pos.Count(r => r.x == j && r.y == i);
                Console.Write(c > 0 ? c : ".");
            }

            Console.WriteLine();
        }
    }
}
