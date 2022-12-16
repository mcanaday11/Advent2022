using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


namespace Advent2022
{
    internal class Day16
    {
        List<Valve> AllValves = new(); //The original list.  Cannot alter this because need to check all possible paths.
        public string results = "";

        public void GetScanInput(string input)
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                string cleanInput = line.Replace("Valve ", "").Replace(" has flow rate=", ",").Replace("; tunnels lead to valves ", ",").Replace("; tunnel leads to valve ", ","); //remove the crap
                string[] data = cleanInput.Split(",");
                List<string> tunnels = new();
                for (int i = 2; i < data.Length; i++)
                {
                    if (data[i] != null) { tunnels.Add(data[i].Trim()); }
                }
                AllValves.Add(new Valve(data[0], int.Parse(data[1]), tunnels));
            }
        }

        public string ScanResults()
        {
            Stopwatch sw = Stopwatch.StartNew();

            CalcDistanceToEveryValve(); //Populate the shortest path on each Valve to other valves

            List<Valve> validValves = AllValves.FindAll(r => r.Rate > 0); //start with only valves with a rate > 0

            //Part 1
            //int TotalPressureReleased = GetTotalPressure(30, validValves, "AA", AllValves);

            //Part 2 - less time and do it twice
            int[] timeFor2 = new int[] {26, 26};
            string[] startFor2 = new string[] { "AA","AA" };
            long TotalPressureReleased = GetTotalPressureFor2(timeFor2, validValves, startFor2, AllValves);

            sw.Stop();

            results += String.Format("Total Pressure released: {0}\r\n", TotalPressureReleased);
            results += String.Format("How long it took to calc: {0} seconds\r\n", sw.ElapsedMilliseconds / 1000);

            return results;
        }

        public int GetTotalPressure(int timeLeft, List<Valve> remainingValves, string curValve, List<Valve> valves)
        {
            int best = 0;
            Valve cur = valves.Find(n => n.Name == curValve);
            //results += String.Format("Minutes remaining: {0}, Current Valve: {1}\r\n", timeLeft, curValve);
            foreach (var t in remainingValves)
            {
                if (t.Rate > 0) //Only deal with valves that have a rate > 0
                {
                    int newTimeLeft = timeLeft - cur.distanceToValve[t.Name] - 1; //How long does it take
                    if (newTimeLeft > 0) //more time, keep going
                    {
                        int flowValue = newTimeLeft * t.Rate + GetTotalPressure(newTimeLeft, remainingValves.FindAll(c => c.Name != t.Name), t.Name, valves);
                        //results += String.Format("Total Pressure: {0}\r\n", flowValue);
                        if (flowValue > best) 
                        {
                            best = flowValue; //new best flow value
                            //results += String.Format("New best flow value: {0}\r\n", best);
                        } 
                    }
                }
            }
            return best;
        }
        public long GetTotalPressureFor2(int[] timeLeft, List<Valve> remainingValves, string[] curValve, List<Valve> valves)
        {
            long best = 0;
            int whoGoesNext = timeLeft[0] >= timeLeft[1] ? 0 : 1; //if I have more or same time, I'll go, otherwise elephant goes

            Valve cur = valves.Find(n => n.Name == curValve[whoGoesNext]);
            foreach (var t in remainingValves)
            {
                if (t.Rate > 0) //Only deal with valves that have a rate > 0
                {
                    int newTimeLeft = timeLeft[whoGoesNext] - cur.distanceToValve[t.Name] - 1; //How long does it take
                    if (newTimeLeft > 0) //more time, keep going
                    {
                        int[] newTimes = new int[] { newTimeLeft, timeLeft[1 - whoGoesNext] }; //update time left
                        string[] newValves = new string[] { t.Name, curValve[1 - whoGoesNext] }; //update places to go
                        long flowValue = newTimeLeft * t.Rate + GetTotalPressureFor2(newTimes, remainingValves.FindAll(c => c.Name != t.Name), newValves, valves);
                        if (flowValue > best)
                        {
                            best = flowValue; //new best flow value
                        }
                    }
                }
            }
            return best;
        }

        void CalcDistanceToEveryValve()
        {
            foreach (Valve v in AllValves)
            {
                string valveName = v.Name;
                Valve currentValve = AllValves.Find(n => n.Name == valveName);
                currentValve.distanceToValve[valveName] = 0;

                var visited = new HashSet<string>(); //keep track of visited valves

                while (currentValve != null && visited.Count < AllValves.Count)
                {
                    visited.Add(currentValve.Name);
                    int distance = currentValve.distanceToValve[valveName] + 1;
                    foreach (var t in currentValve.Tunnels)
                    {
                        if (!visited.Contains(t)) //look for new tunnel
                        {
                            var c = AllValves.Find(n => n.Name == t);
                            if (c.distanceToValve.ContainsKey(valveName))
                            {
                                if (distance < c.distanceToValve[valveName]) c.distanceToValve[valveName] = distance;
                            }
                            else c.distanceToValve[valveName] = distance;
                        }
                    }
                    currentValve = AllValves.Where(c => !visited.Contains(c.Name) && c.distanceToValve.ContainsKey(valveName)).OrderBy(c => c.distanceToValve[valveName]).FirstOrDefault();
                }
            }
        }

        public class Valve
        {
            public Valve(string name, int rate, List<string> tunnels)
            {
                Name = name;
                Rate = rate;
                Tunnels = tunnels;
                this.isOpen = false; //All begin closed
                this.distanceToValve = new Dictionary<string, int>();
            }

            public string Name {get;set;}
            public int Rate { get;set;}
            public List<string> Tunnels { get; set; }
            public bool isOpen {get;set;}
            
            public Dictionary<string, int> distanceToValve = new Dictionary<string, int>();
        }
    }
}
