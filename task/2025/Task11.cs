namespace AdventOfCode.task._2025;

public class Task11 : ITask
{
    public void Solve(string[] lines)
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
    }
}
