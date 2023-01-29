using AdventOfCode;

ITask task = new Task06();
var tasknumber = task.GetType().ToString().Substring(4);
var sample = true;
string[] lines = System.IO.File.ReadAllLines(@"..\..\..\input\" + tasknumber + (sample ? "-sample" : ""));
Console.WriteLine("First part:");
task.Solve(lines);
Console.WriteLine("Second part:");
task.Solve2(lines);