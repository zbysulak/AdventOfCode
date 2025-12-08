namespace AdventOfCode.task._2025;

public class Task07 : ITask
{
    public void Solve(string[] lines)
    {
        var beams = new bool[lines[0].Length];
        beams[Array.IndexOf(lines[0].ToCharArray(), 'S')] = true;
        var splits = 0;
        foreach (var line in lines.Skip(1))
        {
            var newBeams = new bool[lines[0].Length];
            for (var i = 0; i < line.Length; i++)
            {
                if (!beams[i]) continue;
                if (line[i] == '^')
                {
                    splits++;
                    newBeams[i + 1] = newBeams[i - 1] = true;
                }
                else
                {
                    newBeams[i] = true;
                }
            }

            beams = newBeams;
        }

        Console.WriteLine(splits);
    }

    public void Solve2(string[] lines)
    {
        Console.WriteLine();
        var beams = new long[lines[0].Length];
        beams[Array.IndexOf(lines[0].ToCharArray(), 'S')] = 1;
        for (var l = 2; l < lines.Length; l += 2)
        {
            var line = lines[l];
            var newBeams = new long[line.Length];
            for (var i = 0; i < line.Length; i++)
            {
                if (beams[i] == 0) continue;
                if (line[i] == '^')
                {
                    newBeams[i + 1] += beams[i];
                    newBeams[i - 1] += beams[i];
                    newBeams[i] = 0;
                }
                else
                {
                    newBeams[i] += beams[i];
                }
            }

            beams = newBeams.ToArray();
        }

        Console.WriteLine(beams.Sum());
    }
}
