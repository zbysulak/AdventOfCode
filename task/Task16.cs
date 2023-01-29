namespace AdventOfCode;

public class Task16 : ITask
{
    class Valve
    {
        public string Name { get; set; }
        public int Flow { get; set; }
        public List<string> Tunnels { get; set; }

        public Valve(string input)
        {
            // sample input: Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
            input = input.Replace("tunnel leads to valve", "tunnels lead to valves");
            Name = input.Substring(6, 2);
            var a = input.Substring("Valve AA has flow rate=".Length).Replace(" tunnels lead to valves ", "")
                .Split(";");
            Flow = int.Parse(a[0]);
            Tunnels = a[1].Split(", ").ToList();
        }
    }

    class State
    {
        public List<string> OpenedValves { get; set; }
        public string Position { get; set; }
        public int ReleasedPressure { get; set; }

        public State()
        {
            OpenedValves = new List<string>();
        }

        public override bool Equals(object? obj)
        {
            var other = obj as State;
            if (Position != other.Position) return false;
            if (ReleasedPressure != other.ReleasedPressure) return false;
            if (OpenedValves.Count != other.OpenedValves.Count ||
                OpenedValves.Any(ov => !other.OpenedValves.Contains(ov))) return false;
            return true;
        }
    }

    public void Solve(string[] lines)
    {
        var minutes = 30;
        var valves = lines.Select(l => new Valve(l)).ToDictionary(v => v.Name);
        var states = new List<State> { new() { Position = "AA" } };
        for (int min = 1; min <= minutes; min++)
        {
            Console.WriteLine(min + " " + states.Count);
            var newStates = new List<State>();
            foreach (var s in states)
            {
                // increase released pressure
                s.ReleasedPressure += valves.Values.Where(v => s.OpenedValves.Contains(v.Name)).Sum(v => v.Flow);

                // open current valve if it is not opened
                if (valves[s.Position].Flow > 0 && !s.OpenedValves.Contains(s.Position))
                {
                    var ov = s.OpenedValves;
                    ov.Add(s.Position);
                    newStates.Add(new State
                        { Position = s.Position, OpenedValves = ov, ReleasedPressure = s.ReleasedPressure });
                }

                // go to all possible tunnels
                foreach (var tunnel in valves[s.Position].Tunnels)
                {
                    newStates.Add(new State
                        { Position = tunnel, OpenedValves = s.OpenedValves, ReleasedPressure = s.ReleasedPressure });
                }
            }

            states = newStates.Distinct().ToList();
        }

        Console.WriteLine(states.Max(s => s.ReleasedPressure));
    }

    public void Solve2(string[] lines)
    {
    }
}