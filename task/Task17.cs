using System.Drawing;

namespace AdventOfCode;

public class Task17 : ITask
{
    record LongPoint
    {
        public long X { get; set; }
        public long Y { get; set; }

        public LongPoint(long x, long y)
        {
            X = x;
            Y = y;
        }
    }

    class Shape
    {
        public List<Point> Pieces { get; set; }

        private Shape(LongPoint position)
        {
            Position = position;
        }

        public static Shape GetShape(ShapeType type, LongPoint position)
        {
            return new Shape(position)
            {
                Pieces = type switch
                {
                    ShapeType.HLine => new List<Point>
                    {
                        new(0, 0), new(1, 0), new(2, 0), new(3, 0)
                    },
                    ShapeType.Plus => new List<Point> { new(1, 0), new(0, 1), new(1, 1), new(2, 1), new(1, 2) },
                    ShapeType.L => new List<Point> { new(0, 0), new(1, 0), new(2, 0), new(2, 1), new(2, 2) },
                    ShapeType.VLine => new List<Point> { new(0, 0), new(0, 1), new(0, 2), new(0, 3) },
                    ShapeType.Square => new List<Point> { new(0, 0), new(1, 0), new(0, 1), new(1, 1) },
                    _ => throw new Exception("Invalid shape")
                }
            };
        }

        private LongPoint Position { get; set; }

        public bool Move(char dir, List<LongPoint> settledStuff) =>
            dir switch
            {
                '<' => MoveLeft(settledStuff),
                '>' => MoveRight(settledStuff),
                _ => throw new Exception("WTF")
            };

        public IEnumerable<LongPoint> PositionedPieces =>
            Pieces.Select(p => new LongPoint(p.X + Position.X, p.Y + Position.Y));

        private bool MoveRight(List<LongPoint> settledStuff)
        {
            if (Pieces.All(p =>
                    p.X + Position.X + 1 < 7 &&
                    !settledStuff.Contains(new LongPoint(Position.X + p.X + 1, Position.Y + p.Y))))
            {
                Position.X++;
                return true;
            }

            return false;
        }

        private bool MoveLeft(List<LongPoint> settledStuff)
        {
            if (Pieces.All(p =>
                    p.X + Position.X - 1 >= 0 &&
                    !settledStuff.Contains(new LongPoint(Position.X + p.X - 1, Position.Y + p.Y))))
            {
                Position.X--;
                return true;
            }

            return false;
        }

        public bool Fall(List<LongPoint> settledStuff)
        {
            if (Pieces.All(p =>
                    !settledStuff.Contains(new LongPoint(Position.X + p.X, Position.Y + p.Y - 1)) &&
                    Position.Y + p.Y - 1 >= 0))
            {
                Position.Y--;
                return true;
            }

            return false;
        }

        public static int NumberOfShapes => Enum.GetValues<ShapeType>().Length;
    }

    private enum ShapeType
    {
        HLine,
        Plus,
        L,
        VLine,
        Square
    }

    public void Solve(string[] lines)
    {
        var jet = lines[0];
        var settledStuff = new List<LongPoint>();
        Shape fallingShape = null;

        var print = (string comment) =>
        {
            return;
            Console.WriteLine(comment);
            var fsp = fallingShape.PositionedPieces;
            var maxY = settledStuff.Any() ? settledStuff.Max(p => p.Y) + 4 : 3;
            for (long y = maxY; y >= 0; y--)
            {
                Console.Write("|");
                for (int x = 0; x < 7; x++)
                {
                    var p = new LongPoint(x, y);
                    if (fsp.Contains(p))
                        Console.Write("@");
                    else
                        Console.Write(settledStuff.Any(s => s == p) ? "#" : ".");
                }

                Console.WriteLine("|");
            }

            Console.WriteLine("+-------+");
        };

        var nextShape = 0;
        var jetPosition = 0;
        for (int i = 0; i < 2022; i++)
        {
            fallingShape = Shape.GetShape((ShapeType)nextShape,
                new LongPoint(2, settledStuff.Any() ? settledStuff.Max(ss => ss.Y) + 4 : 3));
            print("New shape was spawned");
            while (true)
            {
                fallingShape.Move(jet[jetPosition], settledStuff);

                print($"Jet blew {jet[jetPosition]}");
                jetPosition = (jetPosition + 1) % jet.Length;

                if (!fallingShape.Fall(settledStuff))
                {
                    break;
                }

                print("Piece fell");
            }

            // "towerize" settled shape
            settledStuff.AddRange(fallingShape.PositionedPieces);
            print("Shape has settled");

            nextShape = (nextShape + 1) % Enum.GetValues<ShapeType>().Length;
        }

        Console.WriteLine(settledStuff.Max(ss => ss.Y) + 1);
    }

