// Day 22

namespace AdventOfCode.task._2024;

public class Task22 : ITask
{
    private static long MixAndPrune(long current, long next)
    {
        return (current ^ next) % 16777216;
    }

    public void Solve(string[] lines)
    {
        var sum = 0L;
        foreach (var secret in lines.Select(long.Parse))
        {
            var current = secret;
            for (int i = 0; i < 2000; i++)
            {
                current = MixAndPrune(current, current * 64);
                current = MixAndPrune(current, current / 32);
                current = MixAndPrune(current, current * 2048);
            }

            sum += current;
        }

        Console.WriteLine(sum);
    }

    public class LimitedQueue
    {
        private const int Capacity = 4;
        private readonly Queue<int> _queue = new();

        public void Enqueue(int item)
        {
            if (_queue.Count == Capacity)
            {
                _queue.Dequeue();
            }

            _queue.Enqueue(item);
        }

        public List<int> ToList()
        {
            return _queue.ToList();
        }

        public (int, int, int, int) ToTuple()
        {
            var list = ToList();
            return (list[0], list[1], list[2], list[3]);
        }
    }

    public void Solve2(string[] lines)
    {
        var prices = new List<Dictionary<(int, int, int, int), int>>();

        foreach (var secret in lines.Select(long.Parse))
        {
            int lp = (int)secret % 10;
            var q = new LimitedQueue();
            var current = secret;
            var dict = new Dictionary<(int, int, int, int), int>();
            for (int i = 0; i < 2000; i++)
            {
                current = MixAndPrune(current, current * 64);
                current = MixAndPrune(current, current / 32);
                current = MixAndPrune(current, current * 2048);

                var secretDigit = (int)current % 10;

                q.Enqueue(secretDigit - lp);

                lp = secretDigit;

                if (q.ToList().Count == 4)
                {
                    var t = q.ToTuple();
                    dict.TryAdd(t, secretDigit);
                }
            }

            prices.Add(dict);
        }

        var combinations = new List<(int, int, int, int)>();
        for (int i = -9; i < 10; i++)
        {
            for (int j = -9; j < 10; j++)
            {
                for (int k = -9; k < 10; k++)
                {
                    for (int l = -9; l < 10; l++)
                    {
                        combinations.Add((i, j, k, l));
                    }
                }
            }
        }

        var max = 0;

        foreach (var combination in combinations)
        {
            var sum = 0;
            //Console.WriteLine("checked " + combination);
            foreach (var dict in prices)
            {
                if (dict.TryGetValue(combination, out var price))
                {
                    sum += price;
                }
            }

            if (sum > max)
            {
                max = sum;
            }
        }

        Console.WriteLine(max);

        // 1928 is too high
    }
}
