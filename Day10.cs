using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2022
{
    internal class Day10
    {
        public string result = "";
        List<Commands> CommandList = new();
        List<int> SpecialCycles = new() { 40, 80, 120, 160, 200, 240 };

        public void GetSignalInput(string input) //Day 10
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                string[] fileData = line.Split(" ");
                Commands cmd = new Commands();
                cmd.Command = fileData[0];
                if (fileData.Length == 2)
                {
                    cmd.Value = int.Parse(fileData[1]);
                }
                CommandList.Add(cmd);
            }
        }

        int SumOfStrength = 0;
        int cycle = 0, register = 1;

        public string SignalResults()
        {
            foreach (Commands cmd in CommandList)
            {
                //result += String.Format("Command: {0} Value: {1}  Register:{2}\r\n", cmd.Command, cmd.Value, register);
                if (cmd.Command == "noop")
                {
                    //Do nothing, single cycle
                    Process1Cycle();
                } else //Add to register
                {
                    Process2Cycles();
                    register += cmd.Value;
                }
                if (cycle > 240) { break; } //Get out after Special 240
            }

            //result += String.Format("Sum of Strengths {0}\r\n", SumOfStrength);
            return result;
        }

        private void Process1Cycle()
        {
            //result += String.Format("Cycle: {0} Register:{1}\r\n", cycle, register);
            WriteSprite();

            if (SpecialCycles.Contains(cycle + 1))
            {
                result += "\r\n"; //New line
                
                //int SignalStrength = cycle * register;
                //SumOfStrength += SignalStrength;
                //result += String.Format("*Special - Register: {0} Signal Strength: {1}  Running Sum of Strength: {2}\r\n", register, SignalStrength, SumOfStrength);
            }
            cycle++;
        }
        private void WriteSprite()
        {
            int checkCycle = GetRowCycle(cycle);
            if (checkCycle ==  register || checkCycle == register - 1 || checkCycle == register + 1) //within the sprite range
            {
                result += "#";
            } else
            {
                result += ".";
            }
        }
        private int GetRowCycle(int cycle)
        {
            switch (cycle)
            {
                case < 40: return cycle;
                case < 80: return cycle - 40;
                case < 120: return cycle - 80;
                case < 160: return cycle - 120;
                case < 200: return cycle - 160;
                case < 240: return cycle - 200;
                default: return cycle; //will not happen
            }

        }


        private void Process2Cycles()
        {
            Process1Cycle();
            Process1Cycle();
        }


        public class Commands
        {
            public string Command { get; set; }
            public int Value { get; set; }
        }

    }
}
