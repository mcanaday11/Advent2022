using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2022
{
    internal class Day12
    {
        public string result = "";
        public Map? map;

        public void GetMapInput(string input)
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            map = Map.Parse(lines);
        }

        public string MapResults()
        {

            var result1 = FindPathLength(map, map.Start, map.Goal);
            result += String.Format("Result 1: {0}\r\n", result1);

            var result2 = map.Heights
                             .Where(x => x.Value == 1)
                             .Select(x => FindPathLength(map, x.Key, map.Goal))
                             .Where(p => p.HasValue)
                             .Min();
            result += String.Format("Result 2: {0}\r\n", result2);
            return result;
        }

        static int? FindPathLength(Map map, Point start, Point goal)
        {
            var depth = new Dictionary<Point, int>() { [start] = 0 };
            var queue = new Queue<Point>(depth.Keys);
            while (queue.Count > 0)
            {
                var pt = queue.Dequeue();
                if (pt == goal)
                    break;

                var d = depth[pt];
                var adjacent = map.GetValidMoves(pt)
                                  .Where(x => !depth.ContainsKey(x));
                foreach (var item in adjacent)
                {
                    depth[item] = d + 1;
                    queue.Enqueue(item);
                }
            }

            return depth.TryGetValue(goal, out var result)
              ? result
              : default(int?);
        }

        public record struct Point(int X, int Y)
        {
            public IEnumerable<Point> GetAdjacentPoints()
            {
                yield return new(X - 1, Y);
                yield return new(X, Y - 1);
                yield return new(X + 1, Y);
                yield return new(X, Y + 1);
            }
        }

        public record Map(ImmutableDictionary<Point, int> Heights, Point Start, Point Goal)
        {
            public IEnumerable<Point> GetValidMoves(Point location)
            {
                if (!Heights.TryGetValue(location, out var height))
                    return Enumerable.Empty<Point>();

                var max = height + 1;
                return from pt in location.GetAdjacentPoints()
                       where Heights.TryGetValue(pt, out var ptHeight) && ptHeight <= max
                       select pt;
            }

            public static Map Parse(string[] input)
            {
                Point start = default,
                      goal = default;
                var result = ImmutableDictionary.CreateBuilder<Point, int>();

                for (int row = 0; row < input.Length; ++row)
                {
                    string line = input[row];
                    for (var col = 0; col < line.Length; ++col)
                    {
                        Point pos = new Point(col, row);
                        char ch = line[col];
                        if (ch == 'S')
                        {
                            (result[pos], start) = (1, pos);
                        } else if (ch == 'E')
                        {
                            (result[pos], goal) = (26, pos);
                        } else
                        {
                            result[pos] = ch - 'a' + 1;
                        }
                    }
                }

                return new(result.ToImmutable(), start, goal);
            }
        }
    }
}
