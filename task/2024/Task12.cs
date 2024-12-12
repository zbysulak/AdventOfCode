// Day 12: Garden Groups

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

    //done in ?? hours
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
                    var (a, s) = Explore2(lines, x, y);
                    //Console.WriteLine($"{lines[y][x]} : area {a}, sides {s}");
                    price += a * s;
                }
            }
        }

        Console.WriteLine(price);
    }

    // fence on the right side is not same as fence on the left side of the spot on the right!
    private (int, int, int)? GetFenceInRow(IList<(int x, int y, int dir)> fences, ( int x, int y, int dir) f, int dist)
    {
        try
        {
            if (f.dir == 0)
            {
                return fences.Single(of => f.y == of.y && f.x - of.x == dist && of.dir == 0);
            }

            if (f.dir == 2)
            {
                return fences.Single(of => f.y == of.y && f.x - of.x == dist && of.dir == 2);
            }

            if (f.dir == 1)
            {
                return fences.Single(of => f.x == of.x && f.y - of.y == dist && of.dir == 1);
            }

            if (f.dir == 3)
            {
                return fences.Single(of => f.x == of.x && f.y - of.y == dist && of.dir == 3);
            }
        }
        catch
        {
            return null;
        }

        return null;
    }

    private (int area, int sides) Explore2(string[] lines, int x, int y)
    {
        var plant = lines[y][x];
        var area = 0;
        var fences = new List<(int x, int y, int dir)>();

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
            for (int i = 0; i < Utils.Directions4.Length; i++)
            {
                var d = Utils.Directions4[i];
                var nx = cx + d[0];
                var ny = cy + d[1];

                if (!Utils.CheckBounds(lines, nx, ny) || lines[ny][nx] != plant)
                    fences.Add((cx, cy, i));
                else
                {
                    open.Enqueue((nx, ny));
                }
            }
        }

        var filteredFences = new List<(int x, int y, int dir)>();

        while (fences.Any())
        {
            var f = fences.First();
            fences.RemoveAt(0);
            filteredFences.Add(f);

            // I can't just skip fence that has neighbor because it could disconnect other fences in line (i.e. 3 fences in line, 2nd is skipped -> 1st and 3rd are disconnected)
            var i = 1;
            while (true)
            {
                var next = GetFenceInRow(fences, f, i);

                if (next == null)
                    break;

                fences.Remove(next.Value);
                i++;
            }

            i = 1;
            while (true)
            {
                var next = GetFenceInRow(fences, f, -i);

                if (next == null)
                    break;

                fences.Remove(next.Value);
                i++;
            }
        }

        return (area, filteredFences.Count);
    }
}
