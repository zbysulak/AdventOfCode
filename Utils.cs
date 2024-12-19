namespace AdventOfCode;

public class Utils
{
    public static bool CheckBounds(string[] array, int x, int y)
    {
        return x >= 0 && x < array[0].Length && y >= 0 && y < array.Length;
    }

    public static bool CheckBounds(string[] array, (int x, int y) pos)
    {
        return CheckBounds(array, pos.x, pos.y);
    }

    public static bool CheckBounds(object[][] array, int x, int y)
    {
        return x >= 0 && x < array[0].Length && y >= 0 && y < array.Length;
    }

    public static bool CheckBounds(char[][] array, (int x, int y) pos)
    {
        return pos.x >= 0 && pos.x < array[0].Length && pos.y >= 0 && pos.y < array.Length;
    }

    public static bool CheckBounds(object[][] array, int[] pos)
    {
        if (pos.Length > 2)
            throw new ArgumentException();
        return CheckBounds(array, pos[0], pos[1]);
    }

    /// <summary>
    /// from up, cw
    /// </summary>
    public static readonly int[][] Directions4 = { new[] { 0, -1 }, new[] { 1, 0 }, new[] { 0, 1 }, new[] { -1, 0 } };

    /// <summary>
    /// from up, cw
    /// </summary>
    public static readonly int[][] Directions8 =
    {
        new[] { 0, -1 }, new[] { 1, -1 },
        new[] { 1, 0 }, new[] { 1, 1 },
        new[] { 0, 1 }, new[] { -1, 1 },
        new[] { -1, 0 }, new[] { -1, -1 }
    };

    public static IEnumerable<(int x, int y)> IterateOverGrid(char[][] grid, bool printLine = false)
    {
        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                yield return (x, y);
            }

            if (printLine)
                Console.WriteLine();
        }
    }

    public static (int x, int y) FindInGrid(char[][] grid, char c)
    {
        foreach (var (x, y) in IterateOverGrid(grid))
        {
            if (grid[y][x] == c)
                return (x, y);
        }

        throw new Exception("Not found");
    }
}
