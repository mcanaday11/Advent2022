using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Advent2022
{
    internal class Day15
    {
        public List<Sensor> sensors = new List<Sensor>();
        public string results = "";
        public int SolveYRow = 2000000; //10;
        public int MaxXYValues = 4000000;
        int minX, maxX;

        public void GetSensorInput(string input) //Day 15
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                string cleanInput = line.Replace("Sensor at x=", "").Replace(", y=", ",").Replace(": closest beacon is at x=", ",").Replace(", y=", ","); //remove the crap
                string[] data = cleanInput.Split(",");
                sensors.Add(new Sensor(int.Parse(data[0]), int.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3])));
            }
        }
        public string SensorResults()
        {
            long total = 0;
            minX = sensors.Min(s => s.X - s.Diff);
            maxX = sensors.Max(s => s.X + s.Diff);


            //Part 1
            //for (int i = minX; i <= maxX; i++)
            //{
            //    bool foundBeacon = false;
            //    foreach (var s in sensors)
            //    {
            //        if (s.BeaconX == i && s.BeaconY == SolveYRow)
            //        {
            //            foundBeacon = true;
            //            break;
            //        }
            //    }
            //    if (foundBeacon) { continue; }
            //    foreach (var s in sensors)
            //    {
            //        if (i >= s.MinXAtY(SolveYRow) && i <= s.MaxXAtY(SolveYRow))
            //        {
            //            total++;
            //            break;
            //        }
            //    }
            //}

            //Part 2 - found code to handle this logic.  I couldn't do the math.
            for (var y = 0; y <= MaxXYValues; y++)
            {
                var bounds = sensors.Select(s => new int[] { Math.Max(s.MinXAtY(y), 0), Math.Min(s.MaxXAtY(y), MaxXYValues) })
                    .Where(e => e[0] <= e[1]).ToList();

                bounds.Sort((a, b) => a[0].CompareTo(b[0]));
                var isMerged = true;
                while (isMerged && bounds.Count > 1)
                {
                    isMerged = false;
                    if (bounds[0][0] <= bounds[1][0] && bounds[0][1] >= bounds[1][0])
                    {
                        bounds[0][1] = Math.Max(bounds[0][1], bounds[1][1]);
                        bounds.RemoveAt(1);
                        isMerged = true;
                    }
                }

                if (!isMerged || bounds[0][0] != 0 || bounds[0][1] != MaxXYValues)
                {
                    total = ((long)(bounds[0][1] + 1)) * MaxXYValues + y;
                    break;
                }
            }

            results += String.Format("Total: {0}\r\n", total);
            return results;
        }

        public class Sensor
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int BeaconX { get; set; }
            public int BeaconY { get; set; }

            public int Diff { get; set; }

            public Sensor(int x, int y, int beaconX, int beaconY)
            {
                X = x;
                Y = y;
                BeaconX = beaconX;
                BeaconY = beaconY;

                Diff = Math.Abs(x - beaconX) + Math.Abs(y - beaconY);
            }

            public int MinXAtY(int y) => X - Diff + Math.Abs(Y - y);
            public int MaxXAtY(int y) => X + Diff - Math.Abs(Y - y);
        }
    }
}
