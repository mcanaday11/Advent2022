using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2022
{
    internal class Day9
    {
        public List<Moves> MoveList = new();

        public void GetPathInput(string input) //Day 9
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                string[] fileData = line.Split(" ");
                Moves move = new Moves();
                move.Direction = char.Parse(fileData[0]);
                move.Distance = int.Parse(fileData[1]);
                MoveList.Add(move);
            }
        }

        public string MoveResults()
        {
            Point start = new Point(0, 0); //Starting location
            Snake snake = new Snake(start, start, new List<Point>() { start });

            string result = "";
            foreach (var move in MoveList)
            {
                snake.MoveHead(move.Direction, move.Distance);
                result += String.Format("Head: {0},{1}\r\n", snake.head.X, snake.head.Y);
                for (int i = 0; i < 9; i++)
                {
                    result += String.Format("Tail{0}: {1},{2}\r\n", i + 1, snake.tail[i].X, snake.tail[i].Y);
                }
                result += String.Format("Places visited: {0}\r\n", snake.placesVisited.Count());
            }
            return result;
        }

        public class Moves
        {
            public char Direction { get; set; }
            public int Distance { get; set; }
        }
        public class Snake
        {
            public Snake(Point head, Point tail, List<Point> placesVisited)
            {
                this.head = head;
                this.tail = new Point[9] { tail, tail, tail, tail, tail, tail, tail, tail, tail };
                this.placesVisited = placesVisited;
            }

            public Point head { get; set; }
            public Point[] tail { get; set; }
            //public Point previous { get; set; } 
            public List<Point> placesVisited { get; set; }

            public void MoveHead(char direction, int distance)
            {
                for (int i = 0; i < distance; i++)
                {
                    switch (direction)
                    {
                        case 'U':
                            head = new Point(head.X, head.Y - 1);
                            break;
                        case 'D':
                            head = new Point(head.X, head.Y + 1);
                            break;
                        case 'L':
                            head = new Point(head.X - 1, head.Y);
                            break;
                        case 'R':
                            head = new Point(head.X + 1, head.Y);
                            break;
                    }

                    Point currentHead = head;
                    for (int j = 0; j < tail.Length; j++)
                    {
                        MoveTail(currentHead, j);
                        currentHead = tail[j];
                        if (j == tail.Length - 1)
                        {
                            CheckUnqiueVisits(tail[j]);
                        }
                    }
                }
            }

            private void MoveTail(Point head, int tailIndex)
            {
                int diffX = Math.Abs(head.X - tail[tailIndex].X);
                int diffY = Math.Abs(head.Y - tail[tailIndex].Y);

                if (diffX < 2 && diffY < 2) return; //same space or adjacent, don't move
                Point myTail = tail[tailIndex];

                char direction = FindDirection(head, myTail);
                if (diffY == 0 && diffX == 2) //moved columns
                {
                    if (direction == 'R') //moved right
                    {
                        tail[tailIndex] = new Point(myTail.X + 1, myTail.Y);
                    } else if (direction == 'L') //moved left
                    {
                        tail[tailIndex] = new Point(myTail.X - 1, myTail.Y);
                    } 
                } else if (diffX == 0 && diffY == 2) //moved rows
                {
                    if (direction == 'U') //moved Up
                    {
                        tail[tailIndex] = new Point(myTail.X, myTail.Y - 1);
                    }
                    else if (direction == 'D') //moved Down
                    {
                        tail[tailIndex] = new Point(myTail.X, myTail.Y + 1);
                    }
                } else //Need to move diagonal
                {
                    //Need to move diagonal toward the head
                    if (head.X > myTail.X && head.Y > myTail.Y) //lower right
                    {
                        tail[tailIndex] = new Point(myTail.X + 1, myTail.Y + 1);
                    } else if (head.X > myTail.X && head.Y < myTail.Y) //upper right
                    {
                        tail[tailIndex] = new Point(myTail.X + 1, myTail.Y - 1);
                    }
                    else if (head.X < myTail.X && head.Y < myTail.Y) //upper left
                    {
                        tail[tailIndex] = new Point(myTail.X - 1, myTail.Y - 1);
                    }
                    else if (head.X < myTail.X && head.Y > myTail.Y) //lower left
                    {
                        tail[tailIndex] = new Point(myTail.X - 1, myTail.Y + 1);
                    }
                }
            }
            private char FindDirection(Point head, Point tail)
            {
                if (head.X - tail.X == -2) { return 'L'; } //Moved left on X
                if (head.X - tail.X == 2) { return 'R'; } //Moved right on X
                if (head.Y - tail.Y == -2) { return 'U'; } //Moved up on Y
                if (head.Y - tail.Y == 2) { return 'D'; } //Moved down on Y
                return ' '; //cannot happen
            }

            private void CheckUnqiueVisits(Point tail)
            {
                if (!placesVisited.Contains(tail)) //Add to the list of unique locations
                {
                    placesVisited.Add(tail);
                }

            }


        }

    }
}
