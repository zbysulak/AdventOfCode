// Day 13: Claw Contraption

using System.Text.RegularExpressions;

namespace AdventOfCode.task._2024;

public class Task13 : ITask
{
    public class ClawMachine
    {
        public (int x, int y) ButtonA { get; set; }
        public (int x, int y) ButtonB { get; set; }
        public (int x, int y) Prize { get; set; }

        public (int a, int b) Sequence { get; set; }
        public int Price => Sequence.a * 3 + Sequence.b;

        public ClawMachine(string[] lines)
        {
            const string pattern = @"X[+=](\d+),\s*Y[+=](\d+)";
            if (lines.Length != 3)
                throw new Exception("Invalid input");
            var a = Regex.Match(lines[0], pattern);
            var b = Regex.Match(lines[1], pattern);
            var p = Regex.Match(lines[2], pattern);
            ButtonA = (int.Parse(a.Groups[1].Value), int.Parse(a.Groups[2].Value));
            ButtonB = (int.Parse(b.Groups[1].Value), int.Parse(b.Groups[2].Value));
            Prize = (int.Parse(p.Groups[1].Value), int.Parse(p.Groups[2].Value));

            FindSequence();
        }

        private void FindSequence()
        {
            // b costs 3, a costs 1
            var b = 0;
            while (b * ButtonB.y < Prize.y && b * ButtonB.x < Prize.x)
            {
                var remainingX = (Prize.x - b * ButtonB.x);
                if (remainingX % ButtonA.x == 0)
                {
                    var a = remainingX / ButtonA.x;
                    if (a * ButtonA.y + b * ButtonB.y == Prize.y)
                    {
                        Sequence = (a, b);
                        // Console.WriteLine($"a: {a}, b: {b} -> {Price}");
                        return;
                    }
                }

                b++;
            }
        }
    }

    public void Solve(string[] lines)
    {
        var clawMachines = new List<ClawMachine>();
        int i = 0;
        while (i * 4 < lines.Length)
        {
            clawMachines.Add(new ClawMachine(lines.Skip(i * 4).Take(3).ToArray()));
            i++;
        }

        Console.WriteLine(clawMachines.Sum(m => m.Price));
    }

    public class ClawMachineV2
    {
        public (long x, long y) ButtonA { get; set; }
        public (long x, long y) ButtonB { get; set; }
        public (long x, long y) Prize { get; set; }

        public (long a, long b) Sequence { get; set; }
        public long Price => Sequence.a * 3 + Sequence.b;

        public ClawMachineV2(string[] lines, long offset = 0)
        {
            const string pattern = @"X[+=](\d+),\s*Y[+=](\d+)";
            if (lines.Length != 3)
                throw new Exception("Invalid input");
            var a = Regex.Match(lines[0], pattern);
            var b = Regex.Match(lines[1], pattern);
            var p = Regex.Match(lines[2], pattern);
            ButtonA = (long.Parse(a.Groups[1].Value), long.Parse(a.Groups[2].Value));
            ButtonB = (long.Parse(b.Groups[1].Value), long.Parse(b.Groups[2].Value));
            Prize = (long.Parse(p.Groups[1].Value) + offset, long.Parse(p.Groups[2].Value) + offset);

            FindSequence();
        }

        private void FindSequence()
        {
            var p11 = Prize.y * ButtonA.x;
            var p12 = Prize.x * ButtonA.y;
            var p1 = p11 - p12;
            var p2 = ButtonB.y * ButtonA.x - ButtonA.y * ButtonB.x;
            var b = p1 / p2;
            if (p1 % p2 == 0 && (Prize.x - ButtonB.x * b) % ButtonA.x == 0) // I have to check if both are divisible!
            {
                Sequence = ((Prize.x - ButtonB.x * b) / ButtonA.x, b);
                Console.WriteLine($"a {Sequence.a}, b {Sequence.b}, price {Price}");
            }
        }
    }

    public void Solve2(string[] lines)
    {
        var clawMachines = new List<ClawMachineV2>();
        int i = 0;
        while (i * 4 < lines.Length)
        {
            clawMachines.Add(new ClawMachineV2(lines.Skip(i * 4).Take(3).ToArray(), 10000000000000));
            i++;
        }

        Console.WriteLine(clawMachines.Sum(m => m.Price));
    }
}
