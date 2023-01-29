namespace AdventOfCode;

public class Task11 : ITask
{
    private class Monkey
    {
        public Queue<long> Items { get; init; }
        public int Test { get; init; }
        public int TestPassed { get; init; }
        public int TestFailed { get; init; }
        public Func<long, long> Operation { get; init; }
    }

    private List<Monkey> TestMonkeys = new List<Monkey>
    {
        new Monkey
        {
            Items = new Queue<long>(new[] { 79L, 98 }),
            Operation = old => old * 19,
            Test = 23,
            TestPassed = 2,
            TestFailed = 3
        },
        new Monkey
        {
            Items = new Queue<long>(new[] { 54L, 65, 75, 74 }),
            Operation = old => old + 6,
            Test = 19,
            TestPassed = 2,
            TestFailed = 0,
        },
        new Monkey
        {
            Items = new Queue<long>(new[] { 79L, 60, 97 }),
            Operation = old => old * old,
            Test = 13,
            TestPassed = 1,
            TestFailed = 3,
        },
        new Monkey
        {
            Items = new Queue<long>(new[] { 74L }),
            Operation = old => old + 3,
            Test = 17,
            TestPassed = 0,
            TestFailed = 1,
        }
    };

    private List<Monkey> Monkeys = new List<Monkey>
    {
        new Monkey
        {
            Items = new Queue<long>(new[] { 65L, 58, 93, 57, 66 }),
            Operation = old => old * 7,
            Test = 19,
            TestPassed = 6,
            TestFailed = 4
        },
        new Monkey
        {
            Items = new Queue<long>(new[] { 76L, 97, 58, 72, 57, 92, 82 }),
            Operation = old => old + 4,
            Test = 3,
            TestPassed = 7,
            TestFailed = 5
        },
        new Monkey
        {
            Items = new Queue<long>(new[] { 90L, 89, 96 }),
            Operation = old => old * 5,
            Test = 13,
            TestPassed = 5,
            TestFailed = 1
        },
        new Monkey
        {
            Items = new Queue<long>(new[] { 72L, 63, 72, 99 }),
            Operation = old => old * old,
            Test = 17,
            TestPassed = 0,
            TestFailed = 4
        },
        new Monkey
        {
            Items = new Queue<long>(new[] { 65L }),
            Operation = old => old + 1,
            Test = 2,
            TestPassed = 6,
            TestFailed = 2
        },
        new Monkey
        {
            Items = new Queue<long>(new[] { 97L, 71 }),
            Operation = old => old + 8,
            Test = 11,
            TestPassed = 7,
            TestFailed = 3
        },
        new Monkey
        {
            Items = new Queue<long>(new[] { 83L, 68, 88, 55, 87, 67 }),
            Operation = old => old + 2,
            Test = 5,
            TestPassed = 2,
            TestFailed = 1
        },
        new Monkey
        {
            Items = new Queue<long>(new[] { 64L, 81, 50, 96, 82, 53, 62, 92 }),
            Operation = old => old + 5,
            Test = 7,
            TestPassed = 3,
            TestFailed = 0
        },
    };

    // input is not used in this task because parsing would be too complicated
    public void Solve(string[] lines)
    {
        var monkeys = Monkeys;
        var inspections = new int[monkeys.Count];
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < monkeys.Count; j++)
            {
                while (monkeys[j].Items.Any())
                {
                    inspections[j]++;

                    var item = monkeys[j].Items.Dequeue();
                    item = monkeys[j].Operation(item);
                    item = item / 3;

                    monkeys[item % monkeys[j].Test == 0 ? monkeys[j].TestPassed : monkeys[j].TestFailed].Items
                        .Enqueue(item);
                }
            }
        }

        Array.Sort(inspections);
        Console.WriteLine(inspections[^1] * inspections[^2]);
    }

    public void Solve2(string[] lines)
    {
        var monkeys = Monkeys;
        var inspections = new int[monkeys.Count];

        var M = 1;
        foreach (var monkey in monkeys)
        {
            M *= monkey.Test;
        }

        for (int i = 1; i <= 10000; i++)
        {
            for (int j = 0; j < monkeys.Count; j++)
            {
                while (monkeys[j].Items.Any())
                {
                    inspections[j]++;

                    var item = monkeys[j].Items.Dequeue();
                    item = monkeys[j].Operation(item) % M;
                    //item = item / 3;

                    monkeys[item % monkeys[j].Test == 0 ? monkeys[j].TestPassed : monkeys[j].TestFailed].Items
                        .Enqueue(item);
                }
            }

            if (i <= 20 || i % 1000 == 0)
            {
                Console.WriteLine($"After round {i}");
                for (int q = 0; q < inspections.Length; q++)
                {
                    Console.WriteLine($"Monkey {q} inspected items {inspections[q]} times");
                }
            }
        }

        Array.Sort(inspections);
        Console.WriteLine((long)inspections[^1] * (long)inspections[^2]);
    }
}