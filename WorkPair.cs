namespace Advent2022
{
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
