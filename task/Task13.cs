namespace AdventOfCode;

public class Task13 : ITask
{
    public interface IMessageObject
    {
    }

    public class MessageList : List<IMessageObject>, IMessageObject
    {
        public MessageList(string input)
        {
            input = input.Substring(1, input.Length - 2); // strip braces
            var pos = 0;
            while (pos < input.Length)
            {
                if (input[pos] == ',')
                {
                    pos++;
                }
                else if (input[pos] == '[')
                {
                    var a = pos;
                    var braceCount = 0;
                    do
                    {
                        if (input[a] == '[')
                            braceCount++;
                        if (input[a] == ']')
                            braceCount--;
                        a++;
                    } while (braceCount > 0);

                    var newList = input.Substring(pos, a - pos);
                    this.Add(new MessageList(newList));
                    input = input.Substring(0, pos) + input.Substring(a);
                }
                else
                {
                    // it has to be number
                    // there are only numbers 0-10 (including) 
                    if (input[pos] == '1' && input.Length > pos + 1 && input[pos + 1] == '0')
                    {
                        pos++;
                        Add(new MessageInt(10));
                    }
                    else
                    {
                        Add(new MessageInt(int.Parse(input[pos].ToString())));
                    }

                    pos++;
                }
            }
        }

        public MessageList(MessageInt number)
        {
            Add(number);
        }

        public override string ToString()
        {
            return $"[{String.Join(",", this)}]";
        }
    }

    public class MessageInt : IMessageObject
    {
        public int Value { get; set; }

        public MessageInt(int value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    private bool? Check(MessageList left, MessageList right)
    {
        for (int i = 0; i < left.Count && i < right.Count; i++)
        {
            if (left[i] is MessageInt i1 && right[i] is MessageInt i2)
            {
                if (i1.Value < i2.Value)
                {
                    //Console.WriteLine($"Passed cuz {i1.Value} < {i2.Value}");
                    return true;
                }
                else if (i1.Value > i2.Value)
                {
                    //Console.WriteLine($"Failed cuz {i1.Value} > {i2.Value}");
                    return false;
                } // just continue otherwise
            }
            else if (left[i] is MessageList l1 && right[i] is MessageList l2)
            {
                var r = Check(l1, l2);
                if (r is not null) return r;
            }
            else
            {
                if (left[i] is MessageInt l)
                {
                    var tmpLeft = new MessageList(l);
                    var r = Check(tmpLeft, right[i] as MessageList);
                    if (r is not null) return r;
                }
                else
                {
                    var tmpRight = new MessageList(right[i] as MessageInt);
                    var r = Check(left[i] as MessageList, tmpRight);
                    if (r is not null) return r;
                }
            }
        }

        if (left.Count < right.Count)
        {
            //Console.WriteLine($"Passed cuz size {left.Count} < {right.Count}");
            return true;
        }

        if (left.Count > right.Count)
        {
            //Console.WriteLine($"Failed cuz size {left.Count} > {right.Count}");
            return false;
        }

        return null;
    }

    public void Solve(string[] lines)
    {
        var rightPairs = 0;
        for (int i = 0; i < lines.Length / 3; i++)
        {
            var idx = i * 3;
            if (Check(new MessageList(lines[idx]), new MessageList(lines[idx + 1])) ?? false)
            {
                //Console.WriteLine(i + 1);
                rightPairs += i + 1;
            }
        }

        Console.WriteLine(rightPairs);

        var a = new MessageList(lines[3]);
        Console.WriteLine(a);
    }

    public void Solve2(string[] lines)
    {
        var special1 = "[[2]]";
        var special2 = "[[6]]";
        var messages = new List<MessageList> { new(special1), new(special2) };
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line)) continue;
            messages.Add(new(line));
        }

        var arr = messages.ToArray();
        Array.Sort(arr, Comparer<MessageList>.Create(
            (m1, m2) =>
            {
                var ch = Check(m1, m2);
                if (ch is null) return 0;
                //Console.WriteLine($"{m1} is {(ch.Value?"smaller":"bigger")} than {m2}");
                return ch.Value ? -1 : 1;
            }));

        foreach (var message in arr)
        {
            Console.WriteLine(message);
        }

        var result = arr.Select(m => m.ToString()).ToList();
        Console.WriteLine((result.IndexOf(special1) + 1) * (result.IndexOf(special2) + 1));
    }
}