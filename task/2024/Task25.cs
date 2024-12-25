// Day 25

namespace AdventOfCode.task._2024;

public class Task25 : ITask
{
    public void Solve(string[] lines)
    {
        var locks = new List<List<int>>();
        var keys = new List<List<int>>();
        for (int i = 0; i < lines.Length; i += 8)
        {
            if (lines[i][0] == '#') // lock
            {
                var l = new List<int>();
                for (int x = 0; x < 5; x++)
                {
                    var pins = 0;
                    for (int y = 1; y < 6; y++)
                    {
                        if (lines[i + y][x] == '#')
                            pins++;
                        else
                            break;
                    }

                    l.Add(pins);
                }

                locks.Add(l);
            }
            else // key
            {
                var k = new List<int>();
                for (int x = 0; x < 5; x++)
                {
                    var pins = 0;
                    for (int y = 5; y >= 0; y--)
                    {
                        if (lines[i + y][x] == '#')
                            pins++;
                        else
                            break;
                    }

                    k.Add(pins);
                }

                keys.Add(k);
            }
        }

        var pairsThatFits = 0;

        foreach (var l in locks)
        {
            foreach (var k in keys)
            {
                var fits = true;
                for (int i = 0; i < 5; i++)
                {
                    if (l[i] + k[i] > 5)
                        fits = false;
                }

                if (fits)
                    pairsThatFits++;
            }
        }

        Console.WriteLine(pairsThatFits);
    }

    public void Solve2(string[] lines)
    {
        // no part 2 :( so I have to do day 24 part 2 
    }
}
