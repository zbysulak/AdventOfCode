using System.Drawing;

namespace AdventOfCode.task._2022;

public class Task24 : ITask
{
    private static int mod(int number, int mod) => (number % mod + mod) % mod;

    public class Blizzard
    {
        private static Size mapSize = new(map[0].Length - 2, map.Length - 2);
        public Point StartPosition { get; set; }
        public Direction Direction { get; set; }

        public Point PositionAt(int minute)
        {
            switch (Direction)
            {
                case Direction.Up:
                    return StartPosition with { Y = mod(StartPosition.Y - minute - 1, mapSize.Height) + 1 };
                case Direction.Right:
                    return StartPosition with { X = (StartPosition.X + minute - 1) % mapSize.Width + 1 };
                case Direction.Down:
                    return StartPosition with { Y = (StartPosition.Y + minute - 1) % mapSize.Height + 1 };
                case Direction.Left:
                    return StartPosition with { X = mod(StartPosition.X - minute - 1, mapSize.Width) + 1 };
                default: throw new Exception("can't happen");
            }
        }
    }

    public enum Direction
    {
        Up = '^',
        Right = '>',
        Down = 'v',
        Left = '<'
    }

    public record PositionState
    {
        public Point Position { get; set; }
        public int minute { get; set; }
    }

    private static string[] map;

    public void Solve(string[] lines)
    {
        map = lines;
        var blizzards = new List<Blizzard>();
        var start = Point.Empty;
        var end = Point.Empty;
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[0].Length; x++)
            {
                if (y == 0 && map[y][x] == '.')
                {
                    start = new Point(x, y);
                }
                else if (y == map.Length - 1 && map[y][x] == '.')
                {
                    end = new Point(x, y);
                }
                else if (new[] { '<', '>', 'v', '^' }.Contains(map[y][x]))
                {
                    blizzards.Add(new Blizzard
                    {
                        Direction = (Direction)map[y][x],
                        StartPosition = new Point(x, y)
                    });
                }
            }
        }

        var h = (Point p) => Math.Sqrt(Math.Pow(p.X - end.X, 2) + Math.Pow(p.Y - end.Y, 2));

        var openSet = new List<(Point, int)> { (start, 0) };

        var cameFrom = new Dictionary<(Point, int), (Point, int)>();

        var gScore = new Dictionary<(Point, int), int> { { (start, 0), 0 } }; // cheapest path from start to current
        var fScore = new Dictionary<(Point, int), double>
            { { (start, 0), h(start) } }; // cheapest path from start to end through current

        while (openSet.Any())
        {
            var current = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (fScore[openSet[i]] < fScore[current])
                {
                    current = openSet[i];
                }
            }

            openSet.Remove(current);

            var currentPos = current.Item1;

            if (currentPos == end)
            {
                Console.WriteLine("tadaaa");
                break;
            }

            var neighbors = new List<Point>
            {
                currentPos with { X = currentPos.X + 1 },
                new(currentPos.X - 1, currentPos.Y),
                new(currentPos.X, currentPos.Y + 1),
                new(currentPos.X, currentPos.Y - 1),
                currentPos
            }.Where(p => p == start || p == end ||
                         (p.X > 0 && p.Y > 0 && p.X < map[0].Length - 1 && p.Y < map.Length - 1) &&
                         blizzards.Select(b => b.PositionAt(current.Item2 + 1)).All(b => b != p));

            foreach (var point in neighbors)
            {
                var tentativeG = gScore[current] + 1;
                if (!gScore.ContainsKey((point, current.Item2 + 1)) || tentativeG < gScore[(point, current.Item2 + 1)])
                {
                    cameFrom[(point, current.Item2 + 1)] = current;
                    gScore[(point, current.Item2 + 1)] = tentativeG;
                    fScore[(point, current.Item2 + 1)] = tentativeG + h(point);
                    if (!openSet.Contains((point, current.Item2 + 1)))
                        openSet.Add((point, current.Item2 + 1));
                }
            }
        }

        var endKey = gScore.Keys.Single(k => k.Item1 == end);

        Console.WriteLine(gScore[endKey]);

        var currentt = endKey;
        do
        {
            Console.WriteLine(currentt.Item1);
            currentt = cameFrom[currentt];
        } while (currentt.Item1 != start);

        Console.WriteLine(start);
    }

    public void Solve2(string[] lines)
    {
        map = lines;
        var blizzards = new List<Blizzard>();
        var start = Point.Empty;
        var end = Point.Empty;
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[0].Length; x++)
            {
                if (y == 0 && map[y][x] == '.')
                {
                    start = new Point(x, y);
                }
                else if (y == map.Length - 1 && map[y][x] == '.')
                {
                    end = new Point(x, y);
                }
                else if (new[] { '<', '>', 'v', '^' }.Contains(map[y][x]))
                {
                    blizzards.Add(new Blizzard
                    {
                        Direction = (Direction)map[y][x],
                        StartPosition = new Point(x, y)
                    });
                }
            }
        }
        
        var goal = end;

        var h = (Point p) => Math.Sqrt(Math.Pow(p.X - goal.X, 2) + Math.Pow(p.Y - goal.Y, 2));

        var openSet = new List<(Point, int)> { (start, 0) };
        
        var gScore = new Dictionary<(Point, int), int> { { (start, 0), 0 } }; // cheapest path from start to current
        var fScore = new Dictionary<(Point, int), double>
            { { (start, 0), h(start) } }; // cheapest path from start to end through current

        var endReached = false;
        while (openSet.Any())
        {
            var current = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (fScore[openSet[i]] < fScore[current])
                {
                    current = openSet[i];
                }
            }

            openSet.Remove(current);

            var currentPos = current.Item1;

            if (currentPos == goal)
            {
                if (!endReached)
                {
                    Console.WriteLine("Came to end first time");
                    Console.WriteLine(current.Item2);
                    endReached = true;
                    goal = start;
                    openSet.Clear();
                    fScore.Clear();
                    fScore.Add(current, h(currentPos));
                    gScore.Clear();
                    gScore.Add(current, current.Item2);
                }else if (goal == start)
                {
                    Console.WriteLine("Came back at start for snack");
                    Console.WriteLine(current.Item2);
                    goal = end;
                    openSet.Clear();
                    fScore.Clear();
                    fScore.Add(current, h(currentPos));
                    gScore.Clear();
                    gScore.Add(current, current.Item2);
                }
                else
                {
                    Console.WriteLine(@"I'm at the end with all snacks \o/");
                    Console.WriteLine(current.Item2);
                    break;
                }
            }

            var neighbors = new List<Point>
            {
                currentPos with { X = currentPos.X + 1 },
                new(currentPos.X - 1, currentPos.Y),
                new(currentPos.X, currentPos.Y + 1),
                new(currentPos.X, currentPos.Y - 1),
                currentPos
            }.Where(p => p == start || p == end ||
                         (p.X > 0 && p.Y > 0 && p.X < map[0].Length - 1 && p.Y < map.Length - 1) &&
                         blizzards.Select(b => b.PositionAt(current.Item2 + 1)).All(b => b != p));

            foreach (var point in neighbors)
            {
                var tentativeG = gScore[current] + 1;
                if (!gScore.ContainsKey((point, current.Item2 + 1)) || tentativeG < gScore[(point, current.Item2 + 1)])
                {
                    gScore[(point, current.Item2 + 1)] = tentativeG;
                    fScore[(point, current.Item2 + 1)] = tentativeG + h(point);
                    if (!openSet.Contains((point, current.Item2 + 1)))
                        openSet.Add((point, current.Item2 + 1));
                }
            }
        }
    }
}