// Day 7: Bridge Repair

namespace AdventOfCode.task._2024;

public class Task07 : ITask
{
    //done in 9 minutes
    public void Solve(string[] lines)
    {
        Console.WriteLine(FindValidEquations(lines,
            new Func<long, long, long>[]
            {
                (r, o) => r + o,
                (r, o) => r * o
            })
        );
    }

    private static long FindValidEquations(string[] lines, IEnumerable<Func<long, long, long>> operators)
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
                    foreach (var op in operators)
                    {
                        newResults.Add(op(result, o));
                    }
                }

                results = newResults;
            }

            if (results.Any(r => r == expectedResult))
                sum += expectedResult;
        }

        return sum;
    }

    //done in 12 minutes
    public void Solve2(string[] lines)
    {
        Console.WriteLine(FindValidEquations(lines,
            new Func<long, long, long>[]
            {
                (r, o) => r + o,
                (r, o) => r * o,
                (r, o) => long.Parse(r.ToString() + o)
            })
        );
    }
}
