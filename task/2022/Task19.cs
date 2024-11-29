// Day 19: Not Enough Minerals

/*
 * Pruning used:
 * Only make robots if there is robot more expensive than my current count of robots
 * If I decide not to create new robot, i don't consider making that type until I make another
 * I only make ore robots if I don't have any obsidian robot
 * I only make clay robots if I don't have any geode robot
 * I only continue from state if it does not have less than max-1 geode robots of best state in current minute
 */

using System.Text.RegularExpressions;

namespace AdventOfCode.task._2022;

public class Task19 : ITask
{
    public void Solve(string[] lines)
    {
        var blueprints = ParseBlueprints(lines);

        Console.WriteLine(blueprints.Sum(bp => MaxGeodesOpened(bp, 24) * bp.BlueprintId));
    }

    private static int Max(params int[] numbers)
    {
        return numbers.Max();
    }

    private static int MaxGeodesOpened(RobotBlueprint bp, int timeLimit)
    {
        var states = new List<QuarryState> { new(0, 0, 0, 0, 0, 1, 0, 0, 0, false, false, false, false) };

        var maxPriceOre = Max(bp.OreRobotCost.Ore, bp.ClayRobotCost.Ore, bp.ObsidianRobotCost.Ore,
            bp.GeodeRobotCost.Ore);
        var maxPriceClay = Max(bp.ObsidianRobotCost.Clay);
        var maxPriceObsidian = Max(bp.GeodeRobotCost.Obsidian);

        for (var i = 0; i < timeLimit; i++)
        {
            var newStates = new List<QuarryState>();
            var maxGeodeRobots = states.Max(s => s.GeodeRobot);
            foreach (var state in states.Where(state => state.GeodeRobot >= maxGeodeRobots - 1))
            {
                if (state is { ObsidianRobot: 0, DidntBuildOreRobotButCould: false } && state.OreRobot < maxPriceOre &&
                    state.HasResources(bp.OreRobotCost))
                {
                    newStates.Add(state with
                    {
                        Minute = state.Minute + 1,
                        OreRobot = state.OreRobot + 1,
                        Ore = state.Ore - bp.OreRobotCost.Ore + state.OreRobot,
                        Clay = state.Clay + state.ClayRobot,
                        Obsidian = state.Obsidian + state.ObsidianRobot,
                        Geode = state.Geode + state.GeodeRobot,
                        DidntBuildOreRobotButCould = false,
                        DidntBuildClayRobotButCould = false,
                        DidntBuildObsidianRobotButCould = false,
                        DidntBuildGeodeRobotButCould = false,
                    });
                }

                if (state is { GeodeRobot: 0, DidntBuildClayRobotButCould: false } && state.ClayRobot < maxPriceClay &&
                    state.HasResources(bp.ClayRobotCost))
                {
                    newStates.Add(state with
                    {
                        Minute = state.Minute + 1,
                        ClayRobot = state.ClayRobot + 1,
                        Ore = state.Ore - bp.ClayRobotCost.Ore + state.OreRobot,
                        Clay = state.Clay + state.ClayRobot,
                        Obsidian = state.Obsidian + state.ObsidianRobot,
                        Geode = state.Geode + state.GeodeRobot,
                        DidntBuildOreRobotButCould = false,
                        DidntBuildClayRobotButCould = false,
                        DidntBuildObsidianRobotButCould = false,
                        DidntBuildGeodeRobotButCould = false,
                    });
                }

                if (!state.DidntBuildObsidianRobotButCould && state.ObsidianRobot < maxPriceObsidian &&
                    state.HasResources(bp.ObsidianRobotCost))
                {
                    newStates.Add(state with
                    {
                        Minute = state.Minute + 1,
                        ObsidianRobot = state.ObsidianRobot + 1,
                        Ore = state.Ore - bp.ObsidianRobotCost.Ore + state.OreRobot,
                        Clay = state.Clay - bp.ObsidianRobotCost.Clay + state.ClayRobot,
                        Obsidian = state.Obsidian + state.ObsidianRobot,
                        Geode = state.Geode + state.GeodeRobot,
                        DidntBuildOreRobotButCould = false,
                        DidntBuildClayRobotButCould = false,
                        DidntBuildObsidianRobotButCould = false,
                        DidntBuildGeodeRobotButCould = false,
                    });
                }

                if (!state.DidntBuildGeodeRobotButCould && state.HasResources(bp.GeodeRobotCost))
                {
                    newStates.Add(state with
                    {
                        Minute = state.Minute + 1,
                        GeodeRobot = state.GeodeRobot + 1,
                        Ore = state.Ore - bp.GeodeRobotCost.Ore + state.OreRobot,
                        Clay = state.Clay + state.ClayRobot,
                        Obsidian = state.Obsidian - bp.GeodeRobotCost.Obsidian + state.ObsidianRobot,
                        Geode = state.Geode + state.GeodeRobot,
                        DidntBuildOreRobotButCould = false,
                        DidntBuildClayRobotButCould = false,
                        DidntBuildObsidianRobotButCould = false,
                        DidntBuildGeodeRobotButCould = false,
                    });
                }

                newStates.Add(state with
                {
                    Minute = state.Minute + 1,
                    Ore = state.Ore + state.OreRobot,
                    Clay = state.Clay + state.ClayRobot,
                    Obsidian = state.Obsidian + state.ObsidianRobot,
                    Geode = state.Geode + state.GeodeRobot,
                    DidntBuildOreRobotButCould = state.HasResources(bp.OreRobotCost),
                    DidntBuildClayRobotButCould = state.HasResources(bp.ClayRobotCost),
                    DidntBuildObsidianRobotButCould = state.HasResources(bp.ObsidianRobotCost),
                    DidntBuildGeodeRobotButCould = state.HasResources(bp.GeodeRobotCost),
                });
            }

            states = newStates;

            //Console.WriteLine("Minute " + i + ", have " + states.Count + " states!");
        }

        return states.Max(s => s.Geode);
    }

