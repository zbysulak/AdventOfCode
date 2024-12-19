// Day 17

namespace AdventOfCode.task._2024;

public class Task17 : ITask
{
    public void Solve(string[] lines)
    {
        var regA = int.Parse(lines[0].Substring(12));
        var regB = int.Parse(lines[1].Substring(12));
        var regC = int.Parse(lines[2].Substring(12));
        var commands = lines[4].Substring(9).Split(',').Select(int.Parse).ToArray();

        var commandPointer = 0;
        while (commandPointer < commands.Length)
        {
            switch (commands[commandPointer])
            {
                case 0: // adv: A = A / 2^co
                    regA = (int)Math.Floor(regA / Math.Pow(2, GetComboOperand()));
                    break;
                case 1: // bxl: B = B xor lo
                    regB = regB ^ commands[commandPointer + 1];
                    break;
                case 2: // bst: B = co % 8
                    regB = GetComboOperand() % 8;
                    break;
                case 3: // jnz 
                    if (regA != 0)
                    {
                        commandPointer = commands[commandPointer + 1] - 2; // cp is not increased by 2
                    }

                    break;
                case 4: // bxc: B = B xor C
                    regB = regB ^ regC;
                    break;
                case 5: // out
                    Console.Write(GetComboOperand() % 8 + ",");
                    break;
                case 6: // bdv 
                    regB = (int)Math.Floor(regA / Math.Pow(2, GetComboOperand()));
                    break;
                case 7: // cdv
                    regC = (int)Math.Floor(regA / Math.Pow(2, GetComboOperand()));
                    break;
            }

            commandPointer += 2;
        }

        Console.WriteLine();
        return;

        int GetComboOperand()
        {
            var c = commands[commandPointer + 1];
            switch (c)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    return c;
                case 4:
                    return regA;
                case 5:
                    return regB;
                case 6:
                    return regC;
                case 7:
                default:
                    throw new Exception("invalid command");
            }
        }
    }

    private List<int> RunProgram(long regAInit, int[] commands)
    {
        var regA = regAInit;
        var regB = 0L;
        var regC = 0L;

        var output = new List<int>();

        var commandPointer = 0;
        while (commandPointer < commands.Length)
        {
            switch (commands[commandPointer])
            {
                case 0: // adv: A = A / 2^co
                    regA = (long)Math.Floor(regA / Math.Pow(2, GetComboOperand()));
                    break;
                case 1: // bxl: B = B xor lo
                    regB = regB ^ commands[commandPointer + 1];
                    break;
                case 2: // bst: B = co % 8
                    regB = GetComboOperand() % 8;
                    break;
                case 3: // jnz 
                    if (regA != 0)
                    {
                        commandPointer = commands[commandPointer + 1] - 2; // cp is not increased by 2
                    }

                    break;
                case 4: // bxc: B = B xor C
                    regB = regB ^ regC;
                    break;
                case 5: // out
                    var c = (int)GetComboOperand() % 8;
                    /*if (commands[output.Count] != c)
                        return false;*/
                    output.Add(c);
                    break;
                case 6: // bdv 
                    regB = (long)Math.Floor(regA / Math.Pow(2, GetComboOperand()));
                    break;
                case 7: // cdv
                    regC = (long)Math.Floor(regA / Math.Pow(2, GetComboOperand()));
                    break;
            }

            commandPointer += 2;
        }

        Console.WriteLine(regAInit + " (" + Convert.ToString(regAInit, 8) + ") -> " + string.Join(",", output));

        /*for (int i = 0; i < commands.Length; i++)
        {
            if (output.Count <= i || output[i] != commands[i])
                return false;
        }

        return true;*/
        return output;

        long GetComboOperand()
        {
            var c = commands[commandPointer + 1];
            switch (c)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    return c;
                case 4:
                    return regA;
                case 5:
                    return regB;
                case 6:
                    return regC;
                case 7:
                default:
                    throw new Exception("invalid command");
            }
        }
    }

    public void Solve2(string[] lines)
    {
        var commands = lines[4].Substring(9).Split(',').Select(int.Parse).ToArray();

        var regAoct = (long)Math.Pow(10, commands.Length - 1); // it has to be same lenght as number of commands
        var possibleStarts = new List<long> { 0L };

        for (int i = commands.Length - 1; i > 0; i--)
        {
            var nextStarts = new List<long>();
            foreach (var start in possibleStarts)
            {
                var inc = (long)Math.Pow(10, i);
                for (int j = 0; j < 8; j++)
                {
                    var toTry = start + j * inc;
                    var regA = Convert.ToInt64(toTry.ToString(), 8);
                    var output = RunProgram(regA, commands);
                    if (output.Count == commands.Length && output[i] == commands[i])
                    {
                        nextStarts.Add(toTry);
                    }
                }
            }

            possibleStarts = nextStarts;
        }

        Console.WriteLine(Convert.ToInt64(regAoct.ToString(), 8));
    }
}
