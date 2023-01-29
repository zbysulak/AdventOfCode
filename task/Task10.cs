namespace AdventOfCode;

public class Task10 : ITask
{
    public void Solve(string[] lines)
    {
        var sumOfStrengths = 0;
        var cycles = new[] { 20, 60, 100, 140, 180, 220 };
        var x = 1;
        var tick = 0;
        int? numberToAddInNextCycle = null;
        int i = 0; // current command
        while (i < lines.Length)
        {
            tick++;

            //Console.Write($"tick {tick}");

            if (numberToAddInNextCycle is not null)
            {
                x += numberToAddInNextCycle.Value;
                numberToAddInNextCycle = null;
            }
            else
            {
                switch (lines[i].Substring(0, 4))
                {
                    case "addx":
                        numberToAddInNextCycle = int.Parse(lines[i].Split(" ")[1]);
                        break;
                }

                //Console.Write($", {lines[i]}");

                i++;
            }

            //Console.WriteLine($", X={x}");

            if (cycles.Contains(tick))
            {
                sumOfStrengths += tick * x;
                Console.WriteLine(tick + " * " + x);
            }
        }

        Console.WriteLine(sumOfStrengths);
    }

    public void Solve2(string[] lines)
    {
        var x = 1;
        var tick = 0;
        int? numberToAddInNextCycle = null;
        int i = 0; // current command
        while (i < lines.Length)
        {
            tick++;

            if (numberToAddInNextCycle is not null)
            {
                x += numberToAddInNextCycle.Value;
                numberToAddInNextCycle = null;
            }
            else
            {
                switch (lines[i].Substring(0, 4))
                {
                    case "addx":
                        numberToAddInNextCycle = int.Parse(lines[i].Split(" ")[1]);
                        break;
                }

                i++;
            }
            
            if (Math.Abs(tick % 40 - x) <= 1)
                Console.Write("#");
            else
                Console.Write(".");

            if (tick % 40 == 0) Console.WriteLine();
        }
        
        // for some reason it is shifted one pixel :(
    }
}