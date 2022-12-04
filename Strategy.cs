namespace Advent2022
{
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

}
