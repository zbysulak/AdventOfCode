// Day 21

namespace AdventOfCode.task._2024;

public class Task21 : ITask
{
    public class KeyPad
    {
        private Dictionary<(char from, char to), List<string>> _lookup;
        protected Dictionary<char, (int x, int y)> Keys;
        private Dictionary<string, List<string>> _sequences = new();

        public Dictionary<(char from, char to), List<string>> Lookup
        {
            get
            {
                if (_lookup == null)
                    InitializeLookup();
                return _lookup;
            }
        }

        #region Initialization

        private void InitializeLookup()
        {
            _lookup = new Dictionary<(char from, char to), List<string>>();
            foreach (var from in Keys.Keys)
            {
                foreach (var to in Keys.Keys)
                {
                    _lookup[(from, to)] = InitSeq(from, to);
                }
            }
        }

        private List<string> InitSeq(char from, char to)
        {
            if (from == to) return new List<string> { "A" };
            var pos = Keys[from];
            var subSequence = new List<char>();
            var key = Keys[to];

            if (pos.y < key.y)
            {
                subSequence.AddRange(new string('v', key.y - pos.y));
                pos.y = key.y;
            }

            if (pos.y > key.y)
            {
                subSequence.AddRange(new string('^', -key.y + pos.y));
                pos.y = key.y;
            }

            if (pos.x < key.x)
            {
                subSequence.AddRange(new string('>', key.x - pos.x));
                pos.x = key.x;
            }

            if (pos.x > key.x)
            {
                subSequence.AddRange(new string('<', -key.x + pos.x));
                pos.x = key.x;
            }

            var combinations = new HashSet<string>();
            GetCombinations(string.Join("", subSequence), "", combinations);
            return combinations.Where(c => IsValid(c, from)).Select(e => new string(e) + 'A').ToList();
        }

        private bool IsValid(string sequence, char from)
        {
            var pos = Keys[from];
            foreach (var d in sequence)
            {
                var dir = d switch
                {
                    '^' => (0, -1),
                    'v' => (0, 1),
                    '<' => (-1, 0),
                    '>' => (1, 0),
                    _ => throw new Exception("Invalid direction")
                };
                pos = (pos.x + dir.Item1, pos.y + dir.Item2);
                if (!Keys.ContainsValue(pos))
                    return false;
            }

            return true;
        }

        private static void GetCombinations(string seq, string current, HashSet<string> result)
        {
            if (string.IsNullOrEmpty(seq))
            {
                result.Add(current);
            }

            for (int i = 0; i < seq.Length; i++)
            {
                var newCombination = current + seq[i];
                var remaining = seq.Substring(0, i) + seq.Substring(i + 1);
                GetCombinations(remaining, newCombination, result);
            }
        }

        #endregion

        public List<string> GetShortestSequences(string buttons)
        {
            if (_lookup == null)
                InitializeLookup();

            var canSkip = 0;
            while (_sequences.ContainsKey(buttons[..(canSkip + 1)]))
            {
                canSkip++;
            }

            var sequences = new List<string> { "" };
            var current = 'A';

            for (int i = canSkip; i < buttons.Length; i++)
            {
                var button = buttons[i];
                var newSequences = new List<string>();
                foreach (var s in sequences)
                {
                    newSequences.AddRange(_lookup[(current, button)].Select(ss => s + ss).ToList());
                }

                current = button;
                sequences = newSequences;
            }

            return sequences;
        }
    }

    public class NumericKeyPad : KeyPad
    {
        public NumericKeyPad()
        {
            Keys = new()
            {
                { '7', (0, 0) }, { '8', (1, 0) }, { '9', (2, 0) },
                { '4', (0, 1) }, { '5', (1, 1) }, { '6', (2, 1) },
                { '1', (0, 2) }, { '2', (1, 2) }, { '3', (2, 2) },
                { '0', (1, 3) }, { 'A', (2, 3) }
            };
        }
    }

    public class DirectionalKeyPad : KeyPad
    {
        public DirectionalKeyPad()
        {
            Keys = new()
            {
                { '^', (1, 0) }, { 'A', (2, 0) },
                { '<', (0, 1) }, { 'v', (1, 1) }, { '>', (2, 1) }
            };
        }
    }

    public void Solve(string[] lines)
    {
        var totalComplexity = 0;
        foreach (var line in lines)
        {
            var numPart = int.Parse(line.Substring(0, line.Length - 1));
            var nkp = new NumericKeyPad();
            var dkp = new DirectionalKeyPad();
            var keysToPress = nkp.GetShortestSequences(line);

            var keysToPress2 = keysToPress.Select(k => dkp.GetShortestSequences(k))
                .GroupBy(s => s.First().Length)
                .OrderBy(g => g.Key)
                .First()
                .SelectMany(e => e)
                .Distinct()
                .ToList();

            var keysToPress3 = keysToPress2.Select(k => dkp.GetShortestSequences(k))
                .GroupBy(s => s.First().Length)
                .OrderBy(g => g.Key)
                .First()
                .SelectMany(e => e)
                .Distinct()
                .ToList()
                .First();

            var sequenceLength = keysToPress3.Length;

            totalComplexity += numPart * sequenceLength;
        }

        Console.WriteLine(totalComplexity);
    }

    //private KeyPad _numeric = new NumericKeyPad();
    private KeyPad _directional = new DirectionalKeyPad();

    private Dictionary<(char from, char to, int robot), List<string>> _lookup1 = new();

    private List<string> GetShortestPaths(char from, char to, int robot)
    {
        if (robot == 0)
        {
            Console.WriteLine("reached robot 0");
            return _directional.Lookup[(from, to)];
        }

        if (_lookup1.ContainsKey((from, to, robot)))
        {
            //Console.WriteLine("hit " + from + "->" + to + " " + robot);
            return _lookup1[(from, to, robot)];
        }

        var path = _directional.Lookup[(from, to)].Select(s => GetShortestPath(s, robot)).ToList();

        _lookup1[(from, to, robot)] = path;
        return path;
    }

    private Dictionary<(string command, int robot), string> _lookup2 = new();

    private string GetShortestPath(string commands, int robot)
    {
        if (_lookup2.ContainsKey((commands, robot)))
        {
            //Console.WriteLine("hit " + commands + " " + robot);
            return _lookup2[(commands, robot)];
        }

        var paths = new List<string> { "" };
        var c = 'A';
        foreach (var n in commands)
        {
            var nextPaths = new List<string>();
            var stepPaths = GetShortestPaths(c, n, robot - 1);
            foreach (var p in paths)
            {
                nextPaths.AddRange(stepPaths.Select(s => p + s));
            }

            c = n;
            paths = nextPaths;
        }

        var shortest = paths.OrderBy(s => s.Length).First();
        _lookup2[(commands, robot)] = shortest;
        return shortest;
    }

    public void Solve2(string[] lines)
    {
        _lookup1 = new();
        _lookup2 = new();
        var totalComplexity = 0L;
        const int dPads = 26;
        foreach (var line in lines)
        {
            var numPart = long.Parse(line.Substring(0, line.Length - 1));
            var nkp = new NumericKeyPad();
            var keysToPress = nkp.GetShortestSequences(line);

            var sequences = keysToPress.Select(s => GetShortestPath(s, dPads)).Select(s => new { s, s.Length });

            Console.WriteLine(sequences.First().Length);
            totalComplexity += numPart * sequences.OrderBy(s => s.Length).First().Length;
        }

        Console.WriteLine(totalComplexity);
    }
}
