using System.Drawing;

namespace AdventOfCode.task._2022;

public class Task15 : ITask
{
    public record Range
    {
        public static readonly Range Empty;

        public Range(int from, int to)
        {
            if (from < to)
            {
                From = from;
                To = to;
            }
            else
            {
                From = to;
                To = from;
            }
        }

        public int From { get; set; }
        public int To { get; set; }

        public int Length => To - From;

        public void Merge(Range other)
        {
            if (!CanMerge(other))
                throw new Exception("Can't merge these.");

            if (From <= other.From && To <= other.To)
                To = other.To;
            else if (other.From <= From && other.To <= To)
            {
                From = other.From;
            }
            else if (other.From <= From && other.To >= To)
            {
                From = other.From;
                To = other.To;
            } // other option is this contains whole other
        }

        public bool CanMerge(Range other)
        {
            return !(From < other.From && To < other.From) && !(From > other.To && To > other.To);
        }

        public bool TryMerge(Range other)
        {
            if (CanMerge(other))
            {
                Merge(other);
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return $"[{From}, {To}]";
        }
    }

    public class Sensor
    {
        public Point Position { get; set; }
        public Point ClosestBeacon { get; set; }

        public int Distance => Math.Abs(Position.X - ClosestBeacon.X) + Math.Abs(Position.Y - ClosestBeacon.Y);

        public Sensor(string input)
        {
            // input example: Sensor at x=2, y=18: closest beacon is at x=-2, y=15
            var r = input.Substring("Sensor at ".Length).Split(": closest beacon is at ");
            var p = r[0].Split(", ");
            var b = r[1].Split(", ");
            Position = new(int.Parse(p[0].Substring(2)), int.Parse(p[1].Substring(2)));
            ClosestBeacon = new(int.Parse(b[0].Substring(2)), int.Parse(b[1].Substring(2)));
        }
    }

    private int CoverageOnY(IEnumerable<Sensor> sensors, int y)
    {
        return GetRanges(sensors, y).Sum(r => r.Length);
    }

    private IEnumerable<Range> GetRanges(IEnumerable<Sensor> sensors, int y)
    {
        var ranges = sensors.Select(s =>
        {
            if (Math.Abs(y - s.Position.Y) > s.Distance)
            {
                return Range.Empty;
            }
            else
            {
                var horizontal = s.Distance - Math.Abs(y - s.Position.Y);
                return new Range(s.Position.X - horizontal, s.Position.X + horizontal);
            }
        }).Where(r => r != Range.Empty);

        var didntMergeAny = false;
        while (!didntMergeAny)
        {
            didntMergeAny = true;
            var tmpRanges = new List<Range> { ranges.First() };
            foreach (var range in ranges.Skip(1))
            {
                var didThisGetMerged = false;
                foreach (var range2 in tmpRanges)
                {
                    if (range2.TryMerge(range))
                    {
                        didThisGetMerged = true;
                        didntMergeAny = false;
                    }
                }

                if (!didThisGetMerged)
                    tmpRanges.Add(range);
            }

            ranges = tmpRanges;
        }

        return ranges;
    }

    public void Solve(string[] lines)
    {
        var sensors = lines.Select(l => new Sensor(l));
        //Console.WriteLine(CoverageOnY(sensors, 2000000)); // Warning: test data has different (sample:20, test:2000000)
    }

    public void Solve2(string[] lines)
    {
        var size = 4000000; // sample: 20, test: 4000000
        var sensors = lines.Select(l => new Sensor(l));

        // I am really not proud of this :(
        for (int y = 0; y < size; y++)
        {
            var r = GetRanges(sensors, y);
            if (r.Count() == 2)
            {
                Console.WriteLine(y+4000000L*((long)r.First().To+1));
                break;
            }
        }

        /*
         This works but causes out of memory exception
        var chckd = new bool[size, size];

        foreach (var sensor in sensors)
        {
            for (int m = -sensor.Distance; m <= sensor.Distance; m++)
            {
                for (int i = 0; i <= sensor.Distance - Math.Abs(m); i++)
                {
                    var y = sensor.Position.Y - m;
                    if (y < 0 || y >= size) continue;
                    var x = sensor.Position.X - i;
                    if (x >= 0 && x < size)
                    {
                        chckd[y, x] = true;
                    }

                    x = sensor.Position.X + i;
                    if (x >= 0 && x < size)
                    {
                        chckd[y, x] = true;
                    }
                }
            }
        }

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if (sensors.Any(s => s.Position == new Point(x, y)))
                {
                    //Console.Write("S");
                }
                else if (sensors.Any(s => s.ClosestBeacon == new Point(x, y)))
                {
                    //Console.Write("B");
                }
                else if (chckd[y, x])
                {
                    //Console.Write("#");
                }
                else
                {
                    Console.WriteLine(x*4000000+y);
                    //Console.Write(".");
                }
            }
            //Console.WriteLine();
        }*/
    }
}