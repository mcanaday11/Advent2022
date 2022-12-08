namespace Advent2022
{
    public class Day7
    {
        public DirectoryInfo? RootDirectory;
        public void GetDirectoryInfo(string input) //Day 7
        {
            //Build the Root first
            RootDirectory = new DirectoryInfo();
            RootDirectory.Parent = null;
            RootDirectory.DirectoryName = "/";

            DirectoryInfo currentLevel = new DirectoryInfo();

            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                if (line.StartsWith("$"))
                {
                    //Command
                    string cmd = line.Substring(2, 2);
                    if (cmd == "cd") //can ignore $ ls
                    {
                        string info = line[5..];
                        switch (info)
                        {
                            case "..":
                                currentLevel = currentLevel.Parent; //move up a level
                                break;
                            case "/":
                                currentLevel = RootDirectory;
                                break;
                            default:
                                var foundChild = currentLevel.Child.Find(i => i.DirectoryName == info);
                                if (foundChild != null)
                                {
                                    currentLevel = foundChild;
                                }
                                else
                                {
                                    DirectoryInfo dirInfo = new DirectoryInfo();
                                    dirInfo.Parent = currentLevel;
                                    dirInfo.DirectoryName = info;

                                    currentLevel.Child.Add(dirInfo); //Add the directory - should not happen
                                    currentLevel = dirInfo; //set current level to this new directory
                                }
                                break;
                        }
                    }
                }
                else if (line.StartsWith("dir"))
                {
                    //Directory
                    DirectoryInfo dirInfo = new DirectoryInfo();
                    dirInfo.Parent = currentLevel;
                    dirInfo.DirectoryName = line[4..];

                    if (!currentLevel.Child.Contains(dirInfo))
                    {
                        currentLevel.Child.Add(dirInfo);
                    }
                }
                else
                {
                    //File
                    string[] fileData = line.Split(" ");
                    currentLevel.AddFile(new FileInfo(fileData[1], int.Parse(fileData[0])));
                }
            }
        }

        private List<DirectoryInfo> DirectoryList = new(); //Day 7
        private List<DirectoryInfo> AllDirectoryList = new();
        private int TOTAL_SIZE = 70000000;
        private int SIZE_NEEDED = 30000000;

        private int UnusedSpace() //Day 7
        {
            return TOTAL_SIZE - RootDirectory.GetDirectorySize();
        }
        private int MinSizeNeeded() //Day 7
        {
            return SIZE_NEEDED - UnusedSpace();
        }

        private int SmallestThatWorks() //Day 7
        {
            GetAllDirectories(RootDirectory);
            var ordered = AllDirectoryList.OrderBy(o => o.GetDirectorySize());
            foreach (DirectoryInfo dirInfo in ordered)
            {
                if (dirInfo.GetDirectorySize() >= MinSizeNeeded())
                {
                    return dirInfo.GetDirectorySize();
                }
            }
            return -1; //None found
        }
        public string DirectoryResults() //Day 7
        {
            GetValidDirectories(RootDirectory); //Find all the valid directories

            string result = "";
            int total = 0;
            foreach (DirectoryInfo valid in DirectoryList)
            {
                result += String.Format("[Directory {0}] - Size: {1}\r\n", valid.DirectoryName, valid.GetDirectorySize());
                total += valid.GetDirectorySize();

            }
            result += String.Format("Sum of valid sizes {0}\r\n", total);
            result += String.Format("Unused Space {0}\r\n", UnusedSpace());
            result += String.Format("Size needed {0}\r\n", MinSizeNeeded());
            result += String.Format("Smallest that meets the requirement {0}\r\n", SmallestThatWorks());
            return result;
        }

        private void GetValidDirectories(DirectoryInfo child) //Day 7
        {
            foreach (DirectoryInfo dirInfo in child.Child)
            {
                if (dirInfo.ValidSize() > 0)
                {
                    DirectoryList.Add(dirInfo);
                }
                GetValidDirectories(dirInfo);
            }
        }
        private void GetAllDirectories(DirectoryInfo child) //Day 7
        {
            if (!AllDirectoryList.Contains(child)) { AllDirectoryList.Add(child); }
            foreach (DirectoryInfo dirInfo in child.Child)
            {
                AllDirectoryList.Add(dirInfo);
                GetAllDirectories(dirInfo);
            }
        }

        public class DirectoryInfo //Day 7
        {
            public DirectoryInfo Parent { get; set; }
            public List<DirectoryInfo> Child { get; set; } = new();
            private List<FileInfo> Files { get; set; } = new();
            public string DirectoryName { get; set; }
            private int _directorySize;

            public int ValidSize()
            {
                if (GetDirectorySize() <= 100000)
                {
                    return GetDirectorySize();
                }
                else
                {
                    return 0;
                }
            }

            public int GetDirectorySize()
            {
                //recursive
                return Files.Sum(i => i.FileSize) + Child.Sum(x => x.GetDirectorySize());
            }

            public void AddFile(FileInfo file)
            {
                Files.Add(file);
            }
        }

        public class FileInfo
        {
            public FileInfo(string fileName, int fileSize)
            {
                FileName = fileName;
                FileSize = fileSize;
            }

            public string FileName { get; set; }
            public int FileSize { get; set; }
        }
    }
}
