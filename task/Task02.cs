public class Task02 : ITask
{
    private const int PointsLose = 0;
    private const int PointsDraw = 3;
    private const int PointsWin = 6;

    private const int PointsRock = 1;
    private const int PointsPaper = 2;
    private const int PointsScissors = 3;

    public void Solve(string[] lines)
    {
        var points = 0;

        foreach (var line in lines)
        {
            var a = line switch
            {
                "A X" => PointsRock + PointsDraw,
                "A Y" => PointsPaper + PointsWin,
                "A Z" => PointsScissors + PointsLose,
                "B X" => PointsRock + PointsLose,
                "B Y" => PointsPaper + PointsDraw,
                "B Z" => PointsScissors + PointsWin,
                "C X" => PointsRock + PointsWin,
                "C Y" => PointsPaper + PointsLose,
                "C Z" => PointsScissors + PointsDraw,
                _ => throw new Exception("wut? o.O")
            };
            Console.WriteLine(a);
            points += a;
        }

        Console.WriteLine(points);
    }

    public void Solve2(string[] lines)
    {
        var points = 0;

        foreach (var line in lines)
        {
            var a = line switch
            {
                "A X" => PointsScissors + PointsLose,
                "A Y" => PointsRock + PointsDraw,
                "A Z" => PointsPaper + PointsWin,
                "B X" => PointsRock + PointsLose,
                "B Y" => PointsPaper + PointsDraw,
                "B Z" => PointsScissors + PointsWin,
                "C X" => PointsPaper + PointsLose,
                "C Y" => PointsScissors + PointsDraw,
                "C Z" => PointsRock + PointsWin,
                _ => throw new Exception("wut? o.O")
            };
            Console.WriteLine(a);
            points += a;
        }

        Console.WriteLine(points);
    }
}