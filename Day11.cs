using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent2022
{
    internal class Day11
    {

        public List<Monkey> Monkeys = new();
        public string result = "";

        public void GetMonkeyInput(string input) //Day 11
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            int lineNumber = 1;
            for (int i = 0; i < 8;i++)
            {
                Monkey mky = new Monkey();
                mky.Number = i;
                string[] itemData = lines[lineNumber].Substring(18).Split(" ");
                mky.itemList = new();
                foreach (string item in itemData)
                {
                    mky.itemList.Add(int.Parse(item.Replace(",","")));
                }
                mky.Operation = lines[lineNumber + 1].Substring(19).Trim();
                string findTest = Regex.Match(lines[lineNumber + 2], @"\d+").Value;
                string findTrueToss = Regex.Match(lines[lineNumber + 3], @"\d+").Value;
                string findFalseToss = Regex.Match(lines[lineNumber + 4], @"\d+").Value;

                mky.DivTest = int.Parse(findTest);
                mky.TrueToss = int.Parse(findTrueToss);
                mky.FalseToss = int.Parse(findFalseToss);
                Monkeys.Add(mky);
                lineNumber += 7; //go to next monkey
            }
        }

        public string MonkeyResults()
        {
            int mod = 1;
            foreach (Monkey monkey in Monkeys)
            {
                mod *= monkey.DivTest;  //Need to build the modulus for all the DivTests
            }

            List<int> CheckRounds= new List<int>() {1, 20, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000 };
            for (int round = 1; round < 10001; round++)
            {
                foreach (Monkey mky in Monkeys)
                {
                    //result += String.Format("Monkey{0} - Test: {1} - Operation: {2} - TrueToss: {3} - FalseToss {4} - List: ", mky.Number, mky.DivTest, mky.Operation, mky.TrueToss, mky.FalseToss);
                    foreach (int item in mky.itemList.ToList())  //Need ToList to avoid removing from the list I am iterating
                    {
                        //result += String.Format("{0} ", item);

                        Int64 itemWorry = CalcWorry(mky.Operation, item, mod);
                        mky.Inspections++;
                        int newMonkey = CalcMonkeyToss(itemWorry, mky.DivTest, mky.TrueToss, mky.FalseToss);
                        mky.itemList.Remove(item); //remove from current item list
                        Monkeys[newMonkey].itemList.Add(itemWorry); //add to this new monkey's item list
                    }
                    //result += String.Format("\r\n  Inspections: {1}\r\n", mky.Number, mky.Inspections);
                    //MonkeyInspections.Add(mky.Inspections);
                }

                if (CheckRounds.Contains(round)) {
                    foreach (Monkey mky in Monkeys)
                    {
                        result += String.Format("*Round {0} - Monkey{1} Inspections: {2}\r\n", round, mky.Number, mky.Inspections);
                        //foreach (int item in mky.itemList.ToList())  //Need ToList to avoid removing from the list I am iterating
                        //{
                        //    result += String.Format("{0} ", item);
                        //}
                        //result += String.Format("\r\n");
                    }
                }
            }
            Int64 total = CalcTotalInspections();
            result += String.Format("Total Monkey Business: {0}\r\n", total);
            return result;
        }
        private Int64 CalcTotalInspections()
        {
            List<Int64> result = new List<Int64>();
            foreach (Monkey mky in Monkeys)
            {
                result.Add(mky.Inspections);
            }
            result.Sort();
            return result[result.Count - 1] * result[result.Count - 2]; //Multiply the 2 largest
        }


        private Int64 CalcWorry(string operation, Int64 item, int mod)
        {
            Int64 worry = item;
            Int64 opValue = item; //Set to this in case it is old + old or old * old
            decimal calcValue;
            string findOpValue = Regex.Match(operation, @"\d+").Value;
            if (!string.IsNullOrEmpty(findOpValue))
            {
                opValue = Int64.Parse(findOpValue);
            }

            if (operation.Contains("+")) //Add
            {
                calcValue = (worry + opValue); // / 3;
            } else //Multiply
            {

                calcValue = (worry * opValue); // / 3;
            }
            Int64 result =  (Int64)Math.Floor(calcValue);
            result %= mod;  //Mod the result to get a smaller number to work with
            return result;
        }
        private int CalcMonkeyToss(Int64 itemWorry, int divTest, int trueToss, int falseToss )
        {
            if (itemWorry % divTest == 0) //Divisible - True
            {
                return trueToss;
            } else //Not divisible - False
            {
                return falseToss;
            }
        }

        public class Monkey
        {
            public int Number { get; set; }
            public List<Int64> itemList { get; set; }
            public int Inspections { get; set; }
            public string Operation { get; set; }
            public int DivTest { get; set; }
            public int TrueToss { get; set; }
            public int FalseToss { get; set;}
        }

    }
}
