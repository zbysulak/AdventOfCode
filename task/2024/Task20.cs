// Day 20

using System.Diagnostics;
using System.Drawing;

namespace AdventOfCode.task._2024;

public class Task20 : ITask
{
    public void Solve(string[] lines)
    {
        var grid = lines.Select(s => s.ToArray()).ToArray();
        var start = Utils.FindInGrid(grid, 'S');
        var end = Utils.FindInGrid(grid, 'E');

        var noShortcutPath = FindPath(grid, start, end);
        var noShortcutTime = noShortcutPath.Count - 1;
        var paths = FindPaths(grid, start, end, noShortcutPath);

        foreach (var g in paths.GroupBy(p => p).OrderByDescending(p => p.Key))
        {
            Console.WriteLine($"{g.Count()} shortcuts that save {noShortcutTime - g.Key}ps");
        }

        Console.WriteLine(paths.Count(p => noShortcutTime - p >= 100) + " shortcuts saves at least 100ps");
    }

    private IList<(int, int)> FindPath(char[][] grid, (int x, int y) start, (int x, int y) end)
    {
        var open = new List<((int x, int y) pos, int steps, HashSet<(int, int)> visited)>
            { (start, 0, new HashSet<(int, int)>()) };
        while (open.Count > 0)
        {
            var c = open.First();
            open.Remove(c);
            if (c.pos == end)
            {
                c.visited.Add(c.pos);
                return c.visited.ToList();
            }

            foreach (var d in Utils.Directions4)
            {
                (int x, int y) next = (c.pos.x + d[0], c.pos.y + d[1]);
                if (!c.visited.Contains(next))
                {
                    if (grid[next.y][next.x] != '#')
                    {
                        var newVisited = new HashSet<(int, int)>(c.visited);
                        newVisited.Add(c.pos);
                        open.Add((next, c.steps + 1, newVisited));
                    }
                }
            }
        }

        throw new Exception("path not found");
    }

    private IList<int> FindPaths(char[][] grid, (int x, int y) start, (int x, int y) end,
        IList<(int, int)> originalPath)
    {
        var paths = new List<int>();
        var open = new List<((int x, int y) pos, int steps, HashSet<(int, int)> visited, bool cheated)>
            { (start, 0, new HashSet<(int, int)>(), false) };
        while (open.Count > 0)
        {
            var c = open.First();
            open.Remove(c);
            if (c.pos == end)
            {
                continue;
            }

            foreach (var d in Utils.Directions4)
            {
                (int x, int y) next = (c.pos.x + d[0], c.pos.y + d[1]);
                if (!c.visited.Contains(next))
                {
                    if (grid[next.y][next.x] != '#')
                    {
                        var newVisited = new HashSet<(int, int)>(c.visited);
                        newVisited.Add(c.pos);
                        open.Add((next, c.steps + 1, newVisited, c.cheated));
                    }
                    else if (!c.cheated)
                    {
                        next = (next.x + d[0], next.y + d[1]); // skip one wall and join original path
                        if (originalPath.Contains(next))
                        {
                            var whereJoinOriginal = originalPath.IndexOf(next);
                            var totalTime = c.steps + 1 + originalPath.Skip(whereJoinOriginal).Count();
                            if (totalTime < originalPath.Count)
                                paths.Add(totalTime);
                        }
                    }
                }
            }
        }

        return paths;
    }

    private void PrintPath(char[][] grid, HashSet<(int x, int y)> path)
    {
        foreach (var (x, y) in Utils.IterateOverGrid(grid, true))
        {
            if (grid[y][x] == '#')
                Console.Write("#");
            else if (path.Contains((x, y)))
                Console.Write("O");
            else
                Console.Write(".");
        }
    }


    private bool _sample;

    public void Solve2(string[] lines)
    {
        _sample = lines.Length < 20;
        var grid = lines.Select(s => s.ToArray()).ToArray();
        var start = Utils.FindInGrid(grid, 'S');
        var end = Utils.FindInGrid(grid, 'E');

        var noShortcutPath = FindPath(grid, start, end);
        var noShortcutTime = noShortcutPath.Count - 1;
        var sw = Stopwatch.StartNew();
        var paths = FindPathsWithLongShortcuts(grid, start, end, noShortcutPath);
        Console.WriteLine($"pathfinding took {sw.ElapsedMilliseconds}ms");

        if (_sample)
            foreach (var g in paths
                         .Select(p => noShortcutTime - p)
                         .Where(p => p >= 50)
                         .GroupBy(p => p)
                         .OrderBy(p => p.Key))
            {
                Console.WriteLine($"{g.Count()} shortcuts that save {g.Key}ps");
            }

        Console.WriteLine(paths.Count(p => noShortcutTime - p >= 100) + " shortcuts saves at least 100ps");
    }

    // runs for few minutes, but gives correct answer! :D
    private IList<int> FindPathsWithLongShortcuts(char[][] grid, (int x, int y) start, (int x, int y) end,
        IList<(int, int)> originalPath)
    {
        const int cheatLength = 20;
        var paths = new List<int>();
        var visited = new HashSet<(int, int)>();
        var open = new List<((int x, int y) pos, int steps)>
            { (start, 0) };
        while (open.Count > 0)
        {
            var c = open.First();
            open.Remove(c);
            visited.Add(c.pos);
            if (c.pos == end)
            {
                continue;
            }

            foreach (var d in Utils.Directions4)
            {
                (int x, int y) next = (c.pos.x + d[0], c.pos.y + d[1]);
                if (!visited.Contains(next))
                {
                    if (grid[next.y][next.x] != '#')
                    {
                        open.Add((next, c.steps + 1));
                    }
                }
            }

            for (int x = -cheatLength; x <= cheatLength; x++)
            {
                for (int y = -cheatLength; y <= cheatLength; y++)
                {
                    var thisCheatLenght = Math.Abs(x) + Math.Abs(y);
                    if (thisCheatLenght > cheatLength) continue;
                    if (originalPath.Contains((c.pos.x + x, c.pos.y + y)))
                    {
                        var whereJoinOriginal = originalPath.IndexOf((c.pos.x + x, c.pos.y + y));
                        var totalTime = c.steps + thisCheatLenght - 1 +
                                        originalPath.Skip(whereJoinOriginal).Count();
                        if (totalTime < originalPath.Count && (_sample || totalTime >= 100))
                            paths.Add(totalTime);
                    }
                }
            }
        }

        return paths;
    }
}
