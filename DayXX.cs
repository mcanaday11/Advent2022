using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2022
{
    internal class DayXX
    {

        public List<THING> Things = new();
        public string result = "";

        public void GetThingInput(string input) //Day 11
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                string[] fileData = line.Split(" ");
                THING thg = new THING();
                thg.a = fileData[0];
                if (fileData.Length == 2)
                {
                    thg.b = int.Parse(fileData[1]);
                }
                Things.Add(thg);
            }
        }
        public string ThingResults()
        {
            int total = 0;

            foreach (THING x in Things)
            {
                result += String.Format("First: {0} Second: {1}\r\n", x.a, x.b);
                total++;
            }
            result += String.Format("Total: {0}\r\n", total);
            return result;
        }

        public class THING
        {
            public string a { get; set; }
            public int b { get; set; }
        }

    }
}
