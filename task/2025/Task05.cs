namespace AdventOfCode.task._2025;

public class Task05 : ITask
{
    public void Solve(string[] lines)
    {
        var idx = 0;
        while (!string.IsNullOrWhiteSpace(lines[idx])) idx++;
        var freshRanges = lines[..idx].Select(e =>
        {
            var n = e.Split('-').Select(long.Parse).ToArray();
            return (n[0], n[1]);
        }).ToList();
        var ids = lines.Skip(idx + 1).Select(long.Parse).ToArray();

        var fresh = 0;
        foreach (var id in ids)
        {
            var isFresh = false;
            foreach (var (from, to) in freshRanges)
            {
                if (id >= from && id <= to)
                {
                    isFresh = true;
                    break;
                }
            }

            if (isFresh) fresh++;
        }

        Console.WriteLine(fresh); // 365 too low
    }

    public void Solve2(string[] lines)
    {
        var ranges = lines.TakeWhile(e => !string.IsNullOrWhiteSpace(e))
            .Select(e =>
            {
                var n = e.Split('-').Select(long.Parse).ToArray();
                return (n[0], n[1]);
            })
            .ToArray<(long from, long to)>();

        var merged = new List<(long from, long to)>(ranges);
        while (!merged.All(e1 => merged.All(e2 => e1 == e2 || e1.to < e2.from || e1.from > e2.to)))
        {
            var newMerged = new List<(long from, long to)>();
            var used = new bool[merged.Count];
            for (var i = 0; i < merged.Count; i++)
            {
                if (used[i]) continue;
                var current = merged[i];
                for (var j = i + 1; j < merged.Count; j++)
                {
                    if (used[j]) continue;
                    var other = merged[j];
                    if (current.to >= other.from && current.from <= other.to)
                    {
                        current = (Math.Min(current.from, other.from), Math.Max(current.to, other.to));
                        used[j] = true;
                    }
                }

                newMerged.Add(current);
                used[i] = true;
            }

            merged = newMerged;
        }
        
        Console.WriteLine(merged.Sum(r=> r.to - r.from + 1));
    }
}