    private bool CompareTip(List<LongPoint> tip1, List<LongPoint> tip2)
    {
        var lowest1 = tip1.Min(t => t.Y);
        var lowest2 = tip2.Min(t => t.Y);
        var highest1 = tip1.Max(t => t.Y);
        var highest2 = tip2.Max(t => t.Y);
        if (highest1 - lowest1 != highest2 - lowest2) return false;

        tip2 = tip2.Select(p => new LongPoint(p.X, p.Y - highest2 + highest1)).ToList();

        for (int y = 0; y < highest1 - lowest1; y++)
        {
            for (int x = 0; x < 7; x++)
            {
                if (tip1.Contains(new LongPoint(x, y)) != tip2.Contains(new LongPoint(x, y))) return false;
            }
        }

        return true;
    }

    public void Solve2(string[] lines)
    {
        var jet = lines[0];

        var settledStuff = new List<LongPoint>();
        Shape fallingShape = null;

        var nextShape = 0;
        var jetPosition = 0;
        var lastFullRowY = 0L;
        var skippedStuff = false;

        Dictionary<(int, int), (List<LongPoint>, long, long)>
            tips = new(); // <(jetPosition, nextShape), (list,y,numberOfShape)>

        var shapesToUse = 1000000000000;

        for (long i = 0; i < shapesToUse; i++)
        {
            fallingShape = Shape.GetShape((ShapeType)nextShape,
                new LongPoint(2, settledStuff.Any() ? settledStuff.Max(ss => ss.Y) + 4 : 3));
            while (true)
            {
                fallingShape.Move(jet[jetPosition], settledStuff);

                jetPosition = (jetPosition + 1) % jet.Length;

                if (!fallingShape.Fall(settledStuff))
                {
                    break;
                }
            }

            // "towerize" settled shape
            settledStuff.AddRange(fallingShape.PositionedPieces);

            // try to remove stuff bellow last complete row
            for (long y = settledStuff.Max(s => s.Y); y > lastFullRowY; y--)
            {
                var isComplete = true;
                for (int x = 0; x < 7; x++)
                {
                    if (!settledStuff.Contains(new LongPoint(x, y)))
                    {
                        isComplete = false;
                        break;
                    }
                }

                if (isComplete)
                {
                    settledStuff = settledStuff.Where(s => s.Y >= y).ToList();
                    break;
                }
            }

            // add tip
            if (i > 0 && !skippedStuff)
            {
                var maxY = settledStuff.Max(ss => ss.Y);
                var newTip = settledStuff.Where(s => s.Y > maxY - 10).ToList();
                if (tips.ContainsKey((jetPosition, nextShape)))
                {
                    if (CompareTip(tips[(jetPosition, nextShape)].Item1, newTip))
                    {
                        var q = tips[(jetPosition, nextShape)];
                        Console.WriteLine(
                            $"Found cycle y1={q.Item2}, s1={q.Item3}, y2={maxY}, s2={i}, {jetPosition}, {nextShape}");

                        Console.WriteLine($"tower grew by {maxY - q.Item2} using {i - q.Item3} shapes");

                        var remaining = shapesToUse - i;

                        var cycleRepeated = remaining / (i - q.Item3);

                        Console.WriteLine($"cycle already cycled once and can cycle {cycleRepeated} times");

                        i = shapesToUse - (remaining % (i - q.Item3));
                        settledStuff =
                            settledStuff.Select(s => new LongPoint(s.X, s.Y + cycleRepeated * (maxY - q.Item2)))
                                .ToList();

                        Console.WriteLine($"i is now {i}, height is {settledStuff.Max(s => s.Y)}");

                        skippedStuff = true;
                        lastFullRowY += cycleRepeated * (maxY - q.Item2);
                    }
                }
                else
                {
                    tips.Add((jetPosition, nextShape), (newTip, maxY, i));
                }
            }

            nextShape = (nextShape + 1) % Shape.NumberOfShapes;
        }

        Console.WriteLine(settledStuff.Max(ss => ss.Y) + 1);
    }
}