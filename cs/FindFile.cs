namespace Utils.UnityAssets
{
    /// <summary>List out all files and folders that contain the specified filter</summary>
    public static class FindFile
    {
        public static void Process()
        {
            if (!UnityAssetsUtils.IsSilent)
                Console.WriteLine("\n=== Finding Files/Folders ===");

            Console.Write("\nEnter Filter: ");
            string input = Console.ReadLine().Trim();

            Console.Write("Recursive [y/N]: ");
            bool recursive = !Console.ReadLine().Trim().Equals("N");
            var option = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            var files = Directory.EnumerateFiles(UnityAssetsUtils.WorkingDirectory, $"*{input}*", option);
            bool fileMatched = files.Any();
            var directories = Directory.EnumerateDirectories(UnityAssetsUtils.WorkingDirectory, $"*{input}*", option);
            bool folderMatched = directories.Any();

            if (fileMatched)
            {
                Console.WriteLine($"\n[File Name]");
                foreach (var file in files)
                    Console.WriteLine(file);
            }

            if (folderMatched)
            {
                Console.WriteLine($"\n[Folder Name]");
                foreach (var dir in directories)
                    Console.WriteLine(dir);
            }

            if (!(fileMatched || folderMatched))
                Console.WriteLine("\nNo result was found...");

            Console.WriteLine(string.Empty);
            UnityAssetsUtils.Pause(true);
        }
    }
}
