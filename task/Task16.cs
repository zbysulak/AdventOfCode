namespace AdventOfCode;

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
        var distMatrix = CreateDistanceMatrix(valves, nameToIdx);

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
                Console.Write($"{valves[i].Name}({valves[i].Flow})\t");
            }

            Console.WriteLine();
            for (int i = 0; i < distMatrix.GetLength(0); i++)
            {
                Console.Write($"{valves[i].Name}\t");
                for (int j = 0; j < distMatrix.GetLength(1); j++)
                {
                    Console.Write($"{distMatrix[i, j]}\t");
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
        public record Target
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
        const int timeLimit = 26;
        var valves = lines.Select((l, i) => new Valve(l, i)).ToArray();
        var nameToIdx = valves.ToDictionary<Valve, string, int>(v => v.Name, v => v.Index);
        var distMatrix = CreateDistanceMatrix(valves, nameToIdx);

        var nonZeroValves = valves.Where(v => v.Flow > 0).Select(v => v.Name).ToList();

        List<(string me, string elephant)> GetPairs(State2 s)
        {
            var pairs = new List<(string, string)>();
            var unopened = nonZeroValves
                .Where(v => !s.OpenedValves.Contains(v) && v != s.MyTarget.Name && v != s.ElephantTarget.Name)
                .ToArray();
            for (int i = 0; i < unopened.Length; i++)
            {
                for (int j = 0; j < unopened.Length; j++)
                {
                    if (i == j) continue;
                    if (distMatrix[nameToIdx[unopened[i]], nameToIdx[s.MyTarget.Name]] + s.Minute >= timeLimit)
                        continue;
                    if (distMatrix[nameToIdx[unopened[j]], nameToIdx[s.ElephantTarget.Name]] + s.Minute >= timeLimit)
                        continue;
                    pairs.Add((unopened[i], unopened[j]));
                }
            }

            return pairs;
        }

        var states = new List<State2>
        {
            new()
            {
                MyTarget = new State2.Target { Name = StartingValve },
                ElephantTarget = new State2.Target { Name = StartingValve }
            }
        };
        var finalStates = new List<State2>();

        while (states.Any())
        {
            var newStates = new List<State2>();
            foreach (var state in states)
            {
                if (state.MyTarget.Distance == 0 && state.ElephantTarget.Distance == 0)
                {
                    var ov = new List<string>(state.OpenedValves);
                    if (state.MyTarget.Name != StartingValve) // open valves..
                    {
                        ov.Add(state.MyTarget.Name);
                        ov.Add(state.ElephantTarget.Name);
                    }

                    // find new targets
                    var pairs = GetPairs(state);
                    if (pairs.Count > 0)
                    {
                        foreach (var pair in pairs)
                        {
                            var myDist = distMatrix[nameToIdx[state.MyTarget.Name], nameToIdx[pair.me]];
                            var elDist = distMatrix[nameToIdx[state.ElephantTarget.Name], nameToIdx[pair.elephant]];
                            var nextStep = Math.Min(myDist, elDist);
                            var pressure = state.ReleasedPressure +
                                           (nextStep + 1) *
                                           ov.Sum(v => valves[nameToIdx[v]].Flow); // +1 for opening valve...
                            newStates.Add(new State2
                            {
                                OpenedValves = ov,
                                Previous = state,
                                Minute = state.Minute + nextStep + 1,
                                ReleasedPressure = pressure,
                                MyTarget = new State2.Target
                                {
                                    Distance = myDist - nextStep,
                                    Name = pair.me
                                },
                                ElephantTarget = new State2.Target
                                {
                                    Distance = elDist - nextStep,
                                    Name = pair.elephant
                                }
                            });
                            //break;
                        }
                    }
                    else
                    {
                        // no pairs left, just open valves and run to completion
                        finalStates.Add(new State2
                        {
                            Previous = state,
                            Minute = timeLimit,
                            ElephantTarget = state.ElephantTarget with { Distance = 0 },
                            MyTarget = state.MyTarget with { Distance = 0 },
                            OpenedValves = ov,
                            ReleasedPressure = state.ReleasedPressure +
                                               (timeLimit - state.Minute) * ov.Sum(v => valves[nameToIdx[v]].Flow)
                        });
                    }
                }
                else
                {
                    // in this case one of me and elephant has to be at its destination
                    var me = state.MyTarget.Distance == 0;
                    if (me == (state.ElephantTarget.Distance == 0)) throw new Exception("wtf");
                    var ov = new List<string>(state.OpenedValves)
                        { me ? state.MyTarget.Name : state.ElephantTarget.Name };

                    var unopenedValves = nonZeroValves.Where(v =>
                        (me ? state.ElephantTarget.Name : state.MyTarget.Name) != v && !ov.Contains(v) &&
                        distMatrix[nameToIdx[me ? state.MyTarget.Name : state.ElephantTarget.Name], nameToIdx[v]] +
                        state.Minute <= timeLimit);
                    if (unopenedValves.Any())
                    {
                        foreach (var valve in unopenedValves)
                        {
                            var dist = distMatrix[nameToIdx[(me ? state.MyTarget.Name : state.ElephantTarget.Name)],
                                nameToIdx[valve]];
                            var nextStep = Math.Min(dist,
                                (me ? state.ElephantTarget.Distance : state.MyTarget.Distance) - 1);
                            newStates.Add(new State2
                            {
                                OpenedValves = ov,
                                Previous = state,
                                Minute = state.Minute + nextStep + 1,
                                ReleasedPressure = state.ReleasedPressure +
                                                   (nextStep + 1) * ov.Sum(v => valves[nameToIdx[v]].Flow),
                                MyTarget = new State2.Target
                                {
                                    Distance = (me ? dist : state.MyTarget.Distance - 1) - nextStep,
                                    Name = me ? valve : state.MyTarget.Name
                                },
                                ElephantTarget = new State2.Target
                                {
                                    Distance = (me ? state.ElephantTarget.Distance - 1 : dist) - nextStep,
                                    Name = me ? state.ElephantTarget.Name : valve
                                }
                            });
                            //break;
                        }
                    }
                    else
                    {
                        // no valves left, just open valves and run to completion
                        var ov2 = new List<string>(ov) { me ? state.ElephantTarget.Name : state.MyTarget.Name };
                        var timeToLastValve = me ? state.ElephantTarget.Distance : state.MyTarget.Distance;
                        var pressureBeforeLast = state.ReleasedPressure +
                                                 (timeToLastValve) * ov.Sum(v => valves[nameToIdx[v]].Flow);
                        var pressureAtTheEnd = pressureBeforeLast + (timeLimit - timeToLastValve - state.Minute) *
                            ov2.Sum(v => valves[nameToIdx[v]].Flow);
                        var sss = new State2
                        {
                            Previous = state,
                            Minute = state.Minute + timeToLastValve,
                            ElephantTarget = state.ElephantTarget with { Distance = 0 },
                            MyTarget = state.MyTarget with { Distance = 0 },
                            OpenedValves = ov2,
                            ReleasedPressure = pressureAtTheEnd
                        };
                        finalStates.Add(sss);
                    }
                }

                //break;
            }

            Console.WriteLine(newStates.Count);
            states = newStates;
        }

        /*foreach (var state in closedStates)
        {
            // I have to compute pressure released in remaining time
            var p = state.ReleasedPressure +
                    (timeLimit - state.Minute) * state.OpenedValves.Sum(v => valves[nameToIdx[v]].Flow);
            state.ReleasedPressure = p;
        }*/

        var topState = finalStates.First();
        var topPressure = topState.ReleasedPressure;
        foreach (var state in finalStates)
        {
            if (topPressure < state.ReleasedPressure)
            {
                topPressure = state.ReleasedPressure;
                topState = state;
            }
        }

        var c = topState;

        do
        {
            Console.WriteLine(
                $"#{c.Id}, min: {c.Minute}, me->{c.MyTarget.Name}({c.MyTarget.Distance}), el->{c.ElephantTarget.Name}({c.ElephantTarget.Distance}), press: {c.ReleasedPressure} " +
                string.Join(',', c.OpenedValves.Select(s => $"{s}({valves[nameToIdx[s]].Flow})")));
            c = c.Previous;
        } while (c is not null);

        Console.WriteLine(
            $"best possible outcome is {topState.ReleasedPressure} with opened valves {string.Join(',', topState.OpenedValves)}");
    }
}