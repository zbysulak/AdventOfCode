namespace AdventOfCode.task._2022;

public class Task22 : ITask
{
    public void Solve(string[] lines)
    {
        var map = lines.Take(lines.Length - 2).Select(l => l.PadRight(lines.Take(lines.Length - 2).Max(s => s.Length)))
            .ToList();

        var commands = lines[^1];
        var posX = lines[0].IndexOf('.'); // first empty tile from left
        var posY = 0;
        var facing = 0;

        // I assume commands starts and ends with distance and there are no turns right next to each other.
        var distances = commands.Split('L', 'R').Select(int.Parse).ToList();
        var turns = commands.Where(c => c == 'L' || c == 'R').ToList();

        #region helper functions

        var getNextTile = () =>
        {
            switch (facing)
            {
                case 0: // right
                    if (posX + 1 >= map[posY].Length || map[posY][posX + 1] == ' ')
                    {
                        return (map[posY].IndexOf(map[posY].First(ch => ch == '.' || ch == '#')), posY);
                    }
                    else
                    {
                        return (posX + 1, posY);
                    }
                case 1: // down
                    if (posY + 1 >= map.Count() || map[posY + 1][posX] == ' ')
                    {
                        for (int i = 0; i < map.Count(); i++)
                        {
                            if (map[i][posX] != ' ')
                                return (posX, i);
                        }

                        throw new Exception("wut");
                    }
                    else
                    {
                        return (posX, posY + 1);
                    }
                case 2: // left
                    if (posX - 1 < 0 || map[posY][posX - 1] == ' ')
                    {
                        return (map[posY].LastIndexOf(map[posY].Last(ch => ch == '.' || ch == '#')), posY);
                    }
                    else
                    {
                        return (posX - 1, posY);
                    }

                case 3:
                    if (posY - 1 < 0 || map[posY - 1][posX] == ' ')
                    {
                        for (int i = map.Count() - 1; i >= 0; i--)
                        {
                            if (map[i][posX] != ' ')
                                return (posX, i);
                        }

                        throw new Exception("wut");
                    }
                    else
                    {
                        return (posX, posY - 1);
                    }
                default:
                    throw new Exception("o.O");
            }
        };

        var isTileEmpty = (int x, int y) => map[y][x] == '.';

        var go = (int distance) =>
        {
            while (distance > 0)
            {
                var nextTile = getNextTile();
                if (!isTileEmpty(nextTile.Item1, nextTile.Item2)) break;
                posX = nextTile.Item1;
                posY = nextTile.Item2;
                distance--;
            }
        };

        #endregion

        var currentCommand = 0;

        while (currentCommand < distances.Count() - 1)
        {
            go(distances[currentCommand]);
            facing = turns[currentCommand] switch
            {
                'R' => (facing + 1) % 4,
                'L' => (facing + 3) % 4
            };

            /*Console.WriteLine($"{distances[currentCommand]} {turns[currentCommand]}");
            for (int y = 0; y < map.Count(); y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (x == posX && y == posY)
                        Console.Write(facing switch{ 0 => '>', 1=>'v',2=>'<',3=>'^'});
                    else
                        Console.Write(map[y][x]);
                }

                Console.WriteLine();
            }/**/

            currentCommand++;
        }

        go(distances.Last());

        /*for (int y = 0; y < map.Count(); y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                if (x == posX && y == posY)
                    Console.Write(facing switch{ 0 => '>', 1=>'v',2=>'<',3=>'^'});
                else
                    Console.Write(map[y][x]);
            }

            Console.WriteLine();
        }/**/

        Console.WriteLine(1000 * (posY + 1) + 4 * (posX + 1) + facing);
    }

    private record Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Dir { get; set; }

