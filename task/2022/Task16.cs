namespace AdventOfCode.task._2022;

public class Task16 : ITask
{
    private const string StartingValve = "AA";

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
        var distMatrix = CreateDistanceMatrix(valves);

        var states = new List<State> { new() { Position = StartingValve } };
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
            Console.WriteLine(string.Join(',', s.OpenedValves) + " " + s.Minute+ " p: "+s.ReleasedPressure);
        }

        Console.WriteLine(topPressure);
    }

    private static short[,] CreateDistanceMatrix(IReadOnlyList<Valve> valves)
    {
        var distMatrix = new short[valves.Count, valves.Count];
        var print = () =>
        {
            var sep = true ? "\t" : ",";
            Console.Write(sep);
            for (int i = 0; i < valves.Count; i++)
            {
                Console.Write($"{valves[i].Name}({valves[i].Flow}){sep}");
            }

            Console.WriteLine();
            for (int i = 0; i < distMatrix.GetLength(0); i++)
            {
                Console.Write($"{valves[i].Name}{sep}");
                for (int j = 0; j < distMatrix.GetLength(1); j++)
                {
                    Console.Write($"{distMatrix[i, j]}{sep}");
                }

                Console.WriteLine();
            }
        };

        for (int i = 0; i < valves.Count; i++)
        {
            for (int j = 0; j < valves.Count; j++)
            {
                if (i == j)
                    distMatrix[i, j] = 0;
                else if (valves[i].Tunnels.Contains(valves[j].Name))
                    distMatrix[i, j] = 1;
                else
                    distMatrix[i, j] = short.MaxValue / 3;
            }
        }

        for (int k = 0; k < distMatrix.GetLength(0); k++)
        {
            for (int i = 0; i < distMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < distMatrix.GetLength(0); j++)
                {
                    distMatrix[i, j] = Math.Min(distMatrix[i, j], (short)(distMatrix[i, k] + distMatrix[k, j]));
                }
            }
        }

        //print();
        return distMatrix;
    }

    public void Solve2(string[] lines)
    {
        const int timeLimit = 26;
        var valves = lines.Select((l, i) => new Valve(l, i)).ToArray();
        var nameToIdx = valves.ToDictionary<Valve, string, int>(v => v.Name, v => v.Index);
        var distMatrix = CreateDistanceMatrix(valves);

        var nonZeroValves = valves.Where(v => v.Flow > 0).ToArray();

        var pairs = GetPairs(nonZeroValves);

        var topScore = 0;
        foreach (var pair in pairs)
        {
            var a = (FindMaxFlow(timeLimit, pair.human.ToArray(), nameToIdx, distMatrix),
                FindMaxFlow(timeLimit, pair.elephant.ToArray(), nameToIdx, distMatrix));

            if (a.Item1.ReleasedPressure + a.Item2.ReleasedPressure > topScore)
            {
                topScore = a.Item1.ReleasedPressure + a.Item2.ReleasedPressure;
            }
        }
        // Console.WriteLine(string.Join(',', topPair.Item1.Select(v => v.Name)) + " a " +
        //                   string.Join(',', topPair.Item2.Select(v => v.Name)));
        // Console.WriteLine(string.Join(',', topStates.Item1.OpenedValves) + $" ({topStates.Item1.ReleasedPressure}) a " +
        //                   string.Join(',', topStates.Item2.OpenedValves) + $" ({topStates.Item2.ReleasedPressure})");

        Console.WriteLine(topScore);
    }

    private static State FindMaxFlow(int timeLimit, Valve[] valves, IReadOnlyDictionary<string, int> nameToIdx,
        short[,] distances)
    {
        var states = new List<State> { new() { Position = StartingValve } };
        var closedStates = new List<State>();

        while (states.Any())
        {
            var newStates = new List<State>();
            foreach (var state in states)
            {
                // go to all unopened valves with nonzero flow and open it. 
                foreach (var valve in valves.Where(v => !state.OpenedValves.Contains(v.Name)))
                {
                    var duration =
                        distances[valve.Index,
                            nameToIdx[state.Position]]; // time needed to go to another valve
                    if (state.Minute + duration + 1 > timeLimit) continue; // it won't add any pressure 
                    // current pressure + new pressure released during transfer
                    // it is duration + 1 because opened valves are also releasing pressure during opening new one
                    var pressure = state.ReleasedPressure +
                                   state.OpenedValves.Sum(ov => valves.Single(v => v.Name == ov).Flow) * (duration + 1);
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
                    (timeLimit - state.Minute) * state.OpenedValves.Sum(v => valves.Single(vv => vv.Name == v).Flow);
            if (p <= topPressure) continue;
            topPressure = p;
            state.ReleasedPressure = topPressure;
            state.Minute = timeLimit;
            topState = state;
        }

        return topState;
    }

     private static IEnumerable<(HashSet<Valve> human, HashSet<Valve> elephant)> GetPairs(IReadOnlyList<Valve> valves)
    {
        var maxMask = 1 << valves.Count;

        for (var mask = 0; mask < maxMask; mask++)
        {
            var elephant = new HashSet<Valve>();
            var human = new HashSet<Valve>();
            
            for (var i = 0; i < valves.Count; i++)
            {
                if ((mask & (1 << i)) == 0)
                {
                    elephant.Add(valves[i]);
                }
                else
                {
                    human.Add(valves[i]);
                }
            }

            yield return (human, elephant);
        }
    }
}
