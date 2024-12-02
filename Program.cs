using AdventOfCode.task._2024;

ITask task = new Task03();
var taskNumber = task.GetType().Name[4..];
const bool sample = true;
string[] lines;
try
{
    lines = File.ReadAllLines(@"..\..\..\input\2024\" + taskNumber + (sample ? "-sample" : ""));
}
catch (FileNotFoundException)
{
    lines = Array.Empty<string>();
}

Console.WriteLine("First part:");
task.Solve(lines);
Console.WriteLine("Second part:");
task.Solve2(lines);