    private record QuarryState(
        int Minute,
        int Ore,
        int Clay,
        int Obsidian,
        int Geode,
        int OreRobot,
        int ClayRobot,
        int ObsidianRobot,
        int GeodeRobot,
        bool DidntBuildOreRobotButCould,
        bool DidntBuildClayRobotButCould,
        bool DidntBuildObsidianRobotButCould,
        bool DidntBuildGeodeRobotButCould)
    {
        public bool HasResources(RobotPrice rp)
        {
            return Ore >= rp.Ore && Clay >= rp.Clay && Obsidian >= rp.Obsidian;
        }
    }

    public void Solve2(string[] lines)
    {
        var bps = ParseBlueprints(lines.Take(3).ToArray());
        var g = bps.Select(bp => MaxGeodesOpened(bp, 32));
        var product = g.Aggregate(1, (current, i) => current * i); // Product of all results

        Console.WriteLine(product);
    }

    private static IEnumerable<RobotBlueprint> ParseBlueprints(IEnumerable<string> lines)
    {
        return lines.Select(line => Regex.Matches(line, @"\d+"))
            .Select(m => new RobotBlueprint
            {
                BlueprintId = int.Parse(m[0].Value),
                OreRobotCost = new RobotPrice { Ore = int.Parse(m[1].Value) },
                ClayRobotCost = new RobotPrice { Ore = int.Parse(m[2].Value) },
                ObsidianRobotCost = new RobotPrice { Ore = int.Parse(m[3].Value), Clay = int.Parse(m[4].Value) },
                GeodeRobotCost = new RobotPrice { Ore = int.Parse(m[5].Value), Obsidian = int.Parse(m[6].Value) },
            }).ToList();
    }
}

internal class RobotBlueprint
{
    public int BlueprintId { get; set; }
    public RobotPrice OreRobotCost { get; set; }
    public RobotPrice ClayRobotCost { get; set; }
    public RobotPrice ObsidianRobotCost { get; set; }
    public RobotPrice GeodeRobotCost { get; set; }
}

internal class RobotPrice
{
    public int Ore { get; set; }
    public int Clay { get; set; }
    public int Obsidian { get; set; }

    public override string ToString()
    {
        return $"{{Ore:{Ore}, Clay:{Clay}, Obsidian:{Obsidian}}}";
    }
}