namespace AdventOfCode.task._2025;

public class Task11 : ITask
{
    public void Solve(string[] lines)
    {
        var paths = lines.Select(l => l.Split(": ")).ToDictionary(l => l[0], l => l[1].Split(" ").ToList());

        var queue = new Queue<List<string>>();
        var visited = new HashSet<string>();
        var initialState = "you";
        var possiblePaths = 0;
        queue.Enqueue(new List<string> { initialState });
        visited.Add(initialState);
        while (queue.Count > 0)
        {
            var path = queue.Dequeue();
            if (path.Last() == "out")
            {
                //Console.WriteLine(string.Join(',',path));
                possiblePaths++;
                continue;
            }

            foreach (var next in paths[path.Last()])
            {
                var nextPath = new List<string>(path) { next };
                var key = string.Join(',', nextPath);
                if (!visited.Contains(key))
                {
                    visited.Add(key);
                    queue.Enqueue(nextPath);
                }
            }
        }

        Console.WriteLine(possiblePaths);
    }

    private void PrintPaths(IDictionary<string, List<string>> paths)
    {
        foreach (var path in paths)
        {
            foreach (var next in path.Value)
            {
                Console.WriteLine($"{path.Key} {next}");
            }
        }
    }

    private Dictionary<string, int> _topologicalOrder;

    public void Solve2(string[] lines)
    {
        var paths = lines.Select(l => l.Split(": ")).ToDictionary(l => l[0], l => l[1].Split(" ").ToList());

/*
        var filteredPaths = new Dictionary<string, List<string>>();
        var skipped = new List<string>();
        foreach (var p in paths)
        {
            if (skipped.Contains(p.Key)) continue;
            var newDest = new List<string>(p.Value);
            foreach (var dest in p.Value)
            {
                if (dest is "out" or "fft" or "dac") continue;
                if (paths.TryGetValue(dest, out var to) && to.Count == 1 && to.Single() != "out" &&
                    !filteredPaths.Any(e => e.Value.Contains(dest)))
                {
                    newDest.Remove(dest);
                    newDest.Add(to.Single());
                    skipped.Add(dest);
                }
            }

            filteredPaths.Add(p.Key, newDest);
        }

        paths = filteredPaths;*/

        paths.Add("out", new List<string>());

        var L = new List<KeyValuePair<string, List<string>>>();
        var S = new List<KeyValuePair<string, List<string>>> { new("svr", paths["svr"]) };

        while (S.Count > 0)
        {
            var n = S.First();
            S = S.Skip(1).ToList();
            L.Add(n);
            foreach (var p in paths[n.Key])
            {
                // co vedou do current node 
                var a = paths.Where(e => e.Value.Contains(p));
                if (a.All(e => L.Select(s => s.Key).Contains(e.Key)))
                {
                    S.Add(new(p, paths[p]));
                }
            }
        }

        _topologicalOrder = L.Select((e, i) => new { e.Key, i }).ToDictionary(e => e.Key, e => e.i);

        Console.WriteLine($"\nOrder of chokepoints is: fft {_topologicalOrder["fft"]}, dac {_topologicalOrder["dac"]}");

        var first = _topologicalOrder["fft"] < _topologicalOrder["dac"] ? "fft" : "dac";
        var second = _topologicalOrder["fft"] > _topologicalOrder["dac"] ? "fft" : "dac";

        var startToFirst = FindPathsTo(paths, "svr", first);
        var firstToSecond = FindPathsTo(paths, first, second);
        var secondToEnd = FindPathsTo(paths, second, "out");

        Console.WriteLine(startToFirst * firstToSecond * secondToEnd);
    }

    private long FindPathsTo(IDictionary<string, List<string>> paths, string start, string goal)
    {
        var queue = new Queue<string>();
        var possiblePaths = 0;
        queue.Enqueue(start);
        while (queue.Count > 0)
        {
            var currentPath = queue.Dequeue();
            if (currentPath == goal)
            {
                possiblePaths++;
                continue;
            }

            if (!paths.TryGetValue(currentPath, out var nextPaths)) continue;
            foreach (var next in nextPaths)
            {
                if (currentPath.Contains(next)) continue;
                if (_topologicalOrder[goal] < _topologicalOrder[next])
                    continue;
                queue.Enqueue(next);
            }
        }

        return possiblePaths;
    }
}