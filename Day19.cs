using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Advent2022.Day16;
using static System.Windows.Forms.AxHost;

namespace Advent2022
{
    internal class Day19
    {
        public List<Blueprint> Blueprints = new List<Blueprint>();
        public string results = "";

        public record Robot(Material cost, Material produces);
        public record State(int remainingTime, Material available, Material produced);
        public record Blueprint(int num, Robot ore, Robot clay, Robot obsidian, Robot geode);


        public void GetInput(string input)
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line)) break;
                string cleanInput = line.Replace("Blueprint ", "").Replace(": Each ore robot costs ", ",").Replace(" ore. Each clay robot costs ", ",").Replace(" ore. Each obsidian robot costs ", ","); //remove the crap
                cleanInput = cleanInput.Replace(" ore and ", ",").Replace(" clay. Each geode robot costs ", ",").Replace(" obsidian.", "");  //remove more crap
                string[] data = cleanInput.Split(",");

                Blueprint bp = new Blueprint(
                    num: int.Parse(data[0]),
                    ore: new Robot(new Material(int.Parse(data[1]), 0, 0, 0), new Material(1, 0, 0, 0)),
                    clay: new Robot(new Material(int.Parse(data[2]), 0, 0, 0), new Material(0, 1, 0, 0)),
                    obsidian: new Robot(new Material(int.Parse(data[3]), int.Parse(data[4]), 0, 0), new Material(0, 0, 1, 0)),
                    geode: new Robot(new Material(int.Parse(data[5]), 0, int.Parse(data[6]), 0), new Material(0, 0, 0, 1))
                );
                Blueprints.Add(bp);
            }
        }

        public string ShowResults()
        {
            int totalQuality = 0;
            //Part 1
            //foreach (Blueprint blueprint in Blueprints)
            //{
            //    totalQuality += blueprint.num * FindMaxGeodes(blueprint, 24);
            //}

            //Part 2
            totalQuality = 1; //avoid multiplying by zero
            foreach (Blueprint blueprint in Blueprints.Where(p => p.num <= 3))
            {
                totalQuality *= FindMaxGeodes(blueprint, 32);
            }
            results += String.Format("Total geode quality: {0}\r\n", totalQuality);
            return results;
        }

        private int FindMaxGeodes(Blueprint blueprint, int timeLimit)
        {
            return MaxGeodes(
                blueprint,
                new State(
                    remainingTime: timeLimit,
                    available: new Material(ore: 0, 0, 0, 0),
                    produced: new Material(ore: 1, 0, 0, 0)
                ),
                new Dictionary<State, int>()
            );
        }

        int MaxGeodes(Blueprint bluePrint, State state, Dictionary<State, int> cache)
        {
            if (state.remainingTime == 0)
            {
                return state.available.geode;
            }

            if (!cache.ContainsKey(state))
            {
                cache[state] = (
                    from afterFactory in NextSteps(bluePrint, state)
                    let afterMining = afterFactory with
                    {
                        remainingTime = state.remainingTime - 1,
                        available = afterFactory.available + state.produced
                    }
                    select MaxGeodes(bluePrint, afterMining, cache)
                ).Max();
            }

            return cache[state];
        }

        IEnumerable<State> NextSteps(Blueprint bluePrint, State state)
        {
            var now = state.available;
            var prev = now - state.produced;

            if (!CanBuild(bluePrint.geode, prev) && CanBuild(bluePrint.geode, now))
            {
                yield return Build(state, bluePrint.geode); //Build geode as soon as possible
                yield break;
            }

            if (!CanBuild(bluePrint.obsidian, prev) && CanBuild(bluePrint.obsidian, now))
            {
                yield return Build(state, bluePrint.obsidian);
            }
            if (!CanBuild(bluePrint.clay, prev) && CanBuild(bluePrint.clay, now))
            {
                yield return Build(state, bluePrint.clay);
            }
            if (!CanBuild(bluePrint.ore, prev) && CanBuild(bluePrint.ore, now))
            {
                yield return Build(state, bluePrint.ore);
            }

            yield return state;
        }

        bool CanBuild(Robot robot, Material availableMaterial) => availableMaterial >= robot.cost;

        State Build(State state, Robot robot) =>
            state with
            {
                available = state.available - robot.cost,
                produced = state.produced + robot.produces
            };

        //This is cool, never knew you could do this!
        public record Material(int ore, int clay, int obsidian, int geode)
        {
            public static Material operator +(Material a, Material b)
            {
                return new Material(
                    a.ore + b.ore,
                    a.clay + b.clay,
                    a.obsidian + b.obsidian,
                    a.geode + b.geode
                );
            }

            public static Material operator -(Material a, Material b)
            {
                return new Material(
                    a.ore - b.ore,
                    a.clay - b.clay,
                    a.obsidian - b.obsidian,
                    a.geode - b.geode
                );
            }

            public static bool operator <=(Material a, Material b)
            {
                return
                    a.ore <= b.ore &&
                    a.clay <= b.clay &&
                    a.obsidian <= b.obsidian &&
                    a.geode <= b.geode;
            }

            public static bool operator >=(Material a, Material b)
            {
                return
                    a.ore >= b.ore &&
                    a.clay >= b.clay &&
                    a.obsidian >= b.obsidian &&
                    a.geode >= b.geode;
            }
        }
    }
}
