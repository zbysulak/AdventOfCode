// Day 19

namespace AdventOfCode.task._2024;

public class Task19 : ITask
{
    public void Solve(string[] lines)
    {
        var towels = lines[0].Split(", ");

        var possibleDesigns = lines.Skip(2).Count(p => IsPossible(p, towels));

        Console.WriteLine(possibleDesigns);
    }

    private bool IsPossible(string design, string[] towels)
    {
        if (design.Length == 0) return true;
        if (towels.Any(t => t == design))
            return true;

        return towels.Any(t =>
        {
            var tl = t.Length;
            if (tl > design.Length)
                return false;

            if (new string(design.Take(tl).ToArray()) == t)
            {
                var rest = new string(design.Skip(tl).ToArray());
                return IsPossible(rest, towels);
            }

            return false;
        });
    }

    private string[] _towels;
    private Dictionary<string, long> _lookup;

    public void Solve2(string[] lines)
    {
        _towels = lines[0].Split(", ");
        _lookup = new Dictionary<string, long>();
        var possibleDesigns = 0L;

        foreach (var p in lines.Skip(2))
        {
            var results = GetPossibleArrangements(p);

            possibleDesigns += results;
            // Console.WriteLine($"{p} can be done in {results} ways");
        }

        Console.WriteLine(possibleDesigns);
    }

    private long GetPossibleArrangements(string design)
    {
        if (design.Length == 0)
        {
            return 1;
        }

        if (_lookup.TryGetValue(design, out var arrangements))
        {
            // Console.WriteLine("\tlooked up value for - " + design);
            return arrangements;
        }

        var ways = 0L;
        foreach (var t in _towels)
        {
            var tl = t.Length;
            if (tl > design.Length)
                continue;

            if (new string(design.Take(tl).ToArray()) == t)
            {
                var rest = new string(design.Skip(tl).ToArray());
                ways += GetPossibleArrangements(rest);
            }
        }

        _lookup[design] = ways;
        return ways;
    }
}
