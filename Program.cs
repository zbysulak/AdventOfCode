using AdventOfCode;

ITask task = new Task01();
var taskNumber = task.GetType().Name[4..];
const bool sample = false;
string[] lines;
try
{
    lines = File.ReadAllLines(@"..\..\..\input\" + taskNumber + (sample ? "-sample" : ""));
}
catch (FileNotFoundException)
{
    lines = Array.Empty<string>();
}

Console.WriteLine("First part:");
task.Solve(lines);
Console.WriteLine("Second part:");
task.Solve2(lines);