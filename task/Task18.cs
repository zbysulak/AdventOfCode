using System.Drawing;
using System.Runtime.InteropServices;

namespace AdventOfCode;

public class Task18 : ITask
{
    private record Cube
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public bool Top { get; set; }
        public bool Bottom { get; set; }
        public bool Left { get; set; }
        public bool Right { get; set; }
        public bool Front { get; set; }
        public bool Back { get; set; }

        public Cube(string input)
        {
            var i = input.Split(",").Select(int.Parse).ToList();
            X = i[0];
            Y = i[1];
            Z = i[2];
        }

        public Cube(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void Check(Cube cube)
        {
            if (X == cube.X && Y == cube.Y && Z + 1 == cube.Z) // this is below cube
            {
                Top = true;
                cube.Bottom = true;
            }
            else if (X == cube.X && Y + 1 == cube.Y && Z == cube.Z) // this is behind cube
            {
                Front = true;
                cube.Back = true;
            }
            else if (X + 1 == cube.X && Y == cube.Y && Z == cube.Z) // this is on the left of cube
            {
                Right = true;
                cube.Left = true;
            }
            else if (X == cube.X && Y == cube.Y && Z - 1 == cube.Z) // this is on top of cube
            {
                cube.Top = true;
                Bottom = true;
            }
            else if (X == cube.X && Y - 1 == cube.Y && Z == cube.Z) // this is behind cube
            {
                cube.Front = true;
                Back = true;
            }
            else if (X - 1 == cube.X && Y == cube.Y && Z == cube.Z) // this is on the right of cube
            {
                cube.Right = true;
                Left = true;
            }
        }

        // for 1st part
        public int ExposedSides => (Top ? 0 : 1) + (Bottom ? 0 : 1) +
                                   (Left ? 0 : 1) + (Right ? 0 : 1) +
                                   (Front ? 0 : 1) + (Back ? 0 : 1);

        // for 2nd part
        public int TouchedSides => 6 - ExposedSides;

        public bool PositionEquals(Cube cube)
        {
            return X == cube.X && Y == cube.Y && Z == cube.Z;
        }
    }

    public void Solve(string[] lines)
    {
        var cubes = lines.Select(l => new Cube(l)).ToList();
        foreach (var cube in cubes)
        {
            foreach (var cube1 in cubes)
            {
                cube.Check(cube1);
            }
        }

        Console.WriteLine(cubes.Sum(s => s.ExposedSides));
    }

    public void Solve2(string[] lines)
    {
        var cubes = lines.Select(l => new Cube(l)).ToList();

        var min = new Cube(int.MaxValue, int.MaxValue, int.MaxValue);
        var max = new Cube(int.MinValue, int.MinValue, int.MinValue);
        foreach (var cube in cubes) // find block where all cubes are. +1(-1) to make everything accessible from outside
        {
            min.X = Math.Min(min.X, cube.X-1);
            min.Y = Math.Min(min.Y, cube.Y-1);
            min.Z = Math.Min(min.Z, cube.Z-1);
            max.X = Math.Max(max.X, cube.X+1);
            max.Y = Math.Max(max.Y, cube.Y+1);
            max.Z = Math.Max(max.Z, cube.Z+1);
        }

        // run BFS from any point outside of droplet. if current cube touches droplet, mark it
        var open = new Queue<Cube>();
        var closed = new List<Cube>();
        open.Enqueue(max);

        var getNeighbors = (Cube current) =>
        {
            return new List<Cube>
                {
                    current with { X = current.X + 1 },
                    current with { X = current.X - 1 },
                    current with { Y = current.Y + 1 },
                    current with { Y = current.Y - 1 },
                    current with { Z = current.Z + 1 },
                    current with { Z = current.Z - 1 },
                }.Where(c =>
                    c.X >= min.X && c.Y >= min.Y && c.Z >= min.Z && c.X <= max.X && c.Y <= max.Y && c.Z <= max.Z)
                .Where(c => !cubes.Any(cube => cube.PositionEquals(c)));
        };

        while (open.Any())
        {
            var cur = open.Dequeue();
            closed.Add(cur);

            #region marking neighbors...

            var n = cubes.SingleOrDefault(c => c.X - 1 == cur.X && c.Y == cur.Y && c.Z == cur.Z);
            if (n is not null)
            {
                n.Left = true;
            }

            n = cubes.SingleOrDefault(c => c.X == cur.X && c.Y - 1 == cur.Y && c.Z == cur.Z);
            if (n is not null)
            {
                n.Back = true;
            }

            n = cubes.SingleOrDefault(c => c.X == cur.X && c.Y == cur.Y && c.Z - 1 == cur.Z);
            if (n is not null)
            {
                n.Bottom = true;
            }

            n = cubes.SingleOrDefault(c => c.X + 1 == cur.X && c.Y == cur.Y && c.Z == cur.Z);
            if (n is not null)
            {
                n.Right = true;
            }

            n = cubes.SingleOrDefault(c => c.X == cur.X && c.Y + 1 == cur.Y && c.Z == cur.Z);
            if (n is not null)
            {
                n.Front = true;
            }

            n = cubes.SingleOrDefault(c => c.X == cur.X && c.Y == cur.Y && c.Z + 1 == cur.Z);
            if (n is not null)
            {
                n.Top = true;
            }

            #endregion

            foreach (var neighbor in getNeighbors(cur).Where(nei =>
                         !open.Any(c => c.PositionEquals(nei)) && !closed.Any(c => c.PositionEquals(nei))))
            {
                open.Enqueue(neighbor);
            }
        }

        Console.WriteLine(cubes.Sum(c => c.TouchedSides));
    }
}