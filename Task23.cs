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

    public void Solve2(string[] lines)
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
        var someoneWantsToMove = true;
        var rounds = 0;
        while (someoneWantsToMove)
        {
            rounds++;
            someoneWantsToMove = false;
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
                        someoneWantsToMove = true;
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

            firstCheck++;
            Console.WriteLine(rounds);
            PrintElves(elves);
        }

        Console.WriteLine(rounds);
    }
}