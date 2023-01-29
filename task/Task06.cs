// Day 6: Tuning Trouble

namespace AdventOfCode;

public class Task06 : ITask
{
    public void Solve(string[] lines)
    {
        foreach (var line in lines)
        {
            var currentPos = 0;
            while (currentPos < line.Length+4 && !AreAllDifferent(line.Substring(currentPos, 4)))
            {
                currentPos++;
            }
            Console.WriteLine(currentPos+4);
        }
    }
    
    public void Solve2(string[] lines)
    {
        foreach (var line in lines)
        {
            var currentPos = 0;
            while (currentPos < line.Length+14 && !AreAllDifferent(line.Substring(currentPos, 14)))
            {
                currentPos++;
            }
            Console.WriteLine(currentPos+14);
        }
    }

    private bool AreAllDifferent(string str)
    {
        //Console.WriteLine($"checking {str}");
        var diff = true;
        for (int i = 0; i < str.Length-1; i++)
        {
            //Console.WriteLine($"{str.Substring(i+1)} contains {str[i]}?");
            diff &= !str.Substring(i+1).Contains(str[i]);
        }
        return diff;
    }
}