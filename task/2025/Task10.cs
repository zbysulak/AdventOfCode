namespace AdventOfCode.task._2025;

public class Task10 : ITask
{
    public void Solve(string[] lines)
    {
        var presses = new List<int>();
        foreach (var line in lines)
        {
            var s = line.Split("] ");
            var goal = s[0].TrimStart('[').ToCharArray().Select(c => c == '#').ToArray();
            var buttons = s[1]
                .Split(" {")[0]
                .Split(" ")
                .Select(b => b.TrimStart('(')
                    .TrimEnd(')')
                    .Split(',')
                    .Select(int.Parse)
                    .ToArray())
                .ToArray();
            presses.Add(BFS(goal, buttons));
        }

        Console.WriteLine(presses.Sum());
    }

    private int BFS(bool[] goal, int[][] buttons)
    {
        var queue = new Queue<(bool[] state, List<int> presses)>();
        var visited = new HashSet<string>();
        var initialState = new bool[goal.Length];
        var possiblePresses = new List<List<int>>();
        queue.Enqueue((initialState, new List<int>()));
        visited.Add(string.Join(',', initialState));
        while (queue.Count > 0)
        {
            var (state, presses) = queue.Dequeue();
            if (state.SequenceEqual(goal))
            {
                possiblePresses.Add(presses);
                continue;
            }

            for (var i = 0; i < buttons.Length; i++)
            {
                var button = buttons[i];
                var newState = (bool[])state.Clone();
                foreach (var index in button)
                {
                    newState[index] = !newState[index];
                }

                var stateKey = string.Join(',', newState);
                if (!visited.Contains(stateKey))
                {
                    visited.Add(stateKey);
                    var newPresses = new List<int>(presses) { i };
                    queue.Enqueue((newState, newPresses));
                }
            }
        }

        return possiblePresses.Min(p => p.Count);
    }

    public void Solve2(string[] lines)
    {
        var presses = new List<int>();
        foreach (var line in lines)
        {
            var s = line.Split("] ")[1].Split(" {");
            var goal = s[1].TrimEnd('}').Split(',').Select(int.Parse).ToArray();
            var buttons = s[0]
                .Split(" ")
                .Select(b => b.TrimStart('(')
                    .TrimEnd(')')
                    .Split(',')
                    .Select(int.Parse)
                    .ToArray())
                .ToArray();
            presses.Add(BFS2(goal, buttons));
        }

        Console.WriteLine(presses.Sum());
    }

    private int BFS2(int[] goal, int[][] buttons)
    {
        var queue = new Queue<(int[] state, List<int> presses)>();
        var visited = new HashSet<string>();
        var initialState = new int[goal.Length];
        var possiblePresses = new List<List<int>>();
        queue.Enqueue((initialState, new List<int>()));
        visited.Add(string.Join(',', initialState));
        while (queue.Count > 0)
        {
            var (state, presses) = queue.Dequeue();
            if (state.SequenceEqual(goal))
            {
                possiblePresses.Add(presses);
                continue;
            }

            var invalidState = false;
            for (int i = 0; i < goal.Length; i++)
            {
                if (goal[i] < state[i])
                    invalidState = true;
            }

            if (invalidState)
                continue;

            for (var i = 0; i < buttons.Length; i++)
            {
                var button = buttons[i];
                var newState = (int[])state.Clone();
                foreach (var index in button)
                {
                    newState[index]++;
                }

                var stateKey = string.Join(',', newState);
                if (!visited.Contains(stateKey))
                {
                    visited.Add(stateKey);
                    var newPresses = new List<int>(presses) { i };
                    queue.Enqueue((newState, newPresses));
                }
            }
        }

        return possiblePresses.Min(p => p.Count);
    }
}
