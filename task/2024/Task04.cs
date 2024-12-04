// Day 4

namespace AdventOfCode.task._2024;

public class Task04 : ITask
{
    //done in 12 minutes
    public void Solve(string[] lines)
    {
        var sum = 0;
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                sum += Check(lines, x, y);
            }
        }

        Console.WriteLine(sum);
    }

    private int Check(string[] lines, int x, int y)
    {
        const string needle = "XMAS";
        var coords = new[,]
        {
            { +1, +1 }, // rd diagonal
            { -1, +1 }, // ld diagonal
            { +1, -1 }, //ru
            { -1, -1 }, //lu
            { +1, 0 }, //r
            { -1, 0 }, //l
            { 0, +1 }, //d
            { 0, -1 } //u
        };
        var total = 0;
        for (int d = 0; d < coords.GetLength(0); d++)
        {
            for (int i = 0; i < 4; i++)
            {
                var _x = x + i * coords[d, 0];
                var _y = y + i * coords[d, 1];

                if (_y < 0 || _y >= lines.Length || _x < 0 || _x >= lines[_y].Length)
                    break;
                if (lines[_y][_x] != needle[i])
                    break;
                if (i == 3)
                    total++;
            }
        }

        return total;
    }

    //done in 31 minutes
    public void Solve2(string[] lines)
    {
        var sum = 0;
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                sum += XMasCheck(lines, x, y) ? 1 : 0;
            }
        }

        Console.WriteLine(sum);
    }

    private bool XMasCheck(string[] lines, int x, int y)
    {
        var xmas = new string[4][];
        xmas[0] = new string[] { "M.M", ".A.", "S.S" };
        xmas[1] = new string[] { "S.M", ".A.", "S.M" };
        xmas[2] = new string[] { "M.S", ".A.", "M.S" };
        xmas[3] = new string[] { "S.S", ".A.", "M.M" };
        if (y > lines.Length - 3)
            return false;
        if (x > lines[y].Length - 3)
            return false;

        return xmas.Any(e => XMasCheckSingle(lines, e, x, y));
    }

    private bool XMasCheckSingle(string[] lines, string[] xmas, int x, int y)
    {
        for (int _y = 0; _y < xmas.Length; _y++)
        {
            for (int _x = 0; _x < xmas[_y].Length; _x++)
            {
                if (xmas[_y][_x] != '.' && lines[y + _y][x + _x] != xmas[_y][_x])
                    return false;
            }
        }

        return true;
    }
}
