namespace AdventOfCode.task._2022;

public class Task21 : ITask
{
    private const string Root = "root";
    private const string Me = "humn";

    private class Monkey
    {
        public string Name { get; set; }
        public long? Result { get; set; }
        public char Operation { get; set; }
        public string Operand1 { get; set; }
        public string Operand2 { get; set; }

        public Monkey(string input)
        {
            var spl1 = input.Split(':');
            Name = spl1[0];
            if (long.TryParse(spl1[1], out long r))
            {
                Result = r;
            }
            else
            {
                var spl2 = spl1[1].Trim().Split(' ');
                Operand1 = spl2[0];
                Operand2 = spl2[2];
                Operation = spl2[1][0]; // should be only one character
            }
        }

        public void Compute(long? m1, long? m2)
        {
            if (!m1.HasValue || !m2.HasValue) return;
            Result = Operation switch
            {
                '+' => m1 + m2,
                '-' => m1 - m2,
                '/' => m1 / m2,
                '*' => m1 * m2,
                _ => throw new Exception($"Wrong operation {Operation}")
            };
        }
    }

    public void Solve(string[] lines)
    {
        var monkeys = new Dictionary<string, Monkey>();
        foreach (var line in lines)
        {
            var m = new Monkey(line);
            monkeys.Add(m.Name, m);
        }

        while (monkeys[Root].Result is null)
        {
            foreach (var kv in monkeys)
            {
                var m = kv.Value;
                if (m.Result is null)
                {
                    m.Compute(monkeys[m.Operand1].Result, monkeys[m.Operand2].Result);
                }
            }
        }

        Console.WriteLine(monkeys[Root].Result);
    }

    public void Solve2(string[] lines)
    {
        var monkeys = new Dictionary<string, Monkey>();
        // create monkeys
        foreach (var line in lines)
        {
            var m = new Monkey(line);
            monkeys.Add(m.Name, m);
        }

        var root = monkeys[Root];

        // I assume each monkey shouts its number only for ONE other monkey
        // that means one branch of "monkey tree" can be solved:
        while (monkeys[root.Operand1].Result is null && monkeys[root.Operand2].Result is null)
        {
            foreach (var kv in monkeys)
            {
                var m = kv.Value;
                if (m.Operand1 == Me || m.Operand2 == Me)
                {
                    continue;
                }

                if (m.Result is null)
                {
                    m.Compute(monkeys[m.Operand1].Result, monkeys[m.Operand2].Result);
                }
            }
        }

        // now traverse from root all the way to "me" monkey, while computing needed number
        var currentMonkey = root;
        
        // one of roots' monkeys has to have value
        var numberNeeded =
            monkeys[root.Operand1]?.Result ?? monkeys[root.Operand2].Result; 

        do
        {
            // move one node lower in tree
            currentMonkey = monkeys[currentMonkey.Operand1].Result is null
                ? monkeys[currentMonkey.Operand1]
                : monkeys[currentMonkey.Operand2];

            // check if "my" branch is on right or left
            if (monkeys[currentMonkey.Operand1].Result is null || currentMonkey.Operand1 == Me) // left, if left operand is me, that means it is also unknown number
            {
                numberNeeded = currentMonkey.Operation switch
                {
                    '+' => numberNeeded - monkeys[currentMonkey.Operand2].Result,
                    '-' => numberNeeded + monkeys[currentMonkey.Operand2].Result,
                    '/' => numberNeeded * monkeys[currentMonkey.Operand2].Result,
                    '*' => numberNeeded / monkeys[currentMonkey.Operand2].Result
                };
            }
            else // right
            {
                numberNeeded = currentMonkey.Operation switch
                {
                    '+' => numberNeeded - monkeys[currentMonkey.Operand1].Result ,
                    '-' => monkeys[currentMonkey.Operand1].Result - numberNeeded,
                    '/' => monkeys[currentMonkey.Operand1].Result / numberNeeded,
                    '*' => numberNeeded / monkeys[currentMonkey.Operand1].Result
                };
            }
        } while (currentMonkey.Operand1 != Me && currentMonkey.Operand2 != Me);

        Console.WriteLine(numberNeeded);
    }
}