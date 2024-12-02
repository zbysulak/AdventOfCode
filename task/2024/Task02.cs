// Day 2: Red-Nosed Reports

namespace AdventOfCode.task._2024;

public class Task02 : ITask
{
    public void Solve(string[] lines)
    {
        var safe = 0;
        foreach (var line in lines)
        {
            var d = line.Split(" ").Select(int.Parse).ToArray();
            var inc = d[0] < d[1];
            var s = true;
            for (var i = 1; i < d.Length; i++)
            {
                var diff = d[i] - d[i - 1];
                if ((!inc && diff is >= -3 and <= -1) || (inc && diff is >= 1 and <= 3))
                    continue;
                else
                    s = false;
            }

            if (s) safe++;
        }

        Console.WriteLine(safe);
    }

    private bool IsSafe(List<int> report)
    {
        var inc = report[0] < report[1];
        var s = true;
        for (var i = 1; i < report.Count; i++)
        {
            var diff = report[i] - report[i - 1];
            if (!Check(diff, inc))
                s = false;
        }

        return s;
    }

    // done in 39 minutes
    public void Solve2(string[] lines)
    {
        var safe = 0;
        foreach (var line in lines)
        {
            var d = line.Split(" ").Select(int.Parse).ToList();
            if (IsSafe(d))
            {
                safe++;
                continue;
            }
            else
            {
                for (var i = 0; i < d.Count; i++)
                {
                    var shorter = d.Take(i).Concat(d.TakeLast(d.Count - 1-i)).ToList();
                    if (IsSafe(shorter))
                    {
                        safe++;
                        break;
                    }

                }
            }
        }

        Console.WriteLine(safe);
    }

    private static bool Check(int diff, bool inc) => (!inc && diff is >= -3 and <= -1) || (inc && diff is >= 1 and <= 3);
}
