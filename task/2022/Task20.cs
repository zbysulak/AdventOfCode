// Day 20: Grove Positioning System

namespace AdventOfCode.task._2022;

public class Task20 : ITask
{
    private static int CircleMod(long number, int mod) => (int)((number % mod) + mod) % mod;

    public void Solve(string[] lines)
    {
        var list = lines.Select(int.Parse).ToList();
        var count = list.Count;
        var idxArray = Enumerable.Range(0, count).ToList(); // array to store current order of items (by its indexes)

        var dict = list.Select((item, index) => (item, index))
            .ToDictionary(i => i.index, i => i.item); // lookup dict for item's value by index

        //Console.WriteLine("Init: \t\t" + string.Join(" ", list));

        for (var originalIndex = 0;
             originalIndex < count;
             originalIndex++) // iterate over all indexes in original order
        {
            var currentIdx = idxArray.IndexOf(originalIndex); // find current index of item
            var item = dict[originalIndex];

            if (item > 0)
            {
                var end = (currentIdx + item) % count;
                for (int i = 0; i < item; i++)
                {
                    var idx = i + currentIdx;
                    var nI = idx % count;
                    if (nI + 1 < count)
                        idxArray[nI] = idxArray[nI + 1];
                    else
                        idxArray[nI] = idxArray[0];
                }

                idxArray[end] = originalIndex;
            }
            else if (item < 0)
            {
                var end = CircleMod(currentIdx + item, count);
                for (int i = 0; i > item; i--)
                {
                    var idx = currentIdx + i;
                    var nI = CircleMod(idx + count, count);
                    if (nI - 1 >= 0)
                        idxArray[nI] = idxArray[nI - 1];
                    else
                        idxArray[nI] = idxArray[count - 1];
                }

                idxArray[end] = originalIndex;
            }

            //Console.WriteLine($"Moved {item}: \t" + string.Join(" ", idxArray.Select(i => dict[i])));
        }

        var zeroIdx = idxArray.IndexOf(dict.Single(v => v.Value == 0).Key);
        var findNumber = (int i) => { return dict[idxArray[(zeroIdx + i) % count]]; };

        Console.WriteLine(findNumber(1000) + findNumber(2000) + findNumber(3000));
    }

    private const int Multiplier = 811589153;

    public void Solve2(string[] lines)
    {
        var list = lines.Select(i => long.Parse(i) * Multiplier).ToList();
        var count = list.Count;
        var idxArray = Enumerable.Range(0, count).ToList(); // array to store current order of items (by its indexes)

        var dict = list.Select((item, index) => (item, index))
            .ToDictionary(i => i.index, i => i.item); // lookup dict for item's value by index

        //Console.WriteLine("Init: \t\t" + string.Join(" ", list));

        foreach (var _ in Enumerable.Range(0, 10))
        {
            for (var originalIndex = 0;
                 originalIndex < count;
                 originalIndex++) // iterate over all indexes in original order
            {
                var currentIdx = idxArray.IndexOf(originalIndex); // find current index of item
                var item = dict[originalIndex];

                if (item > 0)
                {
                    for (int i = 0; i < item % (count - 1); i++) // order is kept after count -1 swaps
                    {
                        var idx = i + currentIdx;
                        var nI = idx % count;
                        if (nI + 1 < count)
                        {
                            (idxArray[nI], idxArray[nI + 1]) = (idxArray[nI + 1], idxArray[nI]);
                        }
                        else
                        {
                            (idxArray[nI], idxArray[0]) = (idxArray[0], idxArray[nI]);
                        }
                    }
                }
                else if (item < 0)
                {
                    for (int i = 0; i > item % (count - 1); i--)
                    {
                        var idx = currentIdx + i;
                        var nI = CircleMod(idx, count);
                        if (nI - 1 >= 0)
                        {
                            (idxArray[nI], idxArray[nI - 1]) = (idxArray[nI - 1], idxArray[nI]);
                        }
                        else
                        {
                            (idxArray[nI], idxArray[count - 1]) = (idxArray[count - 1], idxArray[nI]);
                        }
                    }
                }

                /*Console.WriteLine($"Moved {item / Multiplier}: \t" +
                              string.Join(" ", idxArray.Select(i => dict[i] / Multiplier))); /**/
            }
        }

        var zeroIdx = idxArray.IndexOf(dict.Single(v => v.Value == 0).Key);
        var findNumber = (int i) => { return dict[idxArray[(zeroIdx + i) % count]]; };

        Console.WriteLine(findNumber(1000) + findNumber(2000) + findNumber(3000));
    }
}