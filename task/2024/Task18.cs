// Day 18

namespace AdventOfCode.task._2024;

public class Task18 : ITask
{
    private (int x, int y)[] _bytes;

    public void Solve(string[] lines)
    {
        _bytes = lines.Select(l =>
        {
            var c = l.Split(',');
            return (int.Parse(c[0]), int.Parse(c[1]));
        }).ToArray();

        (int x, int y) pos = (0, 0);

        var gridSize = lines.Length < 50 ? 6 : 70;
        var fallen = lines.Length < 50 ? 12 : 1024;
        var open = new List<(int x, int y)> { (pos.x, pos.y) };
        var gScore = new Dictionary<(int, int), int>();
        gScore[pos] = 0;

        var fScore = new Dictionary<(int, int), int>();
        fScore[pos] = getH(pos);

        var cameFrom = new Dictionary<(int x, int y), (int x, int y)>();

        while (open.Any())
        {
            var c = open.OrderBy(f => fScore[f]).First();
            open.Remove(c);

            if (c.x == gridSize && c.y == gridSize)
            {
                Console.WriteLine(GetPathLength(c, cameFrom));
                break;
            }

            foreach (var dir in Utils.Directions4)
            {
                (int x, int y) next = (c.x + dir[0], c.y + dir[1]);
                if (next.x >= 0 && next.x <= gridSize && next.y >= 0 && next.y <= gridSize &&
                    _bytes.Take(fallen).All(f => f != next))
                {
                    var tengScore = gScore[c] + 1;
                    if (!gScore.ContainsKey(next) || tengScore < gScore[next])
                    {
                        cameFrom[next] = c;
                        gScore[next] = tengScore;
                        fScore[next] = tengScore + getH(next);
                        if (!open.Contains(next))
                            open.Add(next);
                    }
                }
            }
        }

        return;

        int getH((int x, int y) pos) => gridSize - pos.x + gridSize - pos.y;
    }

    private int GetPathLength((int, int) pos, Dictionary<(int, int), (int, int)> cameFrom)
        => GetPath(pos, cameFrom).Count;

    private IList<(int, int)> GetPath((int, int) pos, Dictionary<(int, int), (int, int)> cameFrom)
    {
        var path = new List<(int, int)>();
        while (pos != (0, 0))
        {
            path.Add(pos);
            pos = cameFrom[pos];
        }

        return path;
    }


    public void Solve2(string[] lines)
    {
        _bytes = lines.Select(l =>
        {
            var c = l.Split(',');
            return (int.Parse(c[0]), int.Parse(c[1]));
        }).ToArray();

        (int x, int y) start = (0, 0);

        var gridSize = lines.Length < 50 ? 6 : 70;
        var fallen = lines.Length < 50 ? 12 : 1024;

        IList<(int, int)>? path = null;

        for (int i = fallen + 1; i < _bytes.Length; i++)
        {
            // skipping pathfinding if new byte doesn't block previous path
            if (path != null && !path.Contains(_bytes[i - 1]))
                continue;

            var found = false;
            var open = new List<(int x, int y)> { (start.x, start.y) };
            var gScore = new Dictionary<(int, int), int>
            {
                [start] = 0
            };

            var fScore = new Dictionary<(int, int), int>
            {
                [start] = getH(start)
            };

            var cameFrom = new Dictionary<(int x, int y), (int x, int y)>();

            while (open.Any())
            {
                var c = open.OrderBy(f => fScore[f]).First();
                open.Remove(c);

                if (c.x == gridSize && c.y == gridSize)
                {
                    found = true;
                    path = GetPath(c, cameFrom);
                    //Console.WriteLine(i + " found");
                    break;
                }

                foreach (var dir in Utils.Directions4)
                {
                    (int x, int y) next = (c.x + dir[0], c.y + dir[1]);
                    if (next.x >= 0 && next.x <= gridSize && next.y >= 0 && next.y <= gridSize &&
                        _bytes.Take(i).All(f => f != next))
                    {
                        var tengScore = gScore[c] + 1;
                        if (!gScore.ContainsKey(next) || tengScore < gScore[next])
                        {
                            cameFrom[next] = c;
                            gScore[next] = tengScore;
                            fScore[next] = tengScore + getH(next);
                            if (!open.Contains(next))
                                open.Add(next);
                        }
                    }
                }
            }

            if (!found)
            {
                Console.WriteLine(
                    $"path not found when byte #{i} fell at coordinates {_bytes[i - 1].x},{_bytes[i - 1].y}");
                break;
            }
        }

        return;

        int getH((int x, int y) pos) => gridSize - pos.x + gridSize - pos.y;
    }
}
