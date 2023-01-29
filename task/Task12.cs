using System.Collections;
using System.Drawing;

namespace AdventOfCode;

public class Task12 : ITask
{
    public void Solve(string[] lines)
    {
        var map = lines.Select(s => s.ToCharArray()).ToArray();

        Point start = Point.Empty;
        Point end = Point.Empty;
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == 'S')
                {
                    start = new Point(x, y);
                }

                if (lines[y][x] == 'E')
                {
                    end = new Point(x, y);
                }
            }
        }

        map[start.Y][start.X] = 'a';
        map[end.Y][end.X] = 'z';

        var h = (Point p) => Math.Sqrt(Math.Pow(p.X - end.X, 2) + Math.Pow(p.Y - end.Y, 2));

        var openSet = new List<Point> { start };

        var cameFrom = new Dictionary<Point, Point>();

        var gScore = new Dictionary<Point, double> { { start, 0 } }; // cheapest path from start to current
        var fScore = new Dictionary<Point, double> { { start, h(start) } }; // cheapest path from start to end through current

        while (openSet.Any())
        {
            var min = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (fScore[openSet[i]] < fScore[min])
                {
                    min = openSet[i];
                }
            }

            openSet.Remove(min);

            if (min == end)
            {
                Console.WriteLine("tadaaa");
                break;
            }

            var neighbors = new List<Point>
            {
                min with { X = min.X + 1 },
                new(min.X - 1, min.Y),
                new(min.X, min.Y + 1),
                new(min.X, min.Y - 1),
            }.Where(p =>
                p.X >= 0 && p.Y >= 0 && p.X < map[0].Length && p.Y < map.Length &&
                map[p.Y][p.X] - 1 <= map[min.Y][min.X]);

            foreach (var point in neighbors)
            {
                var tentativeG = gScore[min] + 1;
                if (!gScore.ContainsKey(point) || tentativeG < gScore[point])
                {
                    cameFrom[point] = min;
                    gScore[point] = tentativeG;
                    fScore[point] = tentativeG + h(point);
                    if (!openSet.Contains(point))
                        openSet.Add(point);
                }
            }
        }

        Console.WriteLine(gScore[end]);
    }

    public void Solve2(string[] lines)
    {
        var map = lines.Select(s => s.ToCharArray()).ToArray();

        var possibleStarts = new List<Point>();
        Point end = Point.Empty;
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == 'S' || lines[y][x] == 'a')
                {
                    possibleStarts.Add(new Point(x, y));
                }

                if (lines[y][x] == 'E')
                {
                    end = new Point(x, y);
                }
            }
        }

        var minLength = int.MaxValue;
        foreach (var start in possibleStarts)
        {

            map[start.Y][start.X] = 'a';
            map[end.Y][end.X] = 'z';

            var h = (Point p) => Math.Sqrt(Math.Pow(p.X - end.X, 2) + Math.Pow(p.Y - end.Y, 2));

            var openSet = new List<Point> { start };

            var cameFrom = new Dictionary<Point, Point>();

            var gScore = new Dictionary<Point, double> { { start, 0 } }; // cheapest path from start to current
            var fScore = new Dictionary<Point, double>
                { { start, h(start) } }; // cheapest path from start to end through current

            while (openSet.Any())
            {
                var min = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (fScore[openSet[i]] < fScore[min])
                    {
                        min = openSet[i];
                    }
                }

                openSet.Remove(min);

                if (min == end)
                {
                    Console.WriteLine("tadaaa" + gScore[min]);
                    break;
                }

                var neighbors = new List<Point>
                {
                    min with { X = min.X + 1 },
                    new(min.X - 1, min.Y),
                    new(min.X, min.Y + 1),
                    new(min.X, min.Y - 1),
                }.Where(p =>
                    p.X >= 0 && p.Y >= 0 && p.X < map[0].Length && p.Y < map.Length &&
                    map[p.Y][p.X] - 1 <= map[min.Y][min.X]);

                foreach (var point in neighbors)
                {
                    var tentativeG = gScore[min] + 1;
                    if (!gScore.ContainsKey(point) || tentativeG < gScore[point])
                    {
                        cameFrom[point] = min;
                        gScore[point] = tentativeG;
                        fScore[point] = tentativeG + h(point);
                        if (!openSet.Contains(point))
                            openSet.Add(point);
                    }
                }
            }

            if (gScore.ContainsKey(end) && gScore[end] < minLength)
                minLength = (int)gScore[end];
        }

        Console.WriteLine(minLength);
    }
}