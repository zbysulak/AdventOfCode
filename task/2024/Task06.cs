// Day 6: Guard Gallivant

namespace AdventOfCode.task._2024;

public class Task06 : ITask
{
    private int[] GetStart(string[] lines, char startChar)
    {
        var pos = new[] { 0, 0 };
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == startChar)
                {
                    pos = new[] { x, y };
                    break;
                }
            }
        }

        return pos;
    }

    private readonly int[,] _dirs = { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 } };

    // done in 25 minutes
    public void Solve(string[] lines)
    {
        var dir = 0;
        var pos = GetStart(lines, '^');

        var visited = new HashSet<(int, int)>();

        do
        {
            visited.Add((pos[0], pos[1]));
            var newPos = new[] { pos[0] + _dirs[dir, 0], pos[1] + _dirs[dir, 1] };
            if (!InBounds(newPos[0], newPos[1], lines))
            {
                break;
            }

            if (lines[newPos[1]][newPos[0]] == '#')
                while (lines[newPos[1]][newPos[0]] == '#')
                {
                    dir = (dir + 1) % 4;
                    newPos = new[] { pos[0] + _dirs[dir, 0], pos[1] + _dirs[dir, 1] };
                }

            pos = newPos;
        } while (InBounds(pos[0], pos[1], lines));

        Console.WriteLine(visited.Count);
    }

    private bool InBounds(int x, int y, string[] lines)
    {
        return x >= 0 && x < lines[0].Length && y >= 0 && y < lines.Length;
    }

    //brute force solution done in 1:44 hours
    public void Solve2(string[] lines)
    {
        var dir = 0;
        var pos = GetStart(lines, '^');

        /* BF solution
        var obs = new HashSet<(int, int)>();
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (IsThereCycle(lines, new[] { x, y }))
                    obs.Add((x, y));
            }
        }
        Console.WriteLine(obs.Count);
        */

        // I can't check it as guard goes because guard will reach place otherwise unreachable
        // So I get path without any obstacle and then check if there is a cycle on any part of path
        var visited = new HashSet<(int, int)>();
        do
        {
            visited.Add((pos[0], pos[1]));
            var newPos = new[] { pos[0] + _dirs[dir, 0], pos[1] + _dirs[dir, 1] };
            if (!InBounds(newPos[0], newPos[1], lines)) break;

            if (lines[newPos[1]][newPos[0]] == '#')
            {
                dir = (dir + 1) % 4;
                newPos = new[] { pos[0] + _dirs[dir, 0], pos[1] + _dirs[dir, 1] };
            }

            pos = newPos;
        } while (InBounds(pos[0], pos[1], lines));

        Console.WriteLine(visited.Count(p => IsThereCycle(lines, new int[]{p.Item1, p.Item2})));
    }

    private bool IsThereCycle(string[] lines, int[] extraObstacle)
    {
        var pos = GetStart(lines, '^');
        var dir = 0;
        var visited = new HashSet<(int, int, int)>();
        do
        {
            if (visited.Contains((pos[0], pos[1], dir)))
                return true;
            visited.Add((pos[0], pos[1], dir));
            var newPos = new[] { pos[0] + _dirs[dir, 0], pos[1] + _dirs[dir, 1] };
            if (!InBounds(newPos[0], newPos[1], lines))
            {
                break;
            }

            if (lines[newPos[1]][newPos[0]] == '#' || (newPos[0] == extraObstacle[0] && newPos[1] == extraObstacle[1]))
            {
                while (lines[newPos[1]][newPos[0]] == '#' ||
                       (newPos[0] == extraObstacle[0] && newPos[1] == extraObstacle[1]))
                {
                    dir = (dir + 1) % 4;
                    newPos = new[] { pos[0] + _dirs[dir, 0], pos[1] + _dirs[dir, 1] };
                }
            }

            pos = newPos;
        } while (InBounds(pos[0], pos[1], lines));

        return false;
    }
}
