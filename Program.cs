using AdventOfCode.task._2024;

ITask task = new Task15();
var taskNumber = task.GetType().Name[4..];

var lines = File.ReadAllLines(@"..\..\..\input\2024\" + taskNumber);
var linesSample = File.ReadAllLines(@"..\..\..\input\2024\" + taskNumber + "-sample");

Console.Write("First part:\n\tSample:\t");
task.Solve(linesSample);
Console.Write("\tReal:\t");
task.Solve(lines);
Console.Write("\nSecond part:\n\tSample:\t");
task.Solve2(linesSample);
Console.Write("\tReal:\t");
task.Solve2(lines);
