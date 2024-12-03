// Day 3

using System.Text.RegularExpressions;

namespace AdventOfCode.task._2024;

public class Task03 : ITask
{
    private const string Pattern = @"mul\((\d{1,3}),(\d{1,3})\)";

    //done in 15 minutes
    public void Solve(string[] lines)
    {
        var sum = 0;
        foreach (var line in lines)
        {
            var matches = Regex.Matches(line, Pattern);
            foreach (var match in matches.ToList())
            {
                sum += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
            }
        }
        Console.WriteLine(sum);
    }

    //done in 27 minutes
    public void Solve2(string[] lines)
    {
        var sum = 0;
        var enabled = true;
        foreach (var line in lines)
        {
            for (int i = 0; i < line.Length; i++)
            {
                var s = line.Substring(i);
                var doo = Regex.Match(s, @"^do\(\)");
                if (doo.Success)
                {
                    enabled = true;
                    continue;
                }
                var dont = Regex.Match(s, @"^don't\(\)");
                if (dont.Success)
                {
                    enabled = false;
                    continue;
                }

                var match = Regex.Match(s, "^" + Pattern);
                if (enabled && match.Success)
                    sum += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
            }
        }
        Console.WriteLine(sum);
    }
}
