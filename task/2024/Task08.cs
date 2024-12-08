// Day 8

namespace AdventOfCode.task._2024;

public class Task08 : ITask
{
    // 8:31
    // done in 23 minutes
    public void Solve(string[] lines)
    {
        var antinodes = new HashSet<(int, int)>();

        var antennas = new Dictionary<char, IList<(int, int)>>();

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                var c = lines[y][x];
                if (c != '.')
                {
                    if (!antennas.ContainsKey(c))
                    {
                        antennas[c] = new List<(int, int)> { (x, y) };
                        continue;
                    }
                    else
                    {
                        var foundAntennas = antennas[c];
                        foreach (var antenna in foundAntennas)
                        {
                            var dx = antenna.Item1 - x;
                            var dy = antenna.Item2 - y;
                            if (Utils.CheckBounds(lines, x + 2 * dx, y + 2 * dy))
                                antinodes.Add((x + 2 * dx, y + 2 * dy));
                            if (Utils.CheckBounds(lines, x - dx, y - dy))
                                antinodes.Add((x - dx, y - dy));
                        }

                        antennas[c].Add((x, y));
                    }
                }
            }
        }

        Console.WriteLine(antinodes.Count);
        /* printing result
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                Console.Write(antinodes.Contains((x, y))
                    ? ("#" + (lines[y][x] != '.' ? lines[y][x] : ' '))
                    : (lines[y][x] + " "));
            }

            Console.WriteLine();
        } */
    }

    //done in 32 minutes
    public void Solve2(string[] lines)
    {
        var antinodes = new HashSet<(int, int)>();

        var antennas = new Dictionary<char, IList<(int, int)>>();

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                var c = lines[y][x];
                if (c != '.')
                {
                    if (!antennas.ContainsKey(c))
                    {
                        antennas[c] = new List<(int, int)> { (x, y) };
                        continue;
                    }
                    else
                    {
                        var foundAntennas = antennas[c];
                        foreach (var antenna in foundAntennas)
                        {
                            var dx = antenna.Item1 - x;
                            var dy = antenna.Item2 - y;
                            var i = 0;
                            while (Utils.CheckBounds(lines, antenna.Item1 + i * dx, antenna.Item2 + i * dy))
                            {
                                antinodes.Add((antenna.Item1 + i * dx, antenna.Item2 + i * dy));
                                i++;
                            }

                            i = 0;
                            while (Utils.CheckBounds(lines, x - i * dx, y - i * dy))
                            {
                                antinodes.Add((x - i * dx, y - i * dy));
                                i++;
                            }
                        }

                        antennas[c].Add((x, y));
                    }
                }
            }
        }

        Console.WriteLine(antinodes.Count);
        /* printing result
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                Console.Write(antinodes.Contains((x, y))
                    ? ("#" + (lines[y][x] != '.' ? lines[y][x] : ' '))
                    : (lines[y][x] + " "));
            }

            Console.WriteLine();
        } */
    }
}
