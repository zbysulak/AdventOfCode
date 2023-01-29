// Day 8: Treetop Tree House

namespace AdventOfCode;

public class Task07 : ITask
{
    public void Solve(string[] lines)
    {
        var forest = ParseInput(lines);
        //PrintForestWithVisibility(forest);
        var numberOfVisibleTrees = 0;
        for (int i = 0; i < forest.GetLength(0); i++)
        {
            for (int j = 0; j < forest.GetLength(1); j++)
            {
                numberOfVisibleTrees += IsTreeVisible(forest, i, j) ? 1 : 0;
            }
        }

        Console.WriteLine(numberOfVisibleTrees);
    }

    private void PrintForestWithVisibility(short[,] forest)
    {
        for (int i = 0; i < forest.GetLength(0); i++)
        {
            for (int j = 0; j < forest.GetLength(1); j++)
            {
                Console.Write(forest[i, j] + (IsTreeVisible(forest, i, j) ? "+" : "-"));
            }

            Console.WriteLine();
        }
    }

    private bool IsTreeVisible(short[,] forest, int row, int col)
    {
        if (row == 0 || row == forest.GetLength(0) || col == 0 || col == forest.GetLength(1))
            return true; // trees on the edge are always visible
        var currentTree = forest[row, col];
        bool left = true, right = true, top = true, bottom = true;

        for (int c = 0; c < col; c++)
        {
            if (forest[row, c] >= currentTree)
            {
                left = false;
                break;
            }
        }

        for (int c = forest.GetLength(1) - 1; c > col; c--)
        {
            if (forest[row, c] >= currentTree)
            {
                right = false;
                break;
            }
        }

        for (int r = 0; r < row; r++)
        {
            if (forest[r, col] >= currentTree)
            {
                top = false;
                break;
            }
        }

        for (int r = forest.GetLength(0) - 1; r > row; r--)
        {
            if (forest[r, col] >= currentTree)
            {
                bottom = false;
                break;
            }
        }

        return left | right | top | bottom;
    }

    public void Solve2(string[] lines)
    {
        var forest = ParseInput(lines);
        //PrintForestWithVisibility(forest);
        var topScore = 0;
        for (int i = 0; i < forest.GetLength(0); i++)
        {
            for (int j = 0; j < forest.GetLength(1); j++)
            {
                var ss = GetScenicScore(forest, i, j);
                if (ss > topScore) topScore = ss;
            }
        }

        Console.WriteLine(topScore);
    }

    /// <summary>returns multiplied number of trees visible in each direction</summary>
    private int GetScenicScore(short[,] forest, int row, int col)
    {
        if (row == 0 || row == forest.GetLength(0) || col == 0 || col == forest.GetLength(1))
            return 0; // score of trees on edge are always 0

        var currentTree = forest[row, col];
        int left = 0, right = 0, top = 0, bottom = 0;

        for (int c = col - 1; c >= 0; c--)
        {
            left++;
            if (forest[row, c] >= currentTree)
            {
                break;
            }
        }

        for (int c = col + 1; c < forest.GetLength(1); c++)
        {
            right++;
            if (forest[row, c] >= currentTree)
            {
                break;
            }
        }

        for (int r = row - 1; r >= 0; r--)
        {
            top++;
            if (forest[r, col] >= currentTree)
            {
                break;
            }
        }

        for (int r = row + 1; r < forest.GetLength(0); r++)
        {
            bottom++;
            if (forest[r, col] >= currentTree)
            {
                break;
            }
        }

        return left * right * top * bottom;
    }

    private short[,] ParseInput(string[] lines)
    {
        var forest = new short[lines.Length, lines[0].Length];
        for (int i = 0; i < forest.GetLength(0); i++)
        {
            for (int j = 0; j < forest.GetLength(1); j++)
            {
                forest[i, j] = short.Parse(lines[i][j].ToString());
            }
        }

        return forest;
    }
}