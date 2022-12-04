namespace Advent2022
{
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

}