        public Position(int x, int y, int dir)
        {
            X = x;
            Y = y;
            Dir = dir;
        }
    }

    public void Solve2(string[] lines)
    {
        // shape of net is given, size of each square is 50x50 (4x4 in sample)
        var size = lines.Length > 50 ? 50 : 4;

        var map = lines.Take(lines.Length - 2).Select(l => l.PadRight(lines.Take(lines.Length - 2).Max(s => s.Length)))
            .ToList();

        var commands = lines[^1];
        var pos = new Position(lines[0].IndexOf('.'), 0, 0);

        // I assume commands starts and ends with distance and there are no turns right next to each other.
        var distances = commands.Split('L', 'R').Select(int.Parse).ToList();
        var turns = commands.Where(c => c == 'L' || c == 'R').ToList();

        #region helper functions

        Func<Position> getNextTile = () =>
        {
            switch (pos.Dir)
            {
                case 0: // right
                    if (pos.X + 1 >= map[pos.Y].Length || map[pos.Y][pos.X + 1] == ' ')
                    {
                        if (pos.Y < size)
                            return new Position(2 * size - 1, 3 * size - 1 - pos.Y, 2);
                        if (pos.Y < 2 * size)
                            return new Position(pos.Y + size, size - 1, 3);
                        if (pos.Y < 3 * size)
                            return new Position(3 * size - 1, 3 * size - 1 - pos.Y, 2);
                        if (pos.Y < 4 * size)
                            return new Position(pos.Y - 2 * size, 3 * size - 1, 3);
                        throw new Exception("o.O");
                    }
                    else
                    {
                        return pos with { X = pos.X + 1 };
                    }
                case 1: // down
                    if (pos.Y + 1 >= map.Count || map[pos.Y + 1][pos.X] == ' ')
                    {
                        if (pos.X < size)
                            return new Position(pos.X + 2 * size, 0, 1);
                        if (pos.X < 2 * size)
                            return new Position(size - 1, pos.X + 2 * size, 2);
                        if (pos.X < 3 * size)
                            return new Position(2 * size - 1, pos.X - size, 2);
                        throw new Exception("o.O");
                    }
                    else
                    {
                        return pos with { Y = pos.Y + 1 };
                    }
                case 2: // left
                    if (pos.X - 1 < 0 || map[pos.Y][pos.X - 1] == ' ')
                    {
                        if (pos.Y < size)
                            return new Position(0, 3 * size - 1 - pos.Y, 0);
                        if (pos.Y < 2 * size)
                            return new Position(pos.Y - size, 2 * size, 1);
                        if (pos.Y < 3 * size)
                            return new Position(size, 3 * size - 1 - pos.Y, 0);
                        if (pos.Y < 4 * size)
                            return new Position(pos.Y - 2 * size, 0, 1);
                        throw new Exception("o.O");
                    }
                    else
                    {
                        return pos with { X = pos.X - 1 };
                    }

                case 3: // up
                    if (pos.Y - 1 < 0 || map[pos.Y - 1][pos.X] == ' ')
                    {
                        if (pos.X < size)
                            return new Position(size, pos.X + size, 0);
                        if (pos.X < 2 * size)
                            return new Position(0, pos.X + 2 * size, 0);
                        if (pos.X < 3 * size)
                            return new Position(pos.X - 2 * size, 4 * size - 1, 3);
                        throw new Exception("o.O");
                    }
                    else
                    {
                        return pos with { Y = pos.Y - 1 };
                    }
                default:
                    throw new Exception("o.O");
            }
        };

        var isTileEmpty = (int x, int y) => map[y][x] == '.';

        var go = (int distance) =>
        {
            while (distance > 0)
            {
                var nextTile = getNextTile();
                if (!isTileEmpty(nextTile.X, nextTile.Y)) break;
                pos = nextTile;
                distance--;
            }
        };

        #endregion

        var currentCommand = 0;

        while (currentCommand < distances.Count - 1)
        {
            go(distances[currentCommand]);
            pos.Dir = turns[currentCommand] switch
            {
                'R' => (pos.Dir + 1) % 4,
                'L' => (pos.Dir + 3) % 4
            };
            
            /*Console.WriteLine($"{distances[currentCommand]} {turns[currentCommand]}");
            for (int y = 0; y < map.Count(); y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (x == pos.X && y == pos.Y)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(pos.Dir switch { 0 => '>', 1 => 'v', 2 => '<', 3 => '^' });
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                        Console.Write(map[y][x]);
                }

                Console.WriteLine();
            }/**/

            currentCommand++;
        }

        go(distances.Last());
        
        for (int y = 0; y < map.Count(); y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                if (x == pos.X && y == pos.Y)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(pos.Dir switch { 0 => '>', 1 => 'v', 2 => '<', 3 => '^' });
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                    Console.Write(map[y][x]);
            }

            Console.WriteLine();
        }/**/
        Console.WriteLine(1000 * (pos.Y + 1) + 4 * (pos.X + 1) + pos.Dir);
    }
}