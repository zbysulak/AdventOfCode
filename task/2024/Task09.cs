// Day 9: Disk Fragmenter

namespace AdventOfCode.task._2024;

public class Task09 : ITask
{
    // done in 25 minutes
    public void Solve(string[] lines)
    {
        var sum = 0L;

        var memory = new List<int>();
        var file = true;
        var fileId = 0;
        foreach (var c in lines[0].Select(s => int.Parse(s.ToString())))
        {
            for (int i = 0; i < c; i++)
            {
                memory.Add(file ? fileId : -1);
            }

            if (file)
                fileId++;
            file = !file;
        }

        var backIdx = memory.Count - 1;
        for (int i = 0; i <= backIdx; i++)
        {
            if (memory[i] >= 0)
            {
                // Console.WriteLine(memory[i] + " * " + i);
                sum += memory[i] * i;
            }
            else
            {
                while (memory[backIdx] < 0)
                    backIdx--;
                sum += memory[backIdx] * i;
                // Console.WriteLine(memory[backIdx] + " * " + i);
                backIdx--;
            }
        }

        Console.WriteLine(sum);
    }

    //done in 43 minutes (but during meeting :D )
    public void Solve2(string[] lines)
    {
        var sum = 0L;

        var memory = new List<int>();
        var file = true;
        var fileId = 0;
        foreach (var c in lines[0].Select(s => int.Parse(s.ToString())))
        {
            for (int i = 0; i < c; i++)
            {
                memory.Add(file ? fileId : -1);
            }

            if (file)
                fileId++;
            file = !file;
        }

        for (fileId = memory[^1]; fileId >= 0; fileId--)
        {
            var (idx, size) = FindFile(memory, fileId);
            for (int i = 0; i < idx; i++)
            {
                if (memory[i] < 0)
                {
                    var gapSize = 1;
                    while (memory[i + gapSize] < 0)
                        gapSize++;

                    if (size <= gapSize)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            memory[i + j] = memory[idx + j];
                            memory[idx + j] = -1;
                        }

                        break;
                    }
                }
            }
        }

        for (int i = 0; i < memory.Count; i++)
        {
            //Console.Write(memory[i] >= 0 ? memory[i] : ".");
            if (memory[i] < 0) continue;
            sum += memory[i] * i;
        }

        //Console.WriteLine();

        Console.WriteLine(sum);
    }

    private (int idx, int size) FindFile(List<int> memory, int id)
    {
        var idx = memory.Count - 1;
        var size = 0;
        while (memory[idx] != id)
        {
            idx--;
        }

        while (idx >= 0 && memory[idx] == id)
        {
            idx--;
            size++;
        }

        return (idx + 1, size);
    }
}
