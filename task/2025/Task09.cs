namespace AdventOfCode.task._2025;

public class Task09 : ITask
{
    public void Solve(string[] lines)
    {
        var coords = lines.Select(e => e.Split(",")).Select(e => (int.Parse(e[0]), int.Parse(e[1]))).ToArray();
        var maxArea = 0L;
        foreach (var c1 in coords)
        {
            foreach (var c2 in coords)
            {
                if (c1 == c2) continue;
                var area = (1L + Math.Abs(c1.Item1 - c2.Item1)) * (1 + Math.Abs(c1.Item2 - c2.Item2));
                if (area > maxArea)
                    maxArea = area;
            }
        }

        Console.WriteLine(maxArea); //2147366520 too low
    }

    private bool[,] colorfulTiles;

    public void Solve2(string[] lines)
    {
        var coords = lines.Select(e => e.Split(",")).Select(e => (int.Parse(e[0]), int.Parse(e[1]))).ToArray();
        FillTiles(coords);
        var maxArea = 0L;
        foreach (var c1 in coords)
        {
            foreach (var c2 in coords)
            {
                if (c1 == c2) continue;
                var area = (1L + Math.Abs(c1.Item1 - c2.Item1)) * (1 + Math.Abs(c1.Item2 - c2.Item2));
                if (IsInside(coords, c1, c2) && area > maxArea)
                    maxArea = area;
            }
        }

        Console.WriteLine(maxArea);
    }

    private void FillTiles((int, int)[] coords)
    {
        Console.WriteLine();
        var maxX = coords.Max(e => e.Item1);
        var maxY = coords.Max(e => e.Item2);
        colorfulTiles = new bool[maxX + 1, maxY + 1];
        for (int y = 0; y <= maxY; y++)
        {
            for (int x = 0; x <= maxX; x++)
            {
                for (int i = 0; i < coords.Length; i++)
                {
                    var i2 = (i + 1) % coords.Length;
                    if ((x == coords[i].Item1 && x == coords[i2].Item1 &&
                         y <= Math.Max(coords[i].Item2, coords[i2].Item2) &&
                         y >= Math.Min(coords[i].Item2, coords[i2].Item2)) ||
                        (y == coords[i].Item2 && y == coords[i2].Item2 &&
                         x <= Math.Max(coords[i].Item1, coords[i2].Item1) &&
                         x >= Math.Min(coords[i].Item1, coords[i2].Item1)))
                    {
                        colorfulTiles[x, y] = true;
                        break;
                    }
                }

                if (!colorfulTiles[x, y])
                {
                    var intersectionsX = 0;
                    var intersectionsY = 0;
                    for (var i = 0; i < coords.Length; i++)
                    {
                        var i2 = (i + 1) % coords.Length;
                        var p1 = coords[i];
                        var p2 = coords[i2];
                        if (x > p1.Item1 && p1.Item1 == p2.Item1 && y >= Math.Min(p1.Item2, p2.Item2) &&
                            y <= Math.Max(p1.Item2, p2.Item2))
                        {
                            intersectionsX++;
                        }
                        if (y > p1.Item2 && p1.Item2 == p2.Item2 && x >= Math.Min(p1.Item1, p2.Item1) &&
                            x <= Math.Max(p1.Item1, p2.Item1))
                        {
                            intersectionsY++;
                        }
                    }

                    colorfulTiles[x, y] = intersectionsX % 2 == 1 || intersectionsY % 2 == 1;
                }

                Console.Write(colorfulTiles[x, y] ? '#' : '.');
            }

            Console.WriteLine();
        }
    }

    private bool IsInside((int, int)[] coords, (int, int) c1, (int, int) c2)
    {
        for (int y = Math.Min(c1.Item2, c2.Item2); y < Math.Max(c1.Item2, c2.Item2); y++)
        {
            for (int x = Math.Min(c1.Item1, c2.Item1); x < Math.Max(c1.Item1, c2.Item1); x++)
            {
                if (!colorfulTiles[x, y])
                    return false;
            }
        }

        return true;
    }
}
