using System.Drawing;

namespace AdventOfCode;

public class Task23
{
    public class Elf
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int NextX { get; set; }
        public int NextY { get; set; }
    }

    public void Solve(string[] lines)
    {
        var elves = new List<Elf>();
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                if (lines[i][j] == '#')
                    elves.Add(new Elf { X = j, Y = i });
            }
        }

        var checks = new List<Func<Elf, bool>>
        {
            elf =>
            {
                // check north
                if (!elves.Any(e => e.Y == elf.Y - 1 && Math.Abs(e.X - elf.X) <= 1))
                {
                    elf.NextY = elf.Y - 1;
                    elf.NextX = elf.X;
                    return true;
                }

                return false;
            },
            elf =>
            {
                // check south
                if (!elves.Any(e => e.Y == elf.Y + 1 && Math.Abs(e.X - elf.X) <= 1))
                {
                    elf.NextY = elf.Y + 1;
                    elf.NextX = elf.X;
                    return true;
                }

                return false;
            },
            elf =>
            {
                // check west
                if (!elves.Any(e => e.X == elf.X - 1 && Math.Abs(e.Y - elf.Y) <= 1))
                {
                    elf.NextY = elf.Y;
                    elf.NextX = elf.X - 1;
                    return true;
                }

                return false;
            },
            elf =>
            {
                // check east
                if (!elves.Any(e => e.X == elf.X + 1 && Math.Abs(e.Y - elf.Y) <= 1))
                {
                    elf.NextY = elf.Y;
                    elf.NextX = elf.X + 1;
                    return true;
                }

                return false;
            }
        };

        PrintElves(elves);

        var firstCheck = 0;
        for (int r = 0; r < 10; r++)
        {
            // proposing next move
            foreach (var elf in elves)
            {
                // no other elf around him -> don't move 
                if (!elves.Any(e =>
                        Math.Abs(e.X - elf.X) <= 1 && Math.Abs(e.Y - elf.Y) <= 1 && !(elf.X == e.X && elf.Y == e.Y)))
                {
                    elf.NextX = elf.X;
                    elf.NextY = elf.Y;
                    continue;
                }

                // propose next move
                for (int ch = firstCheck; ch < firstCheck + 4; ch++)
                {
                    var check = ch % 4;
                    if (checks[check](elf))
                    {
                        break;
                    }
                }
            }

            foreach (var elf in elves)
            {
                var otherElves = elves.Where(e => !(e.X == elf.X && e.Y == elf.Y));
                if (!otherElves.Any(e => e.NextX == elf.NextX && e.NextY == elf.NextY))
                {
                    elf.X = elf.NextX;
                    elf.Y = elf.NextY;
                }
            }

            //PrintElves(elves);

            firstCheck++;
        }

        var width = (elves.Max(e => e.X) - elves.Min(e => e.X)) + 1;
        var height = (elves.Max(e => e.Y) - elves.Min(e => e.Y)) + 1;

        Console.WriteLine(width * height - elves.Count);
    }

    private void PrintElves(List<Elf> elves)
    {
        for (int y = elves.Min(e => e.Y); y <= elves.Max(e => e.Y); y++)
        {
            for (int x = elves.Min(e => e.X); x <= elves.Max(e => e.X); x++)
            {
                Console.Write(elves.Any(e => e.X == x && e.Y == y) ? "#" : ".");
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }

    private void PrintElves2(Elf2?[,] elves)
    {
        for (int y = 0; y < elves.GetLength(0); y++)
        {
            for (int x = 0; x < elves.GetLength(1); x++)
            {
                Console.Write(elves[y, x] is not null ? "#" : ".");
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }

    private class Elf2
    {
        public Point? ProposedPosition { get; set; }
    }

    public void Solve2(string[] lines)
    {
        var elves = new Elf2?[lines[0].Length * 3, lines.Length * 3];

        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                if (lines[i][j] == '#')
                    elves[i + lines[0].Length, lines.Length + j] = new Elf2();
            }
        }

        var checks = new List<Func<int, int, bool>>
        {
            (x, y) =>
            {
                // check north
                if (elves[y - 1, x - 1] is null && elves[y - 1, x] is null && elves[y - 1, x + 1] is null)
                {
                    elves[y, x].ProposedPosition = new Point(x, y - 1);
                    return true;
                }

                return false;
            },
            (x, y) =>
            {
                // check south
                if (elves[y + 1, x - 1] is null && elves[y + 1, x] is null && elves[y + 1, x + 1] is null)
                {
                    elves[y, x].ProposedPosition = new Point(x, y + 1);
                    return true;
                }

                return false;
            },
            (x, y) =>
            {
                // check west
                if (elves[y - 1, x - 1] is null && elves[y, x - 1] is null && elves[y + 1, x - 1] is null)
                {
                    elves[y, x].ProposedPosition = new Point(x - 1, y);
                    return true;
                }

                return false;
            },
            (x, y) =>
            {
                // check east
                if (elves[y - 1, x + 1] is null && elves[y, x + 1] is null && elves[y + 1, x + 1] is null)
                {
                    elves[y, x].ProposedPosition = new Point(x + 1, y);
                    return true;
                }

                return false;
            }
        };

        PrintElves2(elves);

        bool CheckPropositions(Point p)
        {
            return new List<Elf2?>
                    { elves[p.Y - 1, p.X], elves[p.Y + 1, p.X], elves[p.Y, p.X - 1], elves[p.Y, p.X + 1] }
                .Count(e => e is not null && e.ProposedPosition == p) == 1;
        }

        var firstCheck = 0;
        var someoneWantsToMove = true;
        var rounds = 0;
        while (someoneWantsToMove)
        {
            rounds++;
            someoneWantsToMove = false;
            // proposing next move
            for (int y = 1; y < elves.GetLength(0) - 1; y++) // to prevent indexOutOfBounds..
            {
                for (int x = 1; x < elves.GetLength(1) - 1; x++)
                {
                    // no elf..
                    if (elves[y, x] is null) continue;

                    elves[y, x].ProposedPosition = null;
                    // no other elf around him -> don't move 
                    if (elves[y - 1, x - 1] is null && elves[y - 1, x] is null && elves[y - 1, x + 1] is null &&
                        elves[y, x - 1] is null && elves[y, x + 1] is null &&
                        elves[y + 1, x - 1] is null && elves[y + 1, x] is null && elves[y + 1, x + 1] is null)
                    {
                        elves[y, x].ProposedPosition = new(x, y);
                        continue;
                    }

                    // propose next move
                    for (int ch = firstCheck; ch < firstCheck + 4; ch++)
                    {
                        var check = ch % 4;
                        if (checks[check](x, y))
                        {
                            someoneWantsToMove = true;
                            break;
                        }
                    }
                }
            }

            for (int y = 0; y < elves.GetLength(0); y++)
            {
                for (int x = 0; x < elves.GetLength(1); x++)
                {
                    var elf = elves[y, x];
                    if (elf?.ProposedPosition is null) continue;
                    if (CheckPropositions(elf.ProposedPosition.Value))
                    {
                        elves[elf.ProposedPosition.Value.Y, elf.ProposedPosition.Value.X] = elf;
                        elf.ProposedPosition = null;
                        elves[y, x] = null;
                    }
                }
            }

            firstCheck++;
            //Console.WriteLine(rounds);
            //PrintElves2(elves);
        }
        PrintElves2(elves);
        Console.WriteLine(rounds);
    }
}