namespace AdventOfCode.task._2025;

public class Task04 : ITask
{
    public void Solve(string[] lines)
    {
        var movableRolls = 0;
        foreach (var (x, y) in Utils.IterateOverGrid(lines))
        {
            var c = lines[y][x];
            if (c != '@') continue;
            var rollsAround = 0;
            foreach (var d in Utils.Directions8)
            {
                var nx = x + d[0];
                var ny = y + d[1];
                if (!Utils.CheckBounds(lines, nx, ny))
                    continue;
                if (lines[ny][nx] == '@')
                    rollsAround++;
            }

            if (rollsAround < 4)
                movableRolls++;
        }

        Console.WriteLine(movableRolls);
    }

    public void Solve2(string[] lines)
    {
        var grid = lines.Select(e => e.Select(c => c == '@').ToArray()).ToArray();
        var totalRemovedRolls = 0;
        while (true)
        {
            var newGrid = new bool[grid.Length][];
            var removedRolls = 0;
            foreach (var (x, y) in Utils.IterateOverGrid(lines))
            {
                if (newGrid[y] == null)
                    newGrid[y] = new bool[lines[y].Length];
                var roll = grid[y][x];
                newGrid[y][x] = roll;
                if (!roll) continue;
                var rollsAround = 0;
                foreach (var d in Utils.Directions8)
                {
                    var nx = x + d[0];
                    var ny = y + d[1];
                    if (!Utils.CheckBounds(newGrid, nx, ny))
                        continue;
                    if (grid[ny][nx])
                        rollsAround++;
                }

                if (rollsAround < 4)
                {
                    removedRolls++;
                    newGrid[y][x] = false;
                }
            }

            totalRemovedRolls += removedRolls;
            grid = newGrid;
            if (removedRolls == 0)
                break;
        }

        Console.WriteLine(totalRemovedRolls);
    }
}
