using System.Text.RegularExpressions;

namespace AdventOfCode.task._2025;

public class Task06 : ITask
{
    public void Solve(string[] lines)
    {
        var columns = lines.Select(e => Regex.Replace(e.Trim(), "\\s+", " ").Split(" ")).ToArray();
        var operations = columns[lines.Length - 1];
        var numbers = columns.Take(lines.Length - 1).Select(e => e.Select(int.Parse).ToArray()).ToArray();
        var results = new List<long>();
        for (var i = 0; i < operations.Length; i++)
        {
            var operands = new List<int>();
            for (int j = 0; j < lines.Length - 1; j++)
            {
                operands.Add(numbers[j][i]);
            }

            if (operations[i] == "+")
            {
                results.Add(operands.Sum(e => (long)e));
            }
            else if (operations[i] == "*")
            {
                long prod = 1;
                foreach (var op in operands)
                {
                    prod *= op;
                }

                results.Add(prod);
            }
        }

        Console.WriteLine(results.Sum());
    }

    public void Solve2(string[] lines)
    {
        var operations = Regex.Replace(lines[^1].Trim(), "\\s+", " ").Split(" ");
        var columnIdx = 0;
        var numbers = lines.Take(lines.Length - 1).ToArray();

        var results = new List<long>();
        foreach (var operation in operations)
        {
            var end = columnIdx;
            while (end < numbers[0].Length && numbers.Any(n => n[end] != ' '))
            {
                end++;
            }

            var operands = new List<long>();
            for (int i = columnIdx; i < end; i++)
            {
                var numStr = "";
                foreach (var numbersLine in numbers)
                {
                    numStr += numbersLine[i];
                }

                operands.Add(long.Parse(numStr.Trim()));
            }

            if (operation == "+")
            {
                results.Add(operands.Sum(e => (long)e));
            }
            else if (operation == "*")
            {
                long prod = 1;
                foreach (var op in operands)
                {
                    prod *= op;
                }

                results.Add(prod);
            }

            columnIdx = end + 1;
        }

        Console.WriteLine(results.Sum());
    }
}
