namespace AdventOfCode.task._2025;

public class Task08 : ITask
{
    public void Solve(string[] lines)
    {
        var connections = lines.Length > 20 ? 1000 : 10;
        var groupNo = 1;
        var groups = lines.Select(l =>
        {
            var split = l.Split(',').Select(int.Parse).ToList();
            return new Coordinate(split[0], split[1], split[2]);
        }).ToDictionary(k => k, _ => groupNo++);

        var dists = new Dictionary<(Coordinate, Coordinate), double>();

        for (var j1 = 0; j1 < groups.Count; j1++)
        {
            for (var j2 = j1 + 1; j2 < groups.Count; j2++)
            {
                if (j1 == j2) continue;
                var coord1 = groups.Keys.ElementAt(j1);
                var coord2 = groups.Keys.ElementAt(j2);
                var dist = Math.Sqrt(Math.Pow(coord1.X - coord2.X, 2) +
                                     Math.Pow(coord1.Y - coord2.Y, 2) +
                                     Math.Pow(coord1.Z - coord2.Z, 2));
                dists[(coord1, coord2)] = dist;
            }
        }

        var orderedConnections = dists
            .OrderBy(e => e.Value)
            //.Select(k => k.Key)
            .ToList();

        for (var c = 0; c < connections; c++)
        {
            var j1 = orderedConnections[c].Key.Item1;
            var j2 = orderedConnections[c].Key.Item2;

            if (groups[j1] == groups[j2])
                continue;

            foreach (var g2Old in groups.Where(e => e.Value == groups[j2]).ToList())
            {
                groups[g2Old.Key] = groups[j1];
            }
        }

        var groupSizes = groups.GroupBy(e => e.Value).Select(e => e.Count());

        var top = groupSizes.OrderByDescending(e => e).Take(3);
        var product = 1;
        foreach (var size in top)
        {
            product *= size;
        }

        Console.WriteLine(product);
    }

    public void Solve2(string[] lines)
    {
        var groupNo = 1;
        var groups = lines.Select(l =>
        {
            var split = l.Split(',').Select(int.Parse).ToList();
            return new Coordinate(split[0], split[1], split[2]);
        }).ToDictionary(k => k, _ => groupNo++);

        var dists = new Dictionary<(Coordinate, Coordinate), double>();

        for (var j1 = 0; j1 < groups.Count; j1++)
        {
            for (var j2 = j1 + 1; j2 < groups.Count; j2++)
            {
                if (j1 == j2) continue;
                var coord1 = groups.Keys.ElementAt(j1);
                var coord2 = groups.Keys.ElementAt(j2);
                var dist = Math.Sqrt(Math.Pow(coord1.X - coord2.X, 2) +
                                     Math.Pow(coord1.Y - coord2.Y, 2) +
                                     Math.Pow(coord1.Z - coord2.Z, 2));
                dists[(coord1, coord2)] = dist;
            }
        }

        var orderedConnections = dists
            .OrderBy(e => e.Value)
            //.Select(k => k.Key)
            .ToList();

        (Coordinate, Coordinate) newCon = default;

        var idx = 0;

        while (groups.GroupBy(e => e.Value).Count() > 1)
        {
            var j1 = orderedConnections[idx].Key.Item1;
            var j2 = orderedConnections[idx].Key.Item2;
            idx++;
            if (groups[j1] == groups[j2])
                continue;
            
            newCon = (j1, j2);

            foreach (var g2Old in groups.Where(e => e.Value == groups[j2]).ToList())
            {
                groups[g2Old.Key] = groups[j1];
            }
        }

        Console.WriteLine(newCon.Item1.X * newCon.Item2.X);
    }
}

public record Coordinate(int X, int Y, int Z);
