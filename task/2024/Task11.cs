// Day 11

using System.Diagnostics;

namespace AdventOfCode.task._2024;

public class Task11 : ITask
{
    // done in 17 minutes
    public void Solve(string[] lines)
    {
        var stones = lines[0].Split(' ').Select(long.Parse).ToList();

        const int blinks = 25;

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < blinks; i++)
        {
            var newStones = new List<long>();
            foreach (var stone in stones)
            {
                if (stone == 0)
                {
                    newStones.Add(1);
                    continue;
                }

                if (stone.ToString().Length % 2 == 0)
                {
                    var strStone = stone.ToString();
                    newStones.Add(long.Parse(strStone[..(stone.ToString().Length / 2)]));
                    newStones.Add(long.Parse(strStone[(stone.ToString().Length / 2)..]));
                    continue;
                }

                newStones.Add(stone * 2024);
            }

            stones = newStones;
        }

        Console.WriteLine(stones.Count);
        Console.WriteLine($"it took {sw.ElapsedMilliseconds}ms");
    }

    //done in many hours :(
    public void Solve2(string[] lines)
    {
        var stones = lines[0].Split(' ').Select(long.Parse).ToList();

        var sw = Stopwatch.StartNew();
        Console.WriteLine(stones.Sum(s => GetStones(s, 0)));
        Console.WriteLine($"it took {sw.ElapsedMilliseconds}ms");
    }

    private int blinks = 75;

    private Dictionary<(int, long), long> cache = new(); // (depth, stone) -> stones

    private long GetStones(long stone, int depth)
    {
        if (depth == blinks)
            return 1;

        if (cache.TryGetValue((depth, stone), out var cached))
            return cached;

        long stones;

        if (stone == 0)
        {
            stones = GetStones(1, depth + 1);
        }
        else if (stone.ToString().Length % 2 == 0)
        {
            var strStone = stone.ToString();
            stones = GetStones(long.Parse(strStone[..(strStone.Length / 2)]), depth + 1) +
                     GetStones(long.Parse(strStone[(strStone.Length / 2)..]), depth + 1);
        }
        else
        {
            stones = GetStones(stone * 2024, depth + 1);
        }

        cache[(depth, stone)] = stones;
        return stones;
    }
}
