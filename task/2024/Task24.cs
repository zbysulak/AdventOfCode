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

    private long GetOutput(long x, long y, int bits, List<string> gates)
    {
        var g = new Dictionary<string, bool>();
        GetInitialGates(x, 'x', bits, g);
        GetInitialGates(y, 'y', bits, g);

        var gatesCopy = new List<string>(gates);

        while (gatesCopy.Any())
        {
            var toRemove = new List<string>();
            foreach (var gate in gatesCopy)
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
                gatesCopy.Remove(gtr);
            }
        }

        var outputValue = GatesToLong(g.Where(gate => gate.Key.StartsWith("z")));

        return outputValue;
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

        var outputInt = GetOutput(11, 13, initialGates.Count / 2, originalGates);

        var outputBin = Convert.ToString(outputInt, 2);

        Console.WriteLine(originalGates.Count(g => g[4] == 'A') + " AND");
        Console.WriteLine(originalGates.Count(g => g[4] == 'O') + " OR");
        Console.WriteLine(originalGates.Count(g => g[4] == 'X') + " XOR");

        var solved = new HashSet<string>();

        var pairsToSwap = new List<(string, string)>
        {
            ("y37 AND x37 -> z37", "gcg XOR nbm -> rrn"),
            ("tnn OR bss -> z16", "kcm XOR grr -> fkb"),
            ("y21 AND x21 -> rqf", "x21 XOR y21 -> nnr"),
            ("qsj AND tjk -> z31", "qsj XOR tjk -> rdn")
        };

        foreach (var (a, b) in pairsToSwap)
        {
            var g1o = a.Split(" ")[4];
            var g2o = b.Split(" ")[4];
            originalGates.Remove(a);
            originalGates.Remove(b);
            originalGates.Add(a.Substring(0, a.Length - 3) + g2o);
            originalGates.Add(b.Substring(0, b.Length - 3) + g1o);
        }

        var suspicious = new List<int> { 32, 33, 31 };
        for (int i = 0; i <= 45; i++)
        {
            var g = "z" + i.ToString("D2");
            if (suspicious.Contains(i))
                Console.WriteLine("gate " + g + "----------------");
            GetPathForGate(g, originalGates, solved, 0, suspicious.Contains(i));
        }
        
        var swappedOutputs = pairsToSwap.SelectMany(s=>new string[]{s.Item1.Split(" ")[4], s.Item2.Split(" ")[4]}).ToList();
        swappedOutputs.Sort();
        Console.WriteLine(string.Join(",", swappedOutputs));
    }

    private void GetPathForGate(string gate, List<string> gates, HashSet<string> solved, int lvl, bool print)
    {
        if (solved.Contains(gate))
            return;
        var cg = gates.Single(g => g.Substring(g.Length - 3, 3) == gate);
        if (print)
        {
            for (int i = 0; i < lvl; i++)
            {
                Console.Write(" ");
            }

            Console.WriteLine(string.Join(", ", cg) + "  ");
        }

        var split = cg.Split(" ");

        if (!split[0].StartsWith("x") && !split[0].StartsWith("y"))
            GetPathForGate(split[0], gates, solved, lvl + 1, print);
        if (!split[2].StartsWith("x") && !split[2].StartsWith("y"))
            GetPathForGate(split[2], gates, solved, lvl + 1, print);

        solved.Add(split[4]);
    }
}
