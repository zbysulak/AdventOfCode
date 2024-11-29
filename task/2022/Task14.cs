using System.Drawing;

namespace AdventOfCode.task._2022;

public class Task14 : ITask
{
    public void Solve(string[] lines)
    {
        var walls = new List<Point>();
        var topLeft = new Point(int.MaxValue, 0);
        var bottomRight = new Point(0, 0);
        foreach (var line in lines)
        {
            var points = line.Split(" -> ").Select(p =>
            {
                var c = p.Split(",");
                return new Point(int.Parse(c[0]), int.Parse(c[1]));
            }).ToList();

            // find edges of field
            foreach (var p in points)
            {
                if (p.X < topLeft.X)
                {
                    topLeft = topLeft with { X = p.X };
                }

                if (p.X > bottomRight.X)
                {
                    bottomRight = bottomRight with { X = p.X };
                }

                if (p.Y > bottomRight.Y)
                {
                    bottomRight = bottomRight with { Y = p.Y };
                }
            }

            // create point for each piece of wall
            for (int i = 0; i < points.Count - 1; i++)
            {
                if (points[i].X == points[i + 1].X) // wall is vertical
                {
                    var y1 = points[i].Y;
                    var y2 = points[i + 1].Y;
                    foreach (var y in Enumerable.Range((y1 < y2 ? y1 : y2), Math.Abs(y1 - y2) + 1))
                    {
                        walls.Add(new(points[i].X, y));
                    }
                }
                else // horizontal
                {
                    var x1 = points[i].X;
                    var x2 = points[i + 1].X;
                    foreach (var x in Enumerable.Range(x1 < x2 ? x1 : x2, Math.Abs(x1 - x2) + 1))
                    {
                        walls.Add(new(x, points[i].Y));
                    }
                }
            }
        }

        var source = new Point(500, 0);
        var fallingSand = source;
        var sandCounter = 1;
        var fellToVoid = false;

        var printer = () =>
        {
            for (int y = topLeft.Y; y <= bottomRight.Y; y++)
            {
                for (int x = topLeft.X; x <= bottomRight.X; x++)
                {
                    if (walls.Contains(new(x, y)))
                        Console.Write("#");
                    else if (fallingSand == new Point(x, y))
                        Console.Write("o");
                    else Console.Write(".");
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        };

        printer();

        while (fallingSand.Y < bottomRight.Y)
        {
            if (!walls.Contains(fallingSand with { Y = fallingSand.Y + 1 }))
            {
                fallingSand = fallingSand with { Y = fallingSand.Y + 1 };
            }
            else if (!walls.Contains(new Point(fallingSand.X - 1, fallingSand.Y + 1)))
            {
                fallingSand = new Point(fallingSand.X - 1, fallingSand.Y + 1);
            }
            else if (!walls.Contains(new Point(fallingSand.X + 1, fallingSand.Y + 1)))
            {
                fallingSand = new Point(fallingSand.X + 1, fallingSand.Y + 1);
            }
            else // sand has nowhere to move, generate new one
            {
                //printer();
                walls.Add(fallingSand);
                sandCounter++;
                fallingSand = source;
            }
        }

        printer();

        Console.WriteLine(sandCounter - 1);
    }

    public void Solve2(string[] lines)
    {
        var walls = new List<Point>();
        var topLeft = new Point(int.MaxValue, 0);
        var bottomRight = new Point(0, 0);
        foreach (var line in lines)
        {
            var points = line.Split(" -> ").Select(p =>
            {
                var c = p.Split(",");
                return new Point(int.Parse(c[0]), int.Parse(c[1]));
            }).ToList();

            // find edges of field
            foreach (var p in points)
            {
                if (p.X < topLeft.X)
                {
                    topLeft = topLeft with { X = p.X };
                }

                if (p.X > bottomRight.X)
                {
                    bottomRight = bottomRight with { X = p.X };
                }

                if (p.Y > bottomRight.Y)
                {
                    bottomRight = bottomRight with { Y = p.Y };
                }
            }

            // create point for each piece of wall
            for (int i = 0; i < points.Count - 1; i++)
            {
                if (points[i].X == points[i + 1].X) // wall is vertical
                {
                    var y1 = points[i].Y;
                    var y2 = points[i + 1].Y;
                    foreach (var y in Enumerable.Range((y1 < y2 ? y1 : y2), Math.Abs(y1 - y2) + 1))
                    {
                        walls.Add(new(points[i].X, y));
                    }
                }
                else // horizontal
                {
                    var x1 = points[i].X;
                    var x2 = points[i + 1].X;
                    foreach (var x in Enumerable.Range(x1 < x2 ? x1 : x2, Math.Abs(x1 - x2) + 1))
                    {
                        walls.Add(new(x, points[i].Y));
                    }
                }
            }
        }
        
        //adding "virtual" wall 2 bellow lowest point
        for (int i = 0; i < 1000; i++)
        {
            walls.Add(new (i,bottomRight.Y+2));
        }

        var source = new Point(500, 0);
        var fallingSand = source;
        var sandCounter = 1;
        var fellToVoid = false;

        var printer = () =>
        {
            for (int y = topLeft.Y; y <= bottomRight.Y + 2; y++)
            {
                for (int x = topLeft.X; x <= bottomRight.X; x++)
                {
                    if (walls.Contains(new(x, y)))
                        Console.Write("#");
                    else if (fallingSand == new Point(x, y))
                        Console.Write("o");
                    else Console.Write(".");
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        };

        printer();

        while (true)
        {
            if (!walls.Contains(fallingSand with { Y = fallingSand.Y + 1 }))
            {
                fallingSand = fallingSand with { Y = fallingSand.Y + 1 };
            }
            else if (!walls.Contains(new Point(fallingSand.X - 1, fallingSand.Y + 1)))
            {
                fallingSand = new Point(fallingSand.X - 1, fallingSand.Y + 1);
            }
            else if (!walls.Contains(new Point(fallingSand.X + 1, fallingSand.Y + 1)))
            {
                fallingSand = new Point(fallingSand.X + 1, fallingSand.Y + 1);
            }
            else // sand has nowhere to move, generate new one
            {
                // I could also create new sand when it leaves a rectangle (> max or < min) since it is obvious what will happen 
                //printer();
                if (fallingSand == source)
                {
                    Console.WriteLine($"Damn I can't move @ {sandCounter}");
                    break;
                }
                sandCounter++;
                walls.Add(fallingSand);
                fallingSand = source;
            }
        }
    }
}