// Day 1: Calorie Counting

namespace AdventOfCode.task._2022;

public class Task01 : ITask
{
    public void Solve(string[] lines)
    {
        var max = 0;
        var current = 0;
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                if (current > max) max = current;
                current = 0;
            }
            else
            {
                current += int.Parse(line);
            }
        }

        Console.WriteLine(max);
    }


    public void Solve2(string[] lines)
    {
        var sums = new List<int>();
        var current = 0;
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                sums.Add(current);
                current = 0;
            }
            else
            {
                current += int.Parse(line);
            }
        }

        sums.Sort();
        var top3 = 0;
        for (int i = sums.Count() - 1; i > sums.Count() - 4; i--)
        {
            Console.WriteLine(sums[i]);
            top3 += sums[i];
        }

        Console.WriteLine(top3);
    }
}
