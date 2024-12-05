// Day 5: Print Queue

namespace AdventOfCode.task._2024;

public class Task05 : ITask
{
    //done in 17:24 minutes
    public void Solve(string[] lines)
    {
        var rules = new List<(int, int)>();
        var i = 0;
        while (!string.IsNullOrEmpty(lines[i]))
        {
            var s = lines[i].Split("|").Select(int.Parse).ToArray();
            rules.Add((s[0], s[1]));
            i++;
        }

        i++;

        var updates = new List<List<int>>();
        while (i < lines.Length && !string.IsNullOrEmpty(lines[i]))
        {
            var s = lines[i].Split(",").Select(int.Parse).ToList();
            updates.Add(s);
            i++;
        }

        var sum = 0;

        foreach (var u in updates)
        {
            sum += CheckUpdate(u.ToArray(), rules);
        }

        Console.WriteLine(sum);
    }

    private int CheckUpdate(int[] update, List<(int, int)> rules)
    {
        for (int i = 0; i < update.Length; i++)
        {
            for (int b = 0; b < i; b++)
            {
                // check if there's rule for different order
                if (rules.Any(r => r.Item1 == update[i] && r.Item2 == update[b]))
                    return 0;
            }

            for (int a = i + 1; a < update.Length; a++)
            {
                if (rules.Any(r => r.Item1 == update[a] && r.Item2 == update[i]))
                    return 0;
            }
        }

        return update[update.Length / 2];
    }

    //done in 25:18 minutes
    public void Solve2(string[] lines)
    {
        var rules = new List<(int, int)>();
        var i = 0;
        while (!string.IsNullOrEmpty(lines[i]))
        {
            var s = lines[i].Split("|").Select(int.Parse).ToArray();
            rules.Add((s[0], s[1]));
            i++;
        }

        i++;

        var updates = new List<List<int>>();
        while (i < lines.Length && !string.IsNullOrEmpty(lines[i]))
        {
            var s = lines[i].Split(",").Select(int.Parse).ToList();
            updates.Add(s);
            i++;
        }

        var sum = 0;

        foreach (var u in updates)
        {
            if (CheckUpdate(u.ToArray(), rules) == 0)
            {
                var sorted = Sort(u.ToArray(), rules);
                sum += sorted.ToArray()[sorted.Length / 2];
            }
        }

        Console.WriteLine(sum);
    }

    private int[] Sort(int[] update, List<(int, int)> rules)
    {
        Array.Sort(update, new Sorter(rules));
        return update;
    }

    private class Sorter : IComparer<int>
    {
        private List<(int, int)> _rules;

        public Sorter(List<(int, int)> rules)
        {
            _rules = rules;
        }

        public int Compare(int x, int y)
        {
            return _rules.Any(r => r.Item1 == y && r.Item2 == x) ? -1 : 1;
        }
    }
}
