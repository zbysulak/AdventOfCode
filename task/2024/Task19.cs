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

    public void Solve2(string[] lines)
    {
        _towels = lines[0].Split(", ");

        var possibleDesigns = 0;
        foreach (var p in lines.Skip(2))
        {
            var results = 0;
            GetPossibleArrangements(p, ref results);
            possibleDesigns += results;
            Console.WriteLine($"{p} can be done in {results} ways");
        }

        Console.WriteLine(possibleDesigns);
    }

    private void GetPossibleArrangements(string design, ref int count)
    {
        if (design.Length == 0)
        {
            count++;
            return;
        }

        foreach (var t in _towels)
        {
            var tl = t.Length;
            if (tl > design.Length)
                continue;

            if (new string(design.Take(tl).ToArray()) == t)
            {
                var rest = new string(design.Skip(tl).ToArray());
                GetPossibleArrangements(rest, ref count);
            }
        }
    }
}
