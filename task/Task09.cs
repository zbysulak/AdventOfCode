// Day 9: Rope Bridge

namespace AdventOfCode;

public class Task08 : ITask
{
    private class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position()
        {
        }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    private class Rope
    {
        public Position Head { get; set; }
        public Position Tail { get; set; }

        public int TailsDistance => TailsHistory.Count;

        private HashSet<(int, int)> TailsHistory;

        public Rope()
        {
            Head = new Position { X = 0, Y = 0 };
            Tail = new Position { X = 0, Y = 0 };
            TailsHistory = new HashSet<(int, int)>
            {
                (Tail.X, Tail.Y)
            };
        }

        public void MakeMove(string move)
        {
            var i = move.Split(' ');
            for (int j = 0; j < int.Parse(i[1]); j++)
            {
                SingleMove(i[0]);
                //Console.WriteLine($"Moved to {i[0]}");
                //Print(5);
            }
        }

        public void Print(int size)
        {
            for (int y = size; y >= 0; y--)
            {
                for (int x = 0; x < size; x++)
                {
                    if (Head.X == x && Head.Y == y)
                        Console.Write("H");
                    else if (Tail.X == x && Tail.Y == y)
                        Console.Write("T");
                    else
                        Console.Write(".");
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public void PrintHistory(int size)
        {
            for (int y = size; y >= 0; y--)
            {
                for (int x = 0; x < size; x++)
                {
                    if (TailsHistory.Contains((x, y)))
                        Console.Write("#");
                    else
                        Console.Write(".");
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        private void SingleMove(string direction)
        {
            switch (direction)
            {
                case "L":
                    Head.X--;
                    if (Tail.X - Head.X > 1)
                    {
                        Tail.X--;
                        Tail.Y = Head.Y;
                    }

                    break;
                case "R":
                    Head.X++;
                    if (Head.X - Tail.X > 1)
                    {
                        Tail.X++;
                        Tail.Y = Head.Y;
                    }

                    break;
                case "U":
                    Head.Y++;
                    if (Head.Y - Tail.Y > 1)
                    {
                        Tail.Y++;
                        Tail.X = Head.X;
                    }

                    break;
                case "D":
                    Head.Y--;
                    if (Tail.Y - Head.Y > 1)
                    {
                        Tail.Y--;
                        Tail.X = Head.X;
                    }

                    break;
                default:
                    throw new Exception("Invalid move");
            }

            TailsHistory.Add((Tail.X, Tail.Y));
        }
    }

    public void Solve(string[] lines)
    {
        var rope = new Rope();
        foreach (var line in lines)
        {
            rope.MakeMove(line);
            //rope.Print(5);
        }

        //rope.PrintHistory(5);
        Console.WriteLine(rope.TailsDistance);
    }

    private class LongerRope
    {
        public Position[] Knots { get; set; }

        public int TailsDistance => TailsHistory.Count;

        private HashSet<(int, int)> TailsHistory;

        public LongerRope(int length)
        {
            Knots = new Position[length];
            for (int i = 0; i < length; i++)
            {
                Knots[i] = new Position(0, 0);
            }

            TailsHistory = new HashSet<(int, int)>
            {
                (Knots.Last().X, Knots.Last().Y)
            };
        }

        public void MakeMove(string move)
        {
            var i = move.Split(' ');
            Console.WriteLine($"Moving {i[1]} to {i[0]}\n");
            for (int j = 0; j < int.Parse(i[1]); j++)
            {
                SingleMove(i[0]);
                //Print();
            }
        }

        public void Print()
        {
            for (int y = 4; y >= 0; y--)
            {
                for (int x = 0; x < 6; x++)
                {
                    var noKnot = true;
                    for (int i = 0; i < Knots.Length; i++)
                    {
                        if (Knots[i].X == x && Knots[i].Y == y)
                        {
                            Console.Write(i == 0 ? "H" : i);
                            noKnot = false;
                            break;
                        }
                    }

                    if (noKnot) Console.Write(x == 0 && y == 0 ? "s" : ".");
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public void PrintHistory()
        {
            for (int y = 4; y >= 0; y--)
            {
                for (int x = 0; x < 6; x++)
                {
                    if (x == 0 && y == 0)
                        Console.Write("s");
                    else if (TailsHistory.Contains((x, y)))
                        Console.Write("#");
                    else
                        Console.Write(".");
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        private void SingleMove(string direction)
        {
            switch (direction)
            {
                case "L":
                    Knots.First().X--;
                    break;
                case "R":
                    Knots.First().X++;
                    break;
                case "U":
                    Knots.First().Y++;
                    break;
                case "D":
                    Knots.First().Y--;
                    break;
                default:
                    throw new Exception("Invalid move");
            }

            for (int i = 0; i < Knots.Length - 1; i++)
            {
                var k1 = Knots[i];
                var k2 = Knots[i + 1];
                if (k1.X - k2.X > 1) // moving right
                {
                    k2.X++;
                    k2.Y += k1.Y == k2.Y ? 0 : k1.Y > k2.Y ? 1 : -1; // because knot could also be (+2,+2) blocks away..
                }
                else if (k1.X - k2.X < -1) // moving left
                {
                    k2.X--;
                    k2.Y += k1.Y == k2.Y ? 0 : k1.Y > k2.Y ? 1 : -1;
                }
                else if (k1.Y - k2.Y > 1) // moving up
                {
                    k2.Y++;
                    k2.X += k1.X == k2.X ? 0 : k1.X > k2.X ? 1 : -1;
                }
                else if (k1.Y - k2.Y < -1) // moving down
                {
                    k2.Y--;
                    k2.X += k1.X == k2.X ? 0 : k1.X > k2.X ? 1 : -1;
                }
            }

            TailsHistory.Add((Knots.Last().X, Knots.Last().Y));
        }
    }

    public void Solve2(string[] lines)
    {
        var rope = new LongerRope(10);
        foreach (var line in lines)
        {
            rope.MakeMove(line);
            rope.Print();
        }

        rope.PrintHistory();
        Console.WriteLine(rope.TailsDistance);
    }
}