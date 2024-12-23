// Day 22

namespace AdventOfCode.task._2024;

public class Task23 : ITask
{
    public void Solve(string[] lines)
    {
        var computers = new Dictionary<string, int>();
        var computersI = new Dictionary<int, string>();
        var cn = 0;
        foreach (var line in lines)
        {
            var c = line.Split("-");
            if (computers.TryAdd(c[0], cn))
            {
                computersI.Add(cn, c[0]);
                cn++;
            }

            if (computers.TryAdd(c[1], cn))
            {
                computersI.Add(cn, c[1]);
                cn++;
            }
        }

        var adj = new int[cn, cn];
        foreach (var line in lines)
        {
            var c = line.Split("-");
            adj[computers[c[0]], computers[c[1]]] = 1;
            adj[computers[c[1]], computers[c[0]]] = 1;
        }

        if (lines.Length < 50)
        {
            Console.Write("\n   ");
            foreach (var (cc, cci) in computers)
            {
                Console.Write(" " + cc);
            }

            Console.WriteLine();
            foreach (var (cr, cri) in computers)
            {
                Console.Write(cr);
                foreach (var (cc, cci) in computers)
                {
                    Console.Write("  " + adj[cri, cci]);
                }

                Console.WriteLine();
            }
        }

        var cycles = new HashSet<string>();

        foreach (var (c, idx) in computers)
        {
            if (!c.StartsWith('t'))
                continue;
            for (int i = 0; i < cn; i++)
            {
                if (adj[idx, i] == 1)
                {
                    for (int j = 0; j < cn; j++)
                    {
                        if (adj[i, j] == 1)
                        {
                            if (adj[idx, j] == 1)
                            {
                                var l = new List<string>
                                    { computersI[idx], computersI[i], computersI[j] };
                                l.Sort();
                                cycles.Add(string.Join('-', l));
                            }
                        }
                    }
                }
            }
        }

        Console.WriteLine(cycles.Count);
    }

    private Dictionary<int, string> _idxToComputer;

    public void Solve2(string[] lines)
    {
        var computers = new Dictionary<string, int>();
        _idxToComputer = new Dictionary<int, string>();
        var cn = 0;
        foreach (var line in lines)
        {
            var c = line.Split("-");
            if (computers.TryAdd(c[0], cn))
            {
                _idxToComputer.Add(cn, c[0]);
                cn++;
            }

            if (computers.TryAdd(c[1], cn))
            {
                _idxToComputer.Add(cn, c[1]);
                cn++;
            }
        }

        var adj = new int[cn, cn];
        foreach (var line in lines)
        {
            var c = line.Split("-");
            adj[computers[c[0]], computers[c[1]]] = 1;
            adj[computers[c[1]], computers[c[0]]] = 1;
        }

        _disconnectedComputers = new List<int>();
        var sets = new HashSet<string>();
        foreach (var (c, ci) in computers)
        {
            GetSets(new List<int> { ci }, adj, sets);
            Console.WriteLine("finished: " + c);
        }

        Console.WriteLine(sets.OrderByDescending(s => s.Length).First());

        // it is longer than 12
    }

    private int _maxLength = 0;

    private List<int> _disconnectedComputers;

    private void GetSets(List<int> current, int[,] adj, HashSet<string> sets)
    {
        for (int i = 0; i < adj.GetLength(0); i++)
        {
            if (current.Contains(i)) continue;
            if (adj[current[^1], i] == 0) continue;
            if (_disconnectedComputers.Contains(i)) continue;
            var newCurrent = new List<int>(current) { i };
            var isConnected = true;
            for (int j = 0; j < current.Count - 1; j++)
            {
                if (adj[newCurrent[j], newCurrent[^1]] == 0)
                {
                    isConnected = false;
                    break;
                }
            }

            if (isConnected)
            {
                if (newCurrent.Count > _maxLength)
                {
                    _maxLength = newCurrent.Count;
                    var l = newCurrent.Select(s => _idxToComputer[s]).ToList();
                    l.Sort();
                    sets.Add(string.Join(',', l));
                    Console.WriteLine("Set: " + string.Join(',', l));
                }

                GetSets(newCurrent, adj, sets);
            }
            else
            {
                //_disconnectedComputers.AddRange(current);
            }
        }
    }
}
