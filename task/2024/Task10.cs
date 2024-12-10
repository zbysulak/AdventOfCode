// Day 10: Hoof It

namespace AdventOfCode.task._2024;

public class Task10 : ITask
{
    private static List<(int, int)> GetHeads(string[] lines)
    {
        var trailheads = new List<(int, int)>();
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == '0')
                    trailheads.Add((x, y));
            }
        }

        return trailheads;
    }

    // done in 33 minutes
    public void Solve(string[] lines)
    {
        var trailheads = GetHeads(lines);

        var ends = new List<(int, int)>();
        foreach (var trailhead in trailheads)
        {
            var validSteps = new HashSet<(int, int)> { trailhead };
            for (int i = 1; i < 10; i++) // looking for 10 steps
            {
                var nextSteps = new HashSet<(int, int)>();
                foreach (var step in validSteps)
                {
                    foreach (var dir in Utils.Directions4)
                    {
                        if (!Utils.CheckBounds(lines, step.Item1 + dir[0], step.Item2 + dir[1])) continue;
                        var next = (step.Item1 + dir[0], step.Item2 + dir[1]);
                        if (lines[next.Item2][next.Item1] == i.ToString()[0])
                        {
                            nextSteps.Add(next);
                        }
                    }
                }

                validSteps = nextSteps;
            }

            ends.AddRange(validSteps);
        }

        Console.WriteLine(ends.Count);
    }

    //done in 37 minutes
    public void Solve2(string[] lines)
    {
        var trailheads = GetHeads(lines);

        var ends = new List<(int, int)>();
        foreach (var trailhead in trailheads)
        {
            var validSteps = new List<(int, int)> { trailhead };
            for (int i = 1; i < 10; i++) // looking for 10 steps
            {
                var nextSteps = new List<(int, int)>();
                foreach (var step in validSteps)
                {
                    foreach (var dir in Utils.Directions4)
                    {
                        if (!Utils.CheckBounds(lines, step.Item1 + dir[0], step.Item2 + dir[1])) continue;
                        var next = (step.Item1 + dir[0], step.Item2 + dir[1]);
                        if (lines[next.Item2][next.Item1] == i.ToString()[0])
                        {
                            nextSteps.Add(next);
                        }
                    }
                }

                validSteps = nextSteps;
            }

            ends.AddRange(validSteps);
        }

        Console.WriteLine(ends.Count);
    }
}
