// Day 7: Bridge Repair

namespace AdventOfCode.task._2024;

public class Task07 : ITask
{
    //done in 9 minutes
    public void Solve(string[] lines)
    {
        var sum = 0L;

        foreach (var line in lines)
        {
            var s = line.Split(':');
            var expectedResult = long.Parse(s[0]);
            var operands = s[1].Split(' ').Skip(1).Select(long.Parse).ToArray();
            var results = new List<long> { operands[0] };
            foreach (var o in operands.Skip(1))
            {
                var newResults = new List<long>();
                foreach (var result in results)
                {
                    newResults.Add(result + o);
                    newResults.Add(result * o);
                }

                results = newResults;
            }

            if (results.Any(r => r == expectedResult))
                sum += expectedResult;
        }

        Console.WriteLine(sum);
    }

    //done in 12 minutes
    public void Solve2(string[] lines)
    {
        var sum = 0L;

        foreach (var line in lines)
        {
            var s = line.Split(':');
            var expectedResult = long.Parse(s[0]);
            var operands = s[1].Split(' ').Skip(1).Select(long.Parse).ToArray();
            var results = new List<long> { operands[0] };
            foreach (var o in operands.Skip(1))
            {
                var newResults = new List<long>();
                foreach (var result in results)
                {
                    newResults.Add(result + o);
                    newResults.Add(result * o);
                    newResults.Add(long.Parse(result.ToString() + o));
                }

                results = newResults;
            }

            if (results.Any(r => r == expectedResult))
                sum += expectedResult;
        }

        Console.WriteLine(sum);
    }
}
