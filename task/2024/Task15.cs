// Day 15

namespace AdventOfCode.task._2024;

public class Task15 : ITask
{
    public void Solve(string[] lines)
    {
        Console.WriteLine();
        var gridTmp = new List<string>();
        var i = 0;
        while (!string.IsNullOrEmpty(lines[i]))
        {
            gridTmp.Add(lines[i]);
            i++;
        }

        var grid = gridTmp.Select(e => e.ToArray()).ToArray();

        (int x, int y) pos = (-1, -1);
        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                if (grid[y][x] == '@')
                {
                    pos = (x, y);
                    grid[y][x] = '.';
                    break;
                }
            }
        }

        if (pos is (-1, -1))
            throw new Exception("robot not found");

        PrintGrid(grid, pos);

        while (i < lines.Length)
        {
            var commands = lines[i];
            foreach (var command in commands)
            {
                var dir = command switch
                {
                    '^' => Utils.Directions4[0],
                    '>' => Utils.Directions4[1],
                    'v' => Utils.Directions4[2],
                    '<' => Utils.Directions4[3]
                };

                // Console.WriteLine($"Move {command}:");

                var d = 1;
                var nextD = (pos.x + d * dir[0], pos.y + d * dir[1]);
                while (Utils.CheckBounds(grid, nextD) && grid[nextD.Item2][nextD.Item1] != '.' &&
                       grid[nextD.Item2][nextD.Item1] != '#')
                {
                    nextD = (pos.x + d * dir[0], pos.y + d * dir[1]);
                    d++;
                }

                if (Utils.CheckBounds(grid, nextD) && grid[nextD.Item2][nextD.Item1] != '#')
                {
                    grid[pos.y][pos.x] = '.';
                    pos = (pos.x + dir[0], pos.y + dir[1]);
                    grid[pos.y][pos.x] = '.';
                    grid[nextD.Item2][nextD.Item1] = 'O';
                }

                // PrintGrid(grid, pos);
            }

            i++;
        }

        var totalBoxScore = 0;
        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                if (pos.x == x && pos.y == y) continue;
                if (grid[y][x] == 'O')
                    totalBoxScore += 100 * y + x;
            }
        }

        Console.WriteLine(totalBoxScore);
    }

    public void PrintGrid(char[][] grid, (int x, int y) pos)
    {
        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                if (pos.x == x && pos.y == y)
                    Console.Write('@');
                else
                    Console.Write(grid[y][x]);
            }

            Console.WriteLine();
        }
    }

    public void Solve2(string[] lines)
    {
        Console.WriteLine();

        #region Creating grid

        var gridTmp = new List<string>();
        var i = 0;
        while (!string.IsNullOrEmpty(lines[i]))
        {
            gridTmp.Add(lines[i]);
            i++;
        }

        var grid = gridTmp
            .Select(e => string.Join("",
                e.Select(c =>
                {
                    return c switch
                    {
                        '#' => "##",
                        '.' => "..",
                        '@' => "@.",
                        'O' => "[]"
                    };
                })).ToArray()).ToArray();

        (int x, int y) pos = (-1, -1);
        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                if (grid[y][x] == '@')
                {
                    pos = (x, y);
                    grid[y][x] = '.';
                    break;
                }
            }
        }

        if (pos is (-1, -1))
            throw new Exception("robot not found");

        #endregion

        PrintGrid(grid, pos);

        while (i < lines.Length)
        {
            var commands = lines[i];
            foreach (var command in commands)
            {
                var dir = command switch
                {
                    '^' => 0,
                    '>' => 1,
                    'v' => 2,
                    '<' => 3
                };

                var dirArr = Utils.Directions4[dir];

                // Console.WriteLine($"Move {command}:");

                if (CanMove(grid, pos, dir))
                {
                    grid = Move(grid, pos, dir);
                    pos = (pos.x + dirArr[0], pos.y + dirArr[1]);
                    grid[pos.y][pos.x] = '.';
                }

                // PrintGrid(grid, pos);
            }

            i++;
        }

        PrintGrid(grid, pos);

        var totalBoxScore = 0;
        foreach (var (x, y) in Utils.IterateOverGrid(grid))
        {
            if (pos.x == x && pos.y == y) continue;
            if (grid[y][x] == '[')
                totalBoxScore += 100 * y + x;
        }

        Console.WriteLine(totalBoxScore);
    }

    private bool CanMove(char[][] grid, (int x, int y) pos, int dir)
    {
        var dirA = Utils.Directions4[dir];
        if (dir % 2 == 0)
        {
            var next1 = (pos.x, pos.y + dirA[1]);
            if (grid[next1.Item2][next1.Item1] == '.')
                return true;
            (int, int)? next2 = null;
            if (grid[pos.y + dirA[1]][pos.x] == '[')
                next2 = (pos.x + 1, pos.y + dirA[1]);
            if (grid[pos.y + dirA[1]][pos.x] == ']')
                next2 = (pos.x - 1, pos.y + dirA[1]);

            if (next2 != null)
            {
                return CanMove(grid, next1, dir) && CanMove(grid, next2.Value, dir);
            }

            return false;
        }
        else
        {
            var next = (pos.x + dirA[0], pos.y + dirA[1]);
            if (grid[next.Item2][next.Item1] == '.')
                return true;
            if (grid[next.Item2][next.Item1] == '#')
                return false;

            return CanMove(grid, next, dir);
        }
    }

    private char[][] Move(char[][] grid, (int x, int y) pos, int dir)
    {
        var dy = Utils.Directions4[dir][1];
        var dx = Utils.Directions4[dir][0];
        if (dy != 0)
        {
            var next = (pos.x, pos.y + dy);
            if (grid[next.Item2][next.Item1] == '[')
            {
                grid = Move(grid, next, dir);
                grid = Move(grid, (next.Item1 + 1, next.Item2), dir);
                grid[next.Item2 + dy][next.Item1] = '[';
                grid[next.Item2 + dy][next.Item1 + 1] = ']';
                grid[next.Item2][next.Item1] = '.';
                grid[next.Item2][next.Item1 + 1] = '.';
            }

            if (grid[next.Item2][next.Item1] == ']')
            {
                grid = Move(grid, next, dir);
                grid = Move(grid, (next.Item1 - 1, next.Item2), dir);
                grid[next.Item2 + dy][next.Item1] = ']';
                grid[next.Item2 + dy][next.Item1 - 1] = '[';
                grid[next.Item2][next.Item1] = '.';
                grid[next.Item2][next.Item1 - 1] = '.';
            }

            return grid;
        }
        else
        {
            (int x, int y) next = (pos.x + dx, pos.y);
            if (grid[next.y][next.x] == '.')
            {
                grid[next.y][next.x] = grid[pos.y][pos.x];
                grid[pos.y][pos.x] = '.';
            }
            else
            {
                grid = Move(grid, next, dir);
                grid[next.y][next.x] = grid[pos.y][pos.x];
                grid[pos.y][pos.x] = '.';
            }

            return grid;
        }
    }
}
