// Day 4: Camp Cleanup

namespace AdventOfCode.task._2022;

public class Task04 : ITask
{
    private class Ranges
    {
        internal class Sector
        {
            public int From { get; set; }
            public int To { get; set; }

            public Sector(string input)
            {
                var borders = input.Split('-');
                From = int.Parse(borders[0]);
                To = int.Parse(borders[1]);
            }

            public bool Contains(Sector s)
            {
                return From <= s.From && To >= s.To;
            }
        }

        public Sector Sector1 { get; set; }
        public Sector Sector2 { get; set; }

        public Ranges(string input)
        {
            var sectors = input.Split(',');
            Sector1 = new Sector(sectors[0]);
            Sector2 = new Sector(sectors[1]);
        }

        public bool Contains()
        {
            return Sector1.Contains(Sector2) || Sector2.Contains(Sector1);
        }

        public bool Overlaps()
        {
            return ((Sector1.From >= Sector2.From && Sector1.From <= Sector2.To) ||
                    (Sector1.To >= Sector2.From && Sector1.To <= Sector2.To)) ||
                   ((Sector2.From >= Sector1.From && Sector2.From <= Sector1.To) ||
                    (Sector2.To >= Sector1.From && Sector2.To <= Sector1.To));
        }
    }

    public void Solve(string[] lines)
    {
        var sum = 0;
        foreach (var line in lines)
        {
            var r = new Ranges(line);
            sum += r.Contains() ? 1 : 0;
        }

        Console.WriteLine(sum);
    }

    public void Solve2(string[] lines)
    {
        var sum = 0;
        foreach (var line in lines)
        {
            var r = new Ranges(line);
            sum += r.Overlaps() ? 1 : 0;
        }

        Console.WriteLine(sum);
    }
}