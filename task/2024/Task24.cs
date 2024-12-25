// Day 24

namespace AdventOfCode.task._2024;

public class Task24 : ITask
{
    public void Solve(string[] lines)
    {
        var gates = new Dictionary<string, bool>();
        var i = 0;
        while (!string.IsNullOrEmpty(lines[i]))
        {
            gates.Add(lines[i].Substring(0, 3), lines[i][5] == '1');
            i++;
        }

        var unsolvedGates = new List<string>(lines.Skip(i + 1));

        foreach (var gate in unsolvedGates)
        {
            var parts = gate.Split(" ");
            Console.WriteLine("inputs: " + gate + " : " +
                              gates.Count(pair => pair.Key == parts[0]) + " - " +
                              gates.Count(pair => pair.Key == parts[2]) + " - " + 
                                gates.Count(pair => pair.Key == parts[4]));
        }

        while (unsolvedGates.Any())
        {
            var toRemove = new List<string>();
            foreach (var gate in unsolvedGates)
            {
                var parts = gate.Split(" ");
                if (gates.ContainsKey(parts[0]) && gates.ContainsKey(parts[2]))
                {
                    var output = parts[1] switch
                    {
                        "AND" => gates[parts[0]] && gates[parts[2]],
                        "OR" => gates[parts[0]] || gates[parts[2]],
                        "XOR" => gates[parts[0]] ^ gates[parts[2]]
                    };
                    gates.Add(parts[4], output);
                    toRemove.Add(gate);
                }
            }

            foreach (var g in toRemove)
            {
                unsolvedGates.Remove(g);
            }
        }

        var outputValue = 0L;
        var zGates = gates.Where(g => g.Key.StartsWith("z"))
            .ToDictionary(e => e.Key, e => e.Value);
        var zGatesNames = zGates.Select(g => g.Key).ToList();
        zGatesNames.Sort();
        for (int j = 0; j < zGates.Count; j++)
        {
            var gateName = zGatesNames[j];
            outputValue += zGates[gateName] ? 1L << j : 0L;
        }

        Console.WriteLine(outputValue);
    }

    private bool CheckOutput(long x, long y, int bits, List<string> gates)
    {
        var g = new Dictionary<string, bool>();
        GetInitialGates(x, 'x', bits, g);
        GetInitialGates(y, 'y', bits, g);

        while (gates.Any())
        {
            var toRemove = new List<string>();
            foreach (var gate in gates)
            {
                var parts = gate.Split(" ");
                if (g.ContainsKey(parts[0]) && g.ContainsKey(parts[2]))
                {
                    var output = parts[1] switch
                    {
                        "AND" => g[parts[0]] && g[parts[2]],
                        "OR" => g[parts[0]] || g[parts[2]],
                        "XOR" => g[parts[0]] ^ g[parts[2]]
                    };
                    g.Add(parts[4], output);
                    toRemove.Add(gate);
                }
            }

            foreach (var gtr in toRemove)
            {
                gates.Remove(gtr);
            }
        }

        var outputValue = GatesToLong(g.Where(gate => gate.Key.StartsWith("z")));

        return outputValue == x + y;
    }

    private void GetInitialGates(long n, char name, int bits, Dictionary<string, bool> gates)
    {
        var s = Convert.ToString(n, 2);
        for (int i = bits - 1; i >= 0; i--)
        {
            var idx = bits - 1 - i;
            if (s.Length <= idx)
                gates.Add(name + i.ToString("D2"), false);
            else
                gates.Add(name + i.ToString("D2"), s[idx] == '1');
        }
    }

    private long GatesToLong(IEnumerable<KeyValuePair<string, bool>> gates)
    {
        var dict = gates.ToDictionary(e => e.Key, e => e.Value);
        var gatesNames = gates.Select(g => g.Key).ToList();
        gatesNames.Sort();
        var output = 0L;
        for (int j = 0; j < dict.Count; j++)
        {
            var gateName = gatesNames[j];
            output += dict[gateName] ? 1L << j : 0L;
        }

        return output;
    }

    public void Solve2(string[] lines)
    {
        var initialGates = new Dictionary<string, bool>(
            lines.TakeWhile(l => !string.IsNullOrEmpty(l))
                .Select(l => new KeyValuePair<string, bool>(l[..3], l[5] == '1')));

        var x = GatesToLong(initialGates.Where(g => g.Key.StartsWith('x')));
        var y = GatesToLong(initialGates.Where(g => g.Key.StartsWith('y')));

        var originalGates = new List<string>(lines.SkipWhile(a => a.Length < 10));

        var isCorrect = CheckOutput(11, 13, initialGates.Count / 2, originalGates);
        
        // todo: idea - iterate from most significant bit and check if result is correct. if not, there is some gate that should be swapped. 
        // or: try to organise gates to see an pattern, then find gates that doesnt match pattern
    }
}
