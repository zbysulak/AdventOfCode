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
        var grid = lines.Select(l => l.ToArray()).ToArray();
        var start = Utils.FindInGrid(grid, 'S');
        var end = Utils.FindInGrid(grid, 'E');

        var parents = new Dictionary<(int, int, int), List<(int, int, int)>>();
        var distances = new Dictionary<(int, int, int), int>();

        var dir = 1;

        var queue = new List<((int x, int y, int dir) pos, int price)>();
        queue.Add(((start.x, start.y, dir), 0));
        while (queue.Any())
        {
            var c = queue.First();
            queue.Remove(c);

            var dirArr = Utils.Directions4[c.pos.dir];
            var dl = (c.pos.dir + 3) % 4;
            var dr = (c.pos.dir + 1) % 4;
            var next = new ((int x, int y, int dir) pos, int price)[]
            {
                ((c.pos.x + dirArr[0], c.pos.y + dirArr[1], c.pos.dir), 1), // move
                ((c.pos.x + Utils.Directions4[dl][0], c.pos.y + Utils.Directions4[dl][1], dl), 1001), // turn left
                ((c.pos.x + Utils.Directions4[dr][0], c.pos.y + Utils.Directions4[dr][1], dr), 1001) // turn right
            };
            foreach (var n in next)
            {
                if (grid[n.pos.y][n.pos.x] == '#') continue;
                if (!distances.ContainsKey(n.pos))
                {
                    distances[n.pos] = c.price + n.price;
                    if (parents.ContainsKey(n.pos))
                        parents[n.pos].Add(c.pos);
                    else
                        parents.Add(n.pos, new List<(int, int, int)> { c.pos });
                    queue.Add((n.pos, c.price + n.price));
                }
                else if (distances[n.pos] == c.price + n.price)
                {
                    if (parents.ContainsKey(n.pos))
                        parents[n.pos].Add(c.pos);
                    else
                        parents.Add(n.pos, new List<(int, int, int)> { c.pos });
                }
            }
        }

        var goalParents = parents.Where(p => p.Key.Item1 == end.x && p.Key.Item2 == end.y);
        var goalPrices = distances.Where(p => p.Key.Item1 == end.x && p.Key.Item2 == end.y);
        var minGoal = goalPrices.OrderBy(p => p.Value).First().Key;
        
        

        var uniqueTiles = new HashSet<(int, int)>();
        CountVisitedTiles(parents, minGoal, uniqueTiles);
        Console.WriteLine(uniqueTiles.Count);

        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                if(uniqueTiles.Contains((x,y)))
                    Console.Write("O");
                else 
                    Console.Write(grid[y][x]);
            }

            Console.WriteLine();
        }
        
        // 451 is too low
    }

    private void CountVisitedTiles(Dictionary<(int, int, int), List<(int, int, int)>> parents, (int, int, int) end,
        HashSet<(int, int)> tiles)
    {
        tiles.Add((end.Item1, end.Item2));
        if (!parents.TryGetValue(end, out var parent))
            return;

        foreach (var p in parent)
        {
            CountVisitedTiles(parents, p, tiles);
        }
    }
}
