// See https://aka.ms/new-console-template for more information

using AdventOfCode;

string[] lines = System.IO.File.ReadAllLines(@"..\..\..\input\06");

var task = new Task06();
task.Solve(lines);
task.Solve2(lines);