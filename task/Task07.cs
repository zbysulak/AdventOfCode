// Day 7: No Space Left On Device

public class Task07 : ITask
{
    private class Directory
    {
        public Directory(string input) : this(input, null)
        {
        }

        public Directory(string input, Directory parent)
        {
            ParentDirectory = parent;
            Name = input.Split(' ')[1];
            Files = new List<File>();
            Directories = new List<Directory>();
        }

        public string Name { get; set; }
        public Directory ParentDirectory { get; set; }
        public IList<Directory> Directories { get; set; }
        public IList<File> Files { get; set; }

        public long Sum(long limit)
        {
            var mySize = Size();
            return (mySize <= limit ? mySize : 0) + Directories.Sum(d => d.Sum(limit));
        }

        public IEnumerable<long> GetSizeOfAllDirectories()
        {
            var list = new List<long> { Size() };
            var otherDirs = Directories.SelectMany(d => d.GetSizeOfAllDirectories());
            list.AddRange(otherDirs);
            return list;
        }

        private long Size()
        {
            return Files.Sum(f => f.Size) + Directories.Sum(d => d.Size());
        }

        public override string ToString()
        {
            return $"dir {Name}";
        }

        public void MakeTree(int depth = 0)
        {
            Console.WriteLine(Depth(depth) + $"- {Name} - dir ({Size()})");
            foreach (var dir in Directories)
            {
                Console.WriteLine(Depth(depth + 1));
                dir.MakeTree(depth + 1);
            }

            foreach (var file in Files)
            {
                Console.WriteLine(Depth(depth + 1) + $"- {file.Name}({file.Size})");
            }
        }

        private string Depth(int depth)
        {
            var str = "";
            for (int i = 0; i < depth; i++)
            {
                str += "  ";
            }

            return str;
        }
    }

    private class File
    {
        public string Name { get; set; }
        public long Size { get; set; }

        public File(string input)
        {
            var i = input.Split(' ');
            Name = i[1];
            Size = long.Parse(i[0]);
        }
    }

    private Directory CreateFilesystem(string[] lines)
    {
        var root = new Directory(" /");

        var currentDir = root;
        var i = 0;
        while (i < lines.Length)
        {
            if (lines[i].StartsWith("$"))
            {
                // command
                var command = lines[i].Split(' ');
                switch (command[1])
                {
                    case "cd":
                        switch (command[2])
                        {
                            case "/":
                                currentDir = root;
                                break;
                            case "..":
                                currentDir = currentDir.ParentDirectory;
                                break;
                            default:
                                currentDir = currentDir.Directories.Single(d => d.Name == command[2]);
                                break;
                        }

                        break;
                    case "ls":
                        break;
                }
            }
            else if (lines[i].StartsWith("dir "))
            {
                //create dir
                currentDir.Directories.Add(new Directory(lines[i], currentDir));
            }
            else
            {
                //it is file
                currentDir.Files.Add(new File(lines[i]));
            }

            i++;
        }

        return root;
    }

    public void Solve(string[] lines)
    {
        var root = CreateFilesystem(lines);

        root.MakeTree();

        Console.WriteLine(root.Sum(100000));
    }


    public void Solve2(string[] lines)
    {
        var root = CreateFilesystem(lines);
        var totalSpace = 70000000;
        var spaceNeeded = 30000000;
        var sizes = root.GetSizeOfAllDirectories().ToArray();
        Array.Sort(sizes);
        foreach (var item in sizes)
        {
            Console.WriteLine(item);
        }

        var availableSpace = totalSpace - sizes.Max();
        var smallestDirToFreeUpEnoughSpace = sizes.First(s => availableSpace + s > spaceNeeded);
        Console.WriteLine(smallestDirToFreeUpEnoughSpace);
    }
}