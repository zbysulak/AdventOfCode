namespace AdventOfCode;

public class Task16 : ITask
{
    private class Valve
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public int Flow { get; set; }
        public List<string> Tunnels { get; set; }

        public Valve(string input, int index)
        {
            // sample input: Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
            Index = index;
            input = input.Replace("tunnel leads to valve", "tunnels lead to valves");
            Name = input.Substring(6, 2);
            var a = input.Substring("Valve AA has flow rate=".Length).Replace(" tunnels lead to valves ", "")
                .Split(";");
            Flow = int.Parse(a[0]);
            Tunnels = a[1].Split(", ").ToList();
        }
    }

    private class State
    {
        /// <summary>
        /// Minute that just started
        /// </summary>
        public int Minute { get; set; }

        public List<string> OpenedValves { get; set; }
        public string Position { get; set; }
        public int ReleasedPressure { get; set; }
        public State? Previous { get; set; } // for debugging purposes
        public int Id { get; set; } // for debugging purposes

        private static int _i = 0;

        public State()
        {
            Id = _i++;
            OpenedValves = new List<string>();
        }
    }

    public void Solve(string[] lines)
    {
        const int timeLimit = 30;
        var valves = lines.Select((l, i) => new Valve(l, i)).ToArray();
        var nameToIdx = valves.ToDictionary<Valve, string, int>(v => v.Name, v => v.Index);
        var distMatrix = CreateDistanceMatrix(valves, nameToIdx);

        var states = new List<State> { new() { Position = "AA" } };
        var closedStates = new List<State>();

        while (states.Any())
        {
            var newStates = new List<State>();
            foreach (var state in states)
            {
                // go to all unopened valves with nonzero flow and open it. 
                foreach (var valve in valves.Where(v => !state.OpenedValves.Contains(v.Name) && v.Flow > 0))
                {
                    var duration =
                        distMatrix[valve.Index,
                            nameToIdx[state.Position]]; // time needed to go to another valve
                    if (state.Minute + duration + 1 >= timeLimit) continue; // it won't add any pressure 
                    // current pressure + new pressure released during transfer
                    // it is duration + 1 because opened valves are also releasing pressure during opening new one
                    var pressure = state.ReleasedPressure +
                                   state.OpenedValves.Sum(ov => valves[nameToIdx[ov]].Flow) * (duration + 1);
                    var ov = new List<string>(state.OpenedValves) { valve.Name }; // new set of opened valves
                    newStates.Add(new State
                    {
                        Position = valve.Name,
                        OpenedValves = ov,
                        ReleasedPressure = pressure,
                        Minute = state.Minute + duration + 1, // current + transfer + valve opening
                        Previous = state
                    });
                }
            }

            closedStates.AddRange(states);
            states = newStates;
        }

        var topState = closedStates.First();
        var topPressure = 0;
        foreach (var state in closedStates.Skip(1))
        {
            // I have to compute pressure released in remaining time
            var p = state.ReleasedPressure +
                    (timeLimit - state.Minute) * state.OpenedValves.Sum(v => valves[nameToIdx[v]].Flow);
            if (p > topPressure)
            {
                topPressure = p;
                topState = state;
            }
        }

        var c = topState;
        var backtrack = new List<State>();
        while (c.Previous is not null)
        {
            backtrack.Add(c);
            c = c.Previous;
        }

        backtrack.Reverse();

        foreach (var s in backtrack)
        {
            Console.WriteLine(string.Join(',', s.OpenedValves) + " " + s.Minute);
        }

        Console.WriteLine(topPressure);
    }

    private static short[,] CreateDistanceMatrix(IReadOnlyList<Valve> valves,
        IReadOnlyDictionary<string, int> nameToIdx)
    {
        var distMatrix = new short[valves.Count, valves.Count];
        var print = () =>
        {
            Console.Write("\t");
            for (int i = 0; i < valves.Count; i++)
            {
                Console.Write($"\t{valves[i].Name}({valves[i].Flow})");
            }

            Console.WriteLine();
            for (int i = 0; i < distMatrix.GetLength(0); i++)
            {
                Console.Write($"\t{valves[i].Name}");
                for (int j = 0; j < distMatrix.GetLength(1); j++)
                {
                    Console.Write($"\t{distMatrix[i, j]}");
                }

                Console.WriteLine();
            }
        };

        for (int i = 0; i < valves.Count; i++)
        {
            for (int j = 0; j < valves.Count; j++)
            {
                distMatrix[i, j] = short.MaxValue;
            }
        }

        for (int i = 0; i < valves.Count; i++)
        {
            distMatrix[i, i] = 0;
        }

        foreach (var valve in valves)
        {
            var f = valve.Index;
            foreach (var tunnel in valve.Tunnels)
            {
                var t = valves[nameToIdx[tunnel]].Index;
                distMatrix[f, t] = 1;
                distMatrix[t, f] = 1;
            }
        }

        for (int i = 0; i < distMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < distMatrix.GetLength(0); j++)
            {
                for (int k = 0; k < distMatrix.GetLength(0); k++)
                {
                    if (distMatrix[i, j] > distMatrix[i, k] + distMatrix[k, j])
                    {
                        distMatrix[i, j] = (short)(distMatrix[i, k] + distMatrix[k, j]);
                    }
                }
            }
        }

        //print();
        return distMatrix;
    }

    private class State2
    {
        public class Target
        {
            public string Name { get; set; }
            public int Distance { get; set; }
        }

        /// <summary>
        /// Minute that just started
        /// </summary>
        public int Minute { get; set; }

        public List<string> OpenedValves { get; set; }
        public Target MyTarget { get; set; }
        public Target ElephantTarget { get; set; }
        public int ReleasedPressure { get; set; }
        public State2? Previous { get; set; } // for debugging purposes
        public int Id { get; set; } // for debugging purposes

        private static int _i = 0;

        public State2()
        {
            Id = _i++;
            OpenedValves = new List<string>();
        }
    }

    public void Solve2(string[] lines)
    {
    }
}