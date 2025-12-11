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
        var sw = System.Diagnostics.Stopwatch.StartNew();
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
        Console.WriteLine(sw.ElapsedMilliseconds);
    }

    private int BFS2(int[] goal, int[][] buttons)
    {
        var buttonDist = new Dictionary<int, int>();
        foreach (var button in buttons)
        {
            foreach (var lights in button)
            {
                if (!buttonDist.ContainsKey(lights))
                {
                    buttonDist[lights] = 0;
                }

                buttonDist[lights]++;
            }
        }

        var presses = new int[buttons.Length];
        var state = new int[goal.Length];

        foreach (var pair in buttonDist.Where(k => k.Value == 1))
        {
            var light = pair.Key;
            for (var i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].Contains(light))
                {
                    presses[i] = goal[light];
                    foreach (var l in buttons[i])
                    {
                        state[l] = goal[light];
                    }

                    break;
                }
            }
        }

        var acc = new List<int[]>();
        Recursive(goal, buttons, 0, state, presses, acc);
        return acc.Min(e => e.Sum());
    }

    private void Recursive(int[] goal, int[][] buttons, int btnIdx, int[] state, int[] presses, List<int[]> acc)
    {
        if (btnIdx == buttons.Length)
        {
            if (state.SequenceEqual(goal))
            {
                acc.Add(presses);
            }

            return;
        }

        if (presses[btnIdx] != 0)
        {
            Recursive(goal, buttons, btnIdx + 1, state, presses, acc);
            return;
        }

        var maxPresses = buttons[btnIdx].Min(e => goal[e] - state[e]);
        for (var i = 0; i <= maxPresses; i++)
        {
            var newState = (int[])state.Clone();
            var newPresses = (int[])presses.Clone();
            newPresses[btnIdx] = i;
            foreach (var light in buttons[btnIdx])
            {
                newState[light] += i;
            }
            
            //todo check if goal is reachable from current state

            Recursive(goal, buttons, btnIdx + 1, newState, newPresses, acc);
        }
    }
}
