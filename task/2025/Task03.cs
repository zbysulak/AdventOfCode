namespace AdventOfCode.task._2025;

public class Task03 : ITask
{
    public void Solve(string[] lines)
    {
        var total = 0;
        foreach (var line in lines)
        {
            var max1 = '0';
            var maxIdx1 = 0;
            for (int i = 0; i < line.Length - 1; i++)
            {
                if (line[i] > max1)
                {
                    max1 = line[i];
                    maxIdx1 = i;
                }
            }

            var max2 = '0';
            for (int i = maxIdx1 + 1; i < line.Length; i++)
            {
                if (line[i] > max2)
                    max2 = line[i];
            }

            var number = int.Parse($"{max1}{max2}");
            total += number;
        }

        Console.WriteLine(total);
    }

    public void Solve2(string[] lines)
    {
        const int noBatteries = 12;
        var total = 0L;
        foreach (var line in lines)
        {
            var sequence = "";
            var lastDigitIdx = 0;
            for (int i = 0; i < noBatteries; i++)
            {
                var largestDigit = '0';
                for (int j = lastDigitIdx; j <= line.Length - noBatteries + i; j++)
                {
                    if (line[j] > largestDigit)
                    {
                        largestDigit = line[j];
                        lastDigitIdx = j;
                    }
                }

                sequence += largestDigit;
                lastDigitIdx++;
            }

            var number = long.Parse(sequence);
            Console.WriteLine(number);
            total += number;
        }

        Console.WriteLine(total);
    }
}
