// Day 5: Supply Stacks

public class Task05 : ITask
{
    private class Command
    {
        public int HowMany { get; set; }
        public int From { get; set; }
        public int To { get; set; }

        public Command(string input)
        {
            //move 1 from 2 to 1
            var parts = input.Split(' ');
            HowMany = int.Parse(parts[1]);
            From = int.Parse(parts[3]);
            To = int.Parse(parts[5]);
        }

        public override string ToString()
        {
            return $"move {HowMany} from {From} to {To}";
        }
    }

    private class BetterCrane
    {
        //it really should not be stack anymore.
        public Stack<char>[] Stacks { get; set; }

        public BetterCrane(Stack<char>[] stacks)
        {
            Stacks = stacks;
        }

        public void ApplyCommand(Command command)
        {
            var holding = new char[command.HowMany];
            for (int i = 0; i < command.HowMany; i++)
            {
                holding[i] = Stacks[command.From - 1].Pop();
            }

            for (int i = command.HowMany - 1; i >= 0; i--)
            {
                Stacks[command.To - 1].Push(holding[i]);
            }
        }
    }

    private class Crane
    {
        public Stack<char>[] Stacks { get; set; }

        public Crane(Stack<char>[] stacks)
        {
            Stacks = stacks;
        }

        public void ApplyCommand(Command command)
        {
            for (int i = 0; i < command.HowMany; i++)
            {
                MoveSingle(command.From, command.To);
            }
        }

        private void MoveSingle(int from, int to)
        {
            var item = Stacks[from - 1].Pop();
            Stacks[to - 1].Push(item);
        }
    }

    public void Solve(string[] lines)
    {
        (var stacks, var commands) = ParseInput(lines);
        var crane = new Crane(stacks);
        foreach (var c in commands)
        {
            crane.ApplyCommand(c);
        }

        PrintStacks(stacks);
        /*foreach (var item in commands)
        {
          Console.WriteLine(item);
        }*/
        foreach (var s in stacks)
        {
            Console.Write(s.Peek());
        }

        Console.WriteLine();
    }

    private (Stack<char>[] stacks, List<Command> commands) ParseInput(string[] lines)
    {
        int nStacks = (lines[0].Length + 1) / 4;
        List<char>[] lists = new List<char>[nStacks];

        var commands = new List<Command>();
        for (int s = 0; s < nStacks; s++)
        {
            lists[s] = new List<char>();
        }

        bool parsingCommands = false;
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            if (parsingCommands)
            {
                commands.Add(new Command(line));
            }
            else
            {
                // it is last line of stacks
                if (line[1] == '1')
                {
                    i++;
                    parsingCommands = true;
                    continue;
                }

                for (int j = 0; j < nStacks; j++)
                {
                    var charPos = j * 4 + 1;
                    char ch = line[charPos];
                    if (ch == ' ')
                    {
                        continue;
                    }
                    else
                    {
                        lists[j].Add(ch);
                    }
                }
            }
        }

        // wtfffff
        Stack<char>[] stacks = new Stack<char>[nStacks];

        for (int i = 0; i < nStacks; i++)
        {
            stacks[i] = new Stack<char>();
            for (int j = lists[i].Count() - 1; j >= 0; j--)
            {
                stacks[i].Push(lists[i][j]);
            }
        }

        return (stacks, commands);
    }

    private void PrintStacks(IEnumerable<char>[] stacks)
    {
        foreach (var item in stacks)
        {
            foreach (var ch in item)
            {
                Console.Write(ch + " ");
            }

            Console.WriteLine();
        }
    }

    public void Solve2(string[] lines)
    {
        (var stacks, var commands) = ParseInput(lines);
        var crane = new BetterCrane(stacks);
        foreach (var c in commands)
        {
            crane.ApplyCommand(c);
        }

        PrintStacks(stacks);

        foreach (var s in stacks)
        {
            Console.Write(s.Peek());
        }

        Console.WriteLine();
    }
}