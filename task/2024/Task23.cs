// Day 23: LAN party

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

        var sets = new HashSet<string>();
        BronKerbosch(new int[0], _idxToComputer.Keys.ToArray(), new int[0], adj, sets);

        Console.WriteLine(sets.OrderByDescending(s => s.Length).First());
    }

    /// <summary>
    /// Bron - Kerbosch algorithm to find all maximal cliques in a graph
    /// </summary>
    /// <param name="R">Current clique</param>
    /// <param name="P">Potential nodes</param>
    /// <param name="X">Nodes that have been tried and should not be considered anymore</param>
    /// <param name="adj"></param>
    /// <param name="sets"></param>
    private void BronKerbosch(int[] R, int[] P, int[] X, int[,] adj, HashSet<string> sets)
    {
        if (P.Length == 0 && X.Length == 0)
        {
            var l = new List<string>();
            foreach (var r in R)
            {
                l.Add(_idxToComputer[r]);
            }

            l.Sort();
            sets.Add(string.Join(',', l));
            return;
        }

        foreach (var v in P)
        {
            var newR = R.Append(v).ToArray();
            var newP = P.Where(p => adj[v, p] == 1).ToArray();
            var newX = X.Where(x => adj[v, x] == 1).ToArray();
            BronKerbosch(newR, newP, newX, adj, sets);
            P = P.Where(p => p != v).ToArray();
            X = X.Append(v).ToArray();
        }
    }
}
