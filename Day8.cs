using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2022
{
    public class Day8
    {
        public int[,] ForestArray;

        public void GetForestInput(string input) //Day 8
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            int TotalRows = lines.Length;
            int TotalColumns = lines[0].Length;

            ForestArray = new int[TotalRows, TotalColumns];
            int row = 0;

            foreach (string line in lines)
            {
                char[] charArr = line.ToCharArray();

                for (int c = 0; c < charArr.Length; c++)
                {
                    int Height = charArr[c] - '0'; //weird but this is how to convert a single numeric char to integer
                    ForestArray[row, c] = Height;
                }
                row++;
            }
        }
        public string ReportForestResults() //Day 8
        {
            string result = "";
            int TreeCount = 0;
            int TopScenicValue = 0;
            for (int r = 0; r < ForestArray.GetLength(0); r++)
            {
                for (int c = 0; c < ForestArray.GetLength(1); c++)
                {
                    if (IsVisible(r, c)) { TreeCount++; }

                    int currValue = CalcScenic(r, c);
                    result += String.Format("Scenic Value for [{0},{1}] {2}\r\n", r, c, currValue);
                    if (currValue > TopScenicValue) { TopScenicValue = currValue; }
                }
            }

            result += String.Format("Number of visible trees {0}\r\n", TreeCount);
            result += String.Format("Top Scenic Value {0}\r\n", TopScenicValue);
            return result;
        }

        private bool IsVisible(int row, int col) //Day 8
        {
            if (row == 0 || row == ForestArray.GetLength(0) - 1) { return true; } //Get edges
            if (col == 0 || col == ForestArray.GetLength(1) - 1) { return true; } //Get edges

            int myValue = ForestArray[row, col];
            if (VisibleLeft(row, col, myValue) ||
                    VisibleRight(row, col, myValue) ||
                    VisibleTop(row, col, myValue) ||
                    VisibleBottom(row, col, myValue)) { return true; }
            return false;
        }

        private int CalcScenic(int row, int col) //Day 8
        {
            int myValue = ForestArray[row, col];

            int value = 0;
            value = ScoreLeft(row, col, myValue) *
                ScoreRight(row, col, myValue) *
                ScoreTop(row, col, myValue) *
                ScoreBottom(row, col, myValue);
            return value;
        }

        private bool VisibleLeft(int row, int col, int myValue) //Day 8
        {
            for (int i = 0; i < col; i++)
            {
                if (myValue <= ForestArray[row, i]) { return false; }
            }
            return true;
        }
        private bool VisibleRight(int row, int col, int myValue) //Day 8
        {
            for (int i = col + 1; i < ForestArray.GetLength(1); i++)
            {
                if (myValue <= ForestArray[row, i]) { return false; }
            }
            return true;
        }
        private bool VisibleTop(int row, int col, int myValue) //Day 8
        {
            for (int i = 0; i < row; i++)
            {
                if (myValue <= ForestArray[i, col]) { return false; }
            }
            return true;
        }
        private bool VisibleBottom(int row, int col, int myValue) //Day 8
        {
            for (int i = row + 1; i < ForestArray.GetLength(0); i++)
            {
                if (myValue <= ForestArray[i, col]) { return false; }
            }
            return true;
        }

        private int ScoreLeft(int row, int col, int myValue) //Day 8
        {
            int num = 0;
            for (int i = col - 1; i > -1; i--)
            {
                if (myValue > ForestArray[row, i])
                {
                    num++;
                }
                else if (myValue <= ForestArray[row, i])
                {
                    num++;
                    return num;
                }
            }
            return num;
        }
        private int ScoreRight(int row, int col, int myValue) //Day 8
        {
            int num = 0;
            for (int i = col + 1; i < ForestArray.GetLength(1); i++)
            {
                if (myValue > ForestArray[row, i])
                {
                    num++;
                }
                else if (myValue <= ForestArray[row, i])
                {
                    num++;
                    return num;
                }
            }
            return num;
        }
        private int ScoreTop(int row, int col, int myValue) //Day 8
        {
            int num = 0;
            for (int i = row - 1; i > -1; i--)
            {
                if (myValue > ForestArray[i, col])
                {
                    num++;
                }
                else if (myValue <= ForestArray[i, col])
                {
                    num++;
                    return num;
                }
            }
            return num;
        }
        private int ScoreBottom(int row, int col, int myValue) //Day 8
        {
            int num = 0;
            for (int i = row + 1; i < ForestArray.GetLength(0); i++)
            {
                if (myValue > ForestArray[i, col])
                {
                    num++;
                }
                else if (myValue <= ForestArray[i, col])
                {
                    num++;
                    return num;
                }
            }
            return num;
        }
    }
}
