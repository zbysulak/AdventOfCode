// Day 12

namespace AdventOfCode.task._2024;

public class Task12 : ITask
{
    private HashSet<(int, int)> visited = new();

    // done in 40 minutes
    // because I forgot to clear visited set :)
    public void Solve(string[] lines)
    {
        visited.Clear();
        var price = 0;
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (!visited.Contains((x, y)))
                {
                    var (a, p) = Explore(lines, x, y);
                    price += a * p;
                }
            }
        }

        Console.WriteLine(price);
    }

    private (int area, int perimeter) Explore(string[] lines, int x, int y)
    {
        var plant = lines[y][x];
        var area = 0;
        var perimeter = 0;

        var open = new Queue<(int, int)>();
        open.Enqueue((x, y));
        while (open.Count > 0)
        {
            var (cx, cy) = open.Dequeue();
            if (!visited.Add((cx, cy)))
            {
                continue;
            }

            area++;
            foreach (var d in Utils.Directions4)
            {
                var nx = cx + d[0];
                var ny = cy + d[1];

                if (!Utils.CheckBounds(lines, nx, ny) || lines[ny][nx] != plant)
                    perimeter++;
                else
                {
                    open.Enqueue((nx, ny));
                }
            }
        }

        return (area, perimeter);
    }

    //done in  minutes
    public void Solve2(string[] lines)
    {
        visited.Clear();
        var price = 0;
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (!visited.Contains((x, y)))
                {
                    var (a, _) = Explore(lines, x, y);
                    var s = CountSides(lines, x, y);
                    price += a * s;
                }
            }
        }

        Console.WriteLine(price);
    }

    private int CountSides(string[] lines, int x, int y)
    {
        var sides = 1;
        var plant = lines[y][x];

        var dir = 1; // 0 - up, 1 - right, 2 - down, 3 - left
        var (cx, cy) = (x, y);
        do
        {
            var nx = cx + Utils.Directions4[dir][0];
            var ny = cy + Utils.Directions4[dir][1];

            if (Utils.CheckBounds(lines, nx, ny) && lines[ny][nx] == plant)
            {
                // next in line is same, check up
                var ccwx = nx + Utils.Directions4[(dir + 5) % 4][0];
                var ccwy = ny + Utils.Directions4[(dir + 5) % 4][1];
                if (Utils.CheckBounds(lines, ccwx, ccwy) && lines[ccwy][ccwx] == plant)
                {
                    dir = (dir + 3) % 4;
                    sides++;
                }

                cx = nx;
                cy = ny;
            }
            else
            {
                dir = (dir + 1) % 4;
                sides++;
            }
        } while (!(cx == x && cy == y && dir == 0));

        //todo add inside sides..
        
        return sides;
    }
}
