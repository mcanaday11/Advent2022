using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Advent2022.Day9;

namespace Advent2022
{
    public class Day14
    {
        public string results = "";
        char[,]? TheCave;
        List<List<Point>> RockList = new();
        int minX = 501; //need to set the left edge - start where the sand comes in (plus 1 for zero index)
        int maxX = 501; //need to set the right edge - start where the sand comes in (plus 1 for zero index)
        int maxY = 1; //need to set the bottom edge - start where the sand comes in (plus 1 for zero index)
        Point sandStart = new Point(500, 0);
        public void GetCaveInput(string input)
        {

            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                List<Point> RockSpots = new List<Point>();
                string[] rocks = line.Split(" -> ");
                foreach (string rock in rocks)
                {
                    string[] position = rock.Split(",");
                    Point point = new Point();
                    point.X = Convert.ToInt32(position[0]);
                    point.Y = Convert.ToInt32(position[1]);
                    RockSpots.Add(point);
                }
                RockList.Add(RockSpots);
            }

            //Calc size of the cave
            foreach (List<Point> row in RockList)
            {
                if (row.Max(p => p.X) > maxX) { maxX = row.Max(p => p.X); }
                if (row.Max(p => p.Y) > maxY) { maxY = row.Max(p => p.Y); }
                if (row.Min(p => p.X) < minX) { minX = row.Min(p => p.X); }
            }
            maxX++; //deal with zero based
            maxY++; //deal with zero based

            //For part 2
            maxY += 2; //Add 2 to the max Y
            maxX += 300; //not quite infinite, but should be enough
            minX -= 300; //not quite infinite, but should be enough

            TheCave = new char[maxX, maxY]; 

            //Draw the Rocks in the Cave

            foreach (List<Point> rockLine in RockList)
            {
                DrawRocks(rockLine);
            }
            TheCave[sandStart.X, sandStart.Y] = '+'; //Sand start
            FillAir();
        }

        private void DrawRocks(List<Point> rocks)
        {
            Point prev = rocks[0]; //start with first position
            foreach(Point point in rocks)
            {
                if (point.X != prev.X) //Draw horizontal
                {
                    if (point.X < prev.X) //Draw Left
                    {
                        for (int i = prev.X; i >= point.X; i--)
                        {
                            TheCave[i, point.Y] = '#';
                        }
                    }
                    else //Draw Right
                    {
                        for (int i = prev.X; i <= point.X; i++)
                        {
                            TheCave[i, point.Y] = '#';
                        }
                    }
                } else if (point.Y != prev.Y) //Draw vertical
                {
                    if (point.Y < prev.Y) //Draw Up
                    {
                        for (int i = prev.Y; i >= point.Y; i--)
                        {
                            TheCave[point.X, i] = '#';
                        }
                    }
                    else //Draw Down
                    {
                        for (int i = prev.Y; i <= point.Y; i++)
                        {
                            TheCave[point.X, i] = '#';
                        }
                    }
                } else //same location
                {
                    TheCave[point.X, point.Y] = '#';
                }
                prev = point;
            }
        }
        private void FillAir()
        {
            for (int y = 0; y < maxY; y++) //row check
            {
                for (int x = minX; x < maxX; x++) //column check
                {
                    if (TheCave[x, y] == char.MinValue)
                    {
                        TheCave[x, y] = '.'; //air
                    }
                }
            }
        }

        public string SandResults()
        {
            //For part 2
            for (int i = minX; i < maxX; i++) //fill in the floor
            {
                TheCave[i, maxY - 1] = '#';
            }

            int sandTotal = SandFlow();
            DrawCave();
            results += String.Format("Sand Total: {0}\r\n", sandTotal);
            return results;
        }

        private void DrawCave()
        {
            for (int y = 0; y < maxY; y++) //row check
            {
                for (int x = minX; x < maxX; x++) //column check
                {
                    results += TheCave[x, y].ToString();
                }
                results += "\r\n";
            }
        }

        private int SandFlow()
        {
            int totalSand = 0;
            while (true)
            {
                var sandPosition = sandStart; //always start here
                bool? isSandStopped;
                while ((isSandStopped = SandStop(ref sandPosition)) == false)
                {
                    //sand moving
                }

                //Part 1
                //if (!isSandStopped.HasValue)
                //{
                //    // Sand fell into the abyss!
                //    return sandStacked;
                //}

                //Part 2
                if (sandPosition.Y == 0) //Top row - sand at start
                {
                    return ++totalSand;
                }
                else
                {
                    //Sand stops here
                    totalSand += 1;
                    TheCave[sandPosition.X, sandPosition.Y] = 'o';
                }
            }
        }

        public bool? SandStop(ref Point pos)
        {
            if (pos.Y + 1 >= maxY)
            {
                return null; //Fell off
            }

            if (pos.X >= maxX || pos.X < minX)
            {
                return null; //Fell off
            }

            if (TheCave[pos.X, pos.Y + 1] == '.')
            {
                pos.Y++;
                return false; //move down
            }
            else
            {
                if (pos.X - 1 < minX)
                {
                    return null; //Fell off
                }

                if (TheCave[pos.X - 1, pos.Y + 1] == '.')
                {
                    pos.X--;
                    pos.Y++;
                    return false; //move down left
                }

                if (pos.X + 1 >= maxX)
                {
                    return null; //Fell off
                }

                if (TheCave[pos.X + 1, pos.Y + 1] == '.')
                {
                    pos.X++;
                    pos.Y++;
                    return false; //move down right
                }
            }
            return true;
        }
    }
}
