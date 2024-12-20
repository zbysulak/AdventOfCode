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

        const int dir = 1;
        var parents = new Dictionary<(int, int, int), List<(int, int, int)>>();
        var distances = new Dictionary<(int, int, int), int> // don't forget to initialize it for start, it can cause cycles when backtracking
        {
            { (start.x, start.y, dir), 0 }
        };


        var queue = new PriorityQueue<(int x, int y, int dir), int>();
        queue.Enqueue((start.x, start.y, dir), 0);
        while (queue.TryDequeue(out var c, out var p))
        {
            var dirs = Utils.Directions4;
            var dl = (c.dir + 3) % 4;
            var dr = (c.dir + 1) % 4;

            var possibleMoves = new ((int x, int y, int dir) pos, int price)[]
            {
                ((c.x + dirs[c.dir][0], c.y + dirs[c.dir][1], c.dir), 1),
                ((c.x, c.y, dl), 1000),
                ((c.x, c.y, dr), 1000),
            };

            foreach (var possibleMove in possibleMoves)
            {
                if (grid[possibleMove.pos.y][possibleMove.pos.x] == '#') continue;
                if (!distances.TryGetValue(possibleMove.pos, out var distance)) // visiting this position (inc. direction) for the first time
                {
                    distances.Add(possibleMove.pos, p + possibleMove.price);
                    parents.Add(possibleMove.pos, new List<(int, int, int)> { c });
                    queue.Enqueue(possibleMove.pos, p + possibleMove.price);
                }
                else if (distance == p + possibleMove.price) // visiting n-th time -> it is already in queue
                {
                    parents[possibleMove.pos].Add(c);
                }
            }
        }

        var endPrices = distances.Where(p => p.Key.Item1 == end.x && p.Key.Item2 == end.y);

        // find which end direction is cheapest 
        var cheapestEnd = endPrices.OrderBy(e => e.Value).First().Key;

        var uniqueTiles = new HashSet<(int, int)>();
        CountVisitedTiles(parents, cheapestEnd, uniqueTiles);
        Console.WriteLine(uniqueTiles.Count);

        /* for (int y = 0; y < grid.Length; y++)
         {
             for (int x = 0; x < grid[y].Length; x++)
             {
                 if (uniqueTiles.Contains((x, y)))
                     Console.Write("O");
                 else
                     Console.Write(grid[y][x]);
             }

             Console.WriteLine();
         } */
    }

    // recursively traverse from end to start, add all visited tiles to set
    private static void CountVisitedTiles(Dictionary<(int, int, int), List<(int, int, int)>> parents, (int, int, int) end,
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
