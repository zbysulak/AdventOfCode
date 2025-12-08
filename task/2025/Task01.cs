namespace AdventOfCode.task._2025;

public class Task01 : ITask
{
    public void Solve(string[] lines)
    {
        var pos = 50;
        var zeroCount = 0;
        foreach (var line in lines)
        {
            var d = line[0];
            var dist = int.Parse(line[1..]);
            switch (d)
            {
                case 'L':
                    pos += -dist;
                    break;
                case 'R':
                    pos += dist;
                    break;
                default:
                    throw new NotImplementedException();
            }

            pos = pos % 100;
            if (pos == 0)
                zeroCount++;
        }

        Console.WriteLine(zeroCount);
    }

    public void Solve2(string[] lines)
    {
        var pos = 50;
        var zeroCount = 0;
        foreach (var line in lines)
        {
            var d = line[0];
            var dist = int.Parse(line[1..]);
            if (d == 'L')
                dist = -dist;

            pos += 1000;

            var newPos = pos + dist;

            var zeroPasses = (pos % 100 == 0 || newPos == 0 ? -1 : 0) + Math.Abs((newPos / 100) - (pos / 100));

            pos = (newPos % 100 + 100) % 100;
            if (pos == 0)
            {
                //zeroPasses = Math.Max(zeroPasses - 1, 0);
                zeroCount++;
            }

            zeroCount += zeroPasses;
        }

        Console.WriteLine(zeroCount); // 5967 too high
    }
}
