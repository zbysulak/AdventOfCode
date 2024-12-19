// Day 16

namespace AdventOfCode.task._2024;

public class Task16 : ITask
{
    public void Solve(string[] lines)
    {
        var grid = lines.Select(l => l.ToArray()).ToArray();
        var start = Utils.FindInGrid(grid, 'S');
        var end = Utils.FindInGrid(grid, 'E');

        var dir = 1;

        var visited = new HashSet<(int, int, int)>();

        var queue = new PriorityQueue<(int x, int y, int dir), int>();
        queue.Enqueue((start.x, start.y, dir), 0);
        while (queue.TryDequeue(out var c, out var p))
        {
            if (c.x == end.x && c.y == end.y)
            {
                Console.WriteLine(p);
                break;
            }

            visited.Add(c);

            // move
            var dirArr = Utils.Directions4[c.dir];
            (int x, int y) next = (c.x + dirArr[0], c.y + dirArr[1]);
            if (grid[next.y][next.x] != '#' && !visited.Contains((next.x, next.y, c.dir)))
                queue.Enqueue((next.x, next.y, c.dir), p + 1);
            // turns
            if (!visited.Contains((c.x, c.y, (c.dir + 3) % 4)))
                queue.Enqueue((c.x, c.y, (c.dir + 3) % 4), p + 1000);
            if (!visited.Contains((c.x, c.y, (c.dir + 1) % 4)))
                queue.Enqueue((c.x, c.y, (c.dir + 1) % 4), p + 1000);
        }
    }

    public void Solve2(string[] lines)
    {
    }
}
