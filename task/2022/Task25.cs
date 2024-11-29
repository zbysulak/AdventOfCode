namespace AdventOfCode.task._2022;

public class Task25 : ITask
{
    private long DecodeSnafu(string snafu)
    {
        long result = 0L;
        for (int i = 0; i < snafu.Length; i++)
        {
            char ch = snafu[snafu.Length - 1 - i];
            result += ch switch
            {
                '=' => -2,
                '-' => -1,
                '0' => 0,
                '1' => 1,
                '2' => 2,
                _ => throw new Exception($"Invalid SNAFU character ({ch})")
            } * (long)Math.Pow(5, i);
        }

        return result;
    }

    private string EncodeSnafu(long number)
    {
        var pow = 0;
        while (Math.Pow(5, pow) * 2 < number)
        {
            pow++;
        }

        var result = new List<char>();

        do
        {
            if (number < 0)
            {
                var n = number;
                for (int i = pow - 1; i >= 0; i--)
                {
                    n -= 2 * (long)Math.Pow(5, i);
                }
                var r = n / (long)Math.Pow(5, pow);
                result.Add(r switch { -1 => '-', -2 => '=', _ => '0' });
                number -= r * (long)Math.Pow(5, pow);
            }
            else
            {
                var n = number;
                for (int i = pow - 1; i >= 0; i--)
                {
                    n += 2 * (long)Math.Pow(5, i);
                }

                var r = n / (long)Math.Pow(5, pow);

                result.Add(r.ToString()[0]);
                number -= r * (long)Math.Pow(5, pow);
            }

            pow--;
        } while (pow >= 0);

        if (result[0] == '0' && result.Count > 1)
            result = result.Skip(1).ToList();
        return string.Join("", result);
    }

    public void Solve(string[] lines)
    {
        /*for (int i = 0; i <= 1000; i++)
        {
            var en = EncodeSnafu(i);
            var de = DecodeSnafu(en);
            if(de == i) continue;
            Console.Write(i + "\t" + en + "\t");
            if (de != i)
                Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(de);
            Console.ForegroundColor = ConsoleColor.White;
        }*/
        
        Console.WriteLine(EncodeSnafu(lines.Sum(DecodeSnafu)));
        
    }

    public void Solve2(string[] lines)
    {
    }
}