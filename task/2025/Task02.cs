namespace AdventOfCode.task._2025;

public class Task02 : ITask
{
    public void Solve(string[] lines)
    {
        var ranges = lines[0]
            .Split(",")
            .Select(e => e.Split("-"))
            .Select(e => (e[0], e[1]))
            .ToArray();
        var invalidIds = new List<long>();

        foreach (var range in ranges)
        {
            if (Math.Abs(range.Item1.Length - range.Item2.Length) > 1) throw new Exception("Ou nou :(");
            if (range.Item1.Length == range.Item2.Length && range.Item1.Length % 2 == 1) continue;
            var start = long.Parse(range.Item1);
            var end = long.Parse(range.Item2);
            if (range.Item1.Length % 2 == 1)
            {
                start = long.Parse("1".PadRight(range.Item2.Length, '0'));
                end = long.Parse(range.Item2);
            }
            else if (range.Item2.Length % 2 == 1)
            {
                start = long.Parse(range.Item1);
                end = long.Parse("".PadRight(range.Item1.Length, '9'));
            }

            var startStr = start.ToString();

            var leftPart = long.Parse(startStr[..(startStr.Length / 2)]);

            var number = long.Parse($"{leftPart}{leftPart}");
            while (number <= end)
            {
                if (number >= start)
                    invalidIds.Add(number);
                leftPart++;
                number = long.Parse($"{leftPart}{leftPart}");
            }
        }

        Console.WriteLine(invalidIds.Sum());
    }

    public void Solve2(string[] lines)
    {
        var ranges = lines[0]
            .Split(",")
            .Select(e => e.Split("-"))
            .Select(e => (e[0], e[1]))
            .ToArray();
        var invalidIds = new List<long>();

        foreach (var range in ranges)
        {
            if (Math.Abs(range.Item1.Length - range.Item2.Length) > 1) throw new Exception("Ou nou :(");
            var start = long.Parse(range.Item1);
            var end = long.Parse(range.Item2);

            var newIds = new List<long>();
            if (range.Item1.Length == range.Item2.Length)
                newIds.AddRange(FindInvalidIds(start, end));
            else
            {
                var endShorter = long.Parse("".PadRight(range.Item1.Length, '9'));
                var startLonger = long.Parse("1".PadRight(range.Item2.Length, '0'));
                newIds.AddRange(FindInvalidIds(start, endShorter));
                newIds.AddRange(FindInvalidIds(startLonger, end));
            }
            
            // Console.WriteLine(string.Join(", ", newIds));
            invalidIds.AddRange(newIds);
        }

        Console.WriteLine(invalidIds.Sum()); // 37652107449 too high :(
    }

    private List<long> FindInvalidIds(long from, long to)
    {
        var fromStr = from.ToString();
        if (fromStr.Length != to.ToString().Length)
            throw new Exception("Ou nou :(");
        var invalidIds = new List<long>();

        for (int i = 2; i <= to.ToString().Length; i++)
        {
            if (fromStr.Length % i != 0) continue;
            var partLength = fromStr.Length / i;
            var leftPartStr = fromStr[..partLength];
            var number = long.Parse(string.Concat(Enumerable.Repeat(leftPartStr, i)));
            var part = long.Parse(leftPartStr);
            while (number <= to)
            {
                if (number >= from)
                    invalidIds.Add(number);
                part++;
                number = long.Parse(string.Concat(Enumerable.Repeat(part.ToString(), i)));
            }
        }

        return invalidIds.Distinct().ToList();
    }
}
