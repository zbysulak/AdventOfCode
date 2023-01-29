// Day 3: Rucksack Reorganization

public class Task03 : ITask
{
    public void Solve(string[] lines)
    {
        var sum = 0;
        foreach (var line in lines)
        {
            sum += GetPriority(FindItem(line));
        }

        Console.WriteLine(sum);
    }

    private char FindItem(string rucksack)
    {
        for (int i = 0; i < rucksack.Length / 2; i++)
        {
            for (int j = rucksack.Length / 2; j < rucksack.Length; j++)
            {
                if (rucksack[i] == rucksack[j])
                    return rucksack[i];
            }
        }

        throw new Exception("No common item");
    }

    public void Solve2(string[] lines)
    {
        var sum = 0;
        for (int i = 0; i < lines.Length; i = i + 3)
        {
            sum += GetPriority(FindBadge(lines[i], lines[i + 1], lines[i + 2]));
        }

        Console.WriteLine(sum);
    }

    private char FindBadge(string r1, string r2, string r3)
    {
        var l1 = r1.ToList();
        l1.Sort();
        var l2 = r2.ToList();
        l2.Sort();
        var l3 = r3.ToList();
        l3.Sort();

        int i = 0, j = 0, k = 0;
        while (i < l1.Count() && j < l2.Count() && k < l3.Count())
        {
            if (l1[i] == l2[j] && l2[j] == l3[k]) return l1[i];

            if (l1[i] < l2[j]) i++;
            else if (l2[j] < l3[k]) j++;
            else k++;
        }

        throw new Exception("No common item");
    }

    private int GetPriority(char c)
    {
        if (c <= 'Z')
            return c - 'A' + 27;

        return c - 'a' + 1;
    }
}