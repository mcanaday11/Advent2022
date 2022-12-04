using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Advent2022
{
    public partial class frmAdvent2022 : Form
    {
        public frmAdvent2022()
        {
            InitializeComponent();
        }

        public List<Elf>? Elves;
        public List<Strategy>? Strategies;
        public List<Rucksack> Rucksacks;
        public List<WorkPair> WorkPairs;
        private string result = "";

        private void btnProcess_Click(object sender, EventArgs e)
        {
            txtResult.Clear();
            result = "";
            string input = txtInput.Text;

            //Day 1
            //Elves = GetListofElves(input);
            //ReportMaxCalories();

            //Day 2
            //Strategies = GetStrategyGuide(input);
            //TournamentResults();

            //Day 3
            //Rucksacks = GetRucksacks(input);
            //RucksackResults();

            //Day 4
            WorkPairs = GetWorkPairs(input);
            WorkPairResults();
        }

        private void ReportMaxCalories() //Day 1
        {
            List<Elf> orderedElves = Elves.OrderByDescending(o => o.Calories).ToList();
            
            int bigWeight = 0;
            int counter = 1;
            foreach (Elf elve in orderedElves)
            {

                result += String.Format("[{0}] - {1}\r\n", elve.ElfNumber, elve.Calories);
                bigWeight += elve.Calories;
                counter++;
                if (counter > 3)
                {
                    result += String.Format("Total Calories for Max 3: {0}\r\n", bigWeight);
                    break;
                }
            }
            txtResult.Text = result;
        }

        private List<Elf> GetListofElves(string input) //Day 1
        {
            List<Elf> list = new List<Elf>();

            int calories = 0;
            int elfNum = 1;
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    //End of elf, count calories, reset for next elf
                    Elf elfo = new Elf();
                    elfo.Calories = calories;
                    elfo.ElfNumber = elfNum;
                    list.Add(elfo);
                    elfNum++;
                    calories = 0;
                } else
                {
                    calories += int.Parse(line); //Add calories
                }
            }
            if (calories > 0) //Need to deal with the last elf
            {
                Elf elfo = new Elf();
                elfo.Calories = calories;
                elfo.ElfNumber = elfNum;
                list.Add(elfo);
            }
            return list;
        }

        private List<Strategy> GetStrategyGuide(string input) //Day 2
        {
            List<Strategy> list = new List<Strategy>();
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                Strategy strat = new Strategy();
                strat.Opponent = Action(line[0]);
                strat.RequiredResult = MatchResult(line[2]);
                strat.Response = NeededResult(strat.RequiredResult, strat.Opponent);

                list.Add(strat);
            }
            return list;
        }

        //private List<Rucksack> GetRucksacks(string input) //Day 3-1
        //{
        //    List<Rucksack> list = new List<Rucksack>();
        //    string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        //    foreach (string line in lines)
        //    {
        //        Rucksack ruck = new Rucksack();
        //        int mid = line.Length /2;

        //        ruck.Left = line[..mid];
        //        ruck.Right = line[mid++..];
        //        list.Add(ruck);
        //    }
        //    return list;
        //}
        private List<Rucksack> GetRucksacks(string input) //Day 3-2
        {
            List<Rucksack> list = new List<Rucksack>();
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            int rowcount = 1;
            List<string> tempList= new List<string>();

            foreach (string line in lines)
            {
                tempList.Add(line);
                if (tempList.Count() == 3)
                {
                    Rucksack ruck = new Rucksack();
                    ruck.First = tempList[0];
                    ruck.Second = tempList[1];
                    ruck.Third = tempList[2];
                    list.Add(ruck); 
                    tempList.Clear();
                }
            }
            return list;
        }
        private List<WorkPair> GetWorkPairs(string input) //Day 4
        {
            List<WorkPair> list = new List<WorkPair>();
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                string[] pairs = line.Split(",");
                WorkPair wp = new WorkPair();
                wp.First = pairs[0];
                wp.Second = pairs[1];
                wp.SetRanges();
                list.Add(wp);
            }
            return list;
        }



        private RPS Action(char input) //Day 2
        {
            if (input == 'A')
            {
                return RPS.Rock;
            } else if (input == 'B')
            {
                return RPS.Paper;
            } else if (input == 'C')
            {
                return RPS.Scissors;
            }
            return RPS.None;
        }
        private WLD MatchResult(char input) //Day 2
        {

            //return input switch
            //{
            //    'X' => 
            //}

            if (input == 'X')
            {
                return WLD.Loss;
            }
            else if (input == 'Y')
            {
                return WLD.Draw;
            }
            return WLD.Win;
        }

        private RPS NeededResult(WLD input, RPS opponent) //Day 2
        {
            switch (input)
            {
                case WLD.Loss: //Must lose
                    if (opponent == RPS.Rock)
                    {
                        return RPS.Scissors;
                    } else if(opponent == RPS.Paper)
                    {
                        return RPS.Rock;
                    } 
                    return RPS.Paper; 
                case WLD.Draw: //Must Draw
                    if (opponent == RPS.Rock)
                    {
                        return RPS.Rock;
                    }
                    else if (opponent == RPS.Paper)
                    {
                        return RPS.Paper;
                    }
                    return RPS.Scissors;
                case WLD.Win: //Must Win
                    if (opponent == RPS.Rock)
                    {
                        return RPS.Paper;
                    }
                    else if (opponent == RPS.Paper)
                    {
                        return RPS.Scissors;
                    }
                    return RPS.Rock;
            }
            return RPS.None;
        }

        private void TournamentResults() //Day 2
        {
            int totalScore = 0;

            foreach (Strategy strat in Strategies)
            {
                result += String.Format("Needed Result: {0} [{1} {2} - {3}] --> Score {4}\r\n", strat.RequiredResult, strat.Opponent, strat.Response, strat.Result, strat.Score);
                totalScore += strat.Score;
            }
            result += String.Format("Total Score: {0}\r\n", totalScore);
            txtResult.Text = result;
        }

        private void RucksackResults() //Day 3
        {
            int totalValue = 0;

            foreach (Rucksack ruck in Rucksacks)
            {
                result += String.Format("First: {0} Second: {1} Third: {2} Match: {3} Value: {4}\r\n", ruck.First, ruck.Second, ruck.Third, ruck.Match, ruck.Value);
                totalValue += ruck.Value;
            }
            result += String.Format("Total Value: {0}\r\n", totalValue);
            txtResult.Text = result;
        }

        private void WorkPairResults() //Day 4
        {
            int totalPairs = 0;

            foreach (WorkPair wp in WorkPairs)
            {
                result += String.Format("First: {0} Second: {1} Full Overlap?: {2} Any Overlap? {3}\r\n", wp.First, wp.Second, wp.FullOverlap, wp.AnyOverlap);
                //if (wp.FullOverlap) { totalPairs++; }
                if (wp.AnyOverlap) { totalPairs++; }
            }
            result += String.Format("Total Pairs: {0}\r\n", totalPairs);
            txtResult.Text = result;
        }

    }

    public enum RPS //Day 2
    {
        None = 0,
        Rock = 1,
        Paper = 2,
        Scissors = 3
    }

    public enum WLD //Day 2
    {
        Win = 6,
        Loss = 0,
        Draw = 3
    }

    public class Strategy //Day 2
    {
        public RPS Opponent { get; set; }  
        public RPS Response { get; set; }  

        public WLD RequiredResult { get; set; } 
        public WLD Result
        {
            get
            {
                return GetResult();
            }
        }

        public int Score
        {
            get { return CalcScore(); }
        }

        private WLD GetResult()
        {
            if (Opponent == RPS.Rock)
            {
                switch (Response)
                {
                    case RPS.Rock:
                        return WLD.Draw;
                    case RPS.Paper:
                        return WLD.Win;
                    case RPS.Scissors:
                        return WLD.Loss;
                }
            }
            if (Opponent == RPS.Paper)
            {
                switch (Response)
                {
                    case RPS.Rock:
                        return WLD.Loss;
                    case RPS.Paper:
                        return WLD.Draw;
                    case RPS.Scissors:
                        return WLD.Win;
                }
            }
            if (Opponent == RPS.Scissors)
            {
                switch (Response)
                {
                    case RPS.Rock:
                        return WLD.Win;
                    case RPS.Paper:
                        return WLD.Loss;
                    case RPS.Scissors:
                        return WLD.Draw;
                }
            }
            return WLD.Loss; //Won't happen
        }

        private int CalcScore()
        {
            return (int)Result + (int)Response;
        }
    }

    public class Elf //Day 1
    {
        public int ElfNumber { get; set; }
        public int Calories { get; set; }
    }

    public class Rucksack //Day 3
    {
        public string? First { get; set; }
        public string? Second { get; set; }
        public string? Third { get; set; }
        public char Match
        {
            get
            {
                var matches = First.Intersect(Second);
                matches= matches.Intersect(Third);
                return matches.FirstOrDefault();
            }
        }
        public int Value { 
            get
            {
                List<alphaValues> valueList = GetValueList();
                return valueList.Find(x => x.theChar == Match).theValue;
            }
        }
        private List<alphaValues> GetValueList()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            List<alphaValues> values = new List<alphaValues>();
            int value = 1;
            foreach (char c in alphabet)
            {
                alphaValues av = new alphaValues
                {
                    theChar = c,
                    theValue = value++
                };
                values.Add(av);
            }
            return values;  
        }

        public class alphaValues
        {
            public char theChar { get; set; }
            public int theValue { get; set; }
        }

    }

    public class WorkPair //Day 4
    {
        public string? First { get; set; }
        public string? Second { get; set; }

        public IEnumerable<int>? FirstRange { get; set; }
        public IEnumerable<int>? SecondRange { get; set; }
        public bool FullOverlap { 
            get
            {
                bool FirstWithinSecond = FirstRange.Min() >= SecondRange.Min() && FirstRange.Max() <= SecondRange.Max();
                bool SecondWithinFirst = SecondRange.Min() >= FirstRange.Min() && SecondRange.Max() <= FirstRange.Max();
                return (FirstWithinSecond || SecondWithinFirst);
            } 
        }
        public bool AnyOverlap
        {
            get
            {
                return FirstRange.Intersect(SecondRange).Count() > 0;
            }
        }


        public void SetRanges()
        {
            int min, max;
            string[] firstRange = First.Split("-");
            min = int.Parse(firstRange[0]);
            max = int.Parse(firstRange[1]);
            FirstRange = getRange(min, max);

            string[] secondRange = Second.Split("-");
            min = int.Parse(secondRange[0]);
            max = int.Parse(secondRange[1]);
            SecondRange = getRange(min, max);
        }

        private IEnumerable<int> getRange(int min, int max)
        {
            return Enumerable.Range(min, max - min + 1);
        }

    }

}
