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

    public void Solve2(string[] lines)
    {
        var coords = lines.Select(e => e.Split(",")).Select(e => (int.Parse(e[0]), int.Parse(e[1]))).ToArray();
        var maxArea = 0L;
        var i = 0;
        for (var j = 0; j < coords.Length; j++)
        {
            var c1 = coords[j];
            for (var k = j + 1; k < coords.Length; k++)
            {
                var c2 = coords[k];
                var area = (1L + Math.Abs(c1.Item1 - c2.Item1)) * (1 + Math.Abs(c1.Item2 - c2.Item2));
                if (area > maxArea && IsInside(coords, c1, c2))
                    maxArea = area;
                i++;
            }

            Console.WriteLine(i);
        }

        Console.WriteLine(maxArea); // 1564768836 it too high :(
    }

    private bool IsInside((int, int)[] coords, int x, int y)
    {
        var intersectionsX = 0;
        var intersectionsY = 0;
        var bound = false;
        for (int i = 0; i < coords.Length; i++)
        {
            var i2 = (i + 1) % coords.Length;
            var p1 = coords[i];
            var p2 = coords[i2];
            if ((x == p1.Item1 && x == p2.Item1 &&
                 y <= Math.Max(p1.Item2, p2.Item2) &&
                 y >= Math.Min(p1.Item2, p2.Item2)) ||
                (y == p1.Item2 && y == p2.Item2 &&
                 x <= Math.Max(p1.Item1, p2.Item1) &&
                 x >= Math.Min(p1.Item1, p2.Item1)))
            {
                bound = true;
                break;
            }

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

        return bound || intersectionsX % 2 == 1 || intersectionsY % 2 == 1;
    }

    private bool IsInside((int, int)[] coords, (int, int) c1, (int, int) c2)
    {
        for (int y = Math.Min(c1.Item2, c2.Item2); y < Math.Max(c1.Item2, c2.Item2); y++)
        {
            foreach (int x in new[] { c1.Item1, c2.Item1 })
            {
                // todo: to optimise I can "walk" on boundary and check only if I cross any border.
                // todo: I can check which borders I'll cross in advance
                if (!IsInside(coords, x, y))
                    return false;
            }
        }

        for (int x = Math.Min(c1.Item1, c2.Item1); x < Math.Max(c1.Item1, c2.Item1); x++)
        {
            foreach (int y in new[] { c1.Item2, c2.Item2 })
            {
                if (!IsInside(coords, x, y))
                    return false;
            }
        }

        return true;
    }
}
