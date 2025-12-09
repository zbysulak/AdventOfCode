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
            var left = d == 'L';
            if (left)
                dist = -dist;

            var newPos = pos + dist;
            var zeroPasses = Math.Abs(newPos / 100);
            
            if (newPos < 0 && pos != 0)
                zeroPasses++;
            if(left && newPos == 0)
                zeroPasses++;
            pos = (newPos % 100 + 100) % 100;
            zeroCount += zeroPasses;
            //Console.WriteLine($"After {line}: pos={pos}, zeroPasses={zeroPasses}");
        }

        Console.WriteLine(zeroCount); // 5967 too high
    }
}
