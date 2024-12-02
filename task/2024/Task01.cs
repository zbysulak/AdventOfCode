// Day 1: Calorie Counting

namespace AdventOfCode.task._2024;

public class Task01 : ITask
{
    // done in 9 minutes
    public void Solve(string[] lines)
    {
        var l = new int[lines.Length];
        var r = new int[lines.Length];
        for (int i = 0; i < l.Length; i++)
        {
            var s = lines[i].Split("   ");
            l[i] = int.Parse(s[0]);
            r[i] = int.Parse(s[1]);
        }

        Array.Sort(l);
        Array.Sort(r);

        var sum = 0;
        for (int i = 0; i < l.Length; i++)
        {
            sum += Math.Abs(l[i] - r[i]);
        }

        Console.WriteLine(sum);
    }


    // done in 23 minutes
    public void Solve2(string[] lines)
    {
        var l = new int[lines.Length];
        var r = new int[lines.Length];
        for (int i = 0; i < l.Length; i++)
        {
            var s = lines[i].Split("   ");
            l[i] = int.Parse(s[0]);
            r[i] = int.Parse(s[1]);
        }

        Array.Sort(l);
        Array.Sort(r);

        var similarity = 0;
        var idx = 0;
        var idxR = 0;
        while (idx < l.Length && idxR < r.Length)
        {
            var count = 0;
            var oldIdx = idxR;
            while (idxR < r.Length && l[idx] > r[idxR])
                idxR++;
            while (idxR < r.Length && l[idx] == r[idxR])
            {
                idxR++;
                count++;
            }

            similarity += count * l[idx];
            idx++;
            idxR = oldIdx;
        }

        Console.WriteLine(similarity);
    }
}
