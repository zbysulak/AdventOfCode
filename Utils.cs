namespace AdventOfCode;

public class Utils
{
    public static bool CheckBounds(object[][] array, int x, int y)
    {
        return x >= 0 && x < array[0].Length && y >= 0 && y < array.Length;
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
}
