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

    public void Solve2(string[] lines)
    {
        var paths = lines.Select(l => l.Split(": ")).ToDictionary(l => l[0], l => l[1].Split(" ").ToList());
        var possiblePaths = 0;

        var pathsWithDac = FindPathsTo(paths, new List<string> { "svr" }, "dac");
        var pathsWithFftDac = pathsWithDac.Where(e => e.Contains("fft")).ToList();
/*
        var queue = new Queue<List<string>>();
        var visited = new HashSet<string>();
        var initialState = "svr";
        queue.Enqueue(new List<string> { initialState });
        visited.Add(initialState);
        while (queue.Count > 0)
        {
            var path = queue.Dequeue();
            if (path.Last() == "out")
            {
                //Console.WriteLine(string.Join(',',path));
                if (path.Contains("fft") && path.Contains("dac"))
                    possiblePaths++;
                continue;
            }

            foreach (var next in paths[path.Last()])
            {
                var nextPath = new List<string>(path) { next };
                var key = string.Join(',', nextPath.OrderBy(x => x));
                if (!visited.Contains(key))
                {
                    visited.Add(key);
                    queue.Enqueue(nextPath);
                }
            }
        }*/

        Console.WriteLine(possiblePaths);
    }

    private IList<IList<string>> FindPathsTo(IDictionary<string, List<string>> paths, IList<string> path,
        string goal)
    {
        var queue = new Queue<IList<string>>();
        var visited = new HashSet<string>();
        var possiblePaths = new List<IList<string>>();
        queue.Enqueue(path);
        visited.Add(string.Join(',', path.OrderBy(x => x)));
        while (queue.Count > 0)
        {
            var currentPath = queue.Dequeue();
            if (currentPath.Last() == goal)
            {
                //Console.WriteLine(string.Join(',',path));
                //if (currentPath.Contains("fft") && currentPath.Contains("dac"))
                possiblePaths.Add(currentPath);
                continue;
            }

            if (!paths.TryGetValue(currentPath.Last(), out var nextPaths)) continue;
            foreach (var next in nextPaths)
            {
                var nextPath = new List<string>(currentPath) { next };
                var key = string.Join(',', nextPath.OrderBy(x => x));
                if (!visited.Contains(key))
                {
                    visited.Add(key);
                    queue.Enqueue(nextPath);
                }
            }
        }

        return possiblePaths;
    }
}
