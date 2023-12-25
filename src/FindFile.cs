namespace Utils.UnityAssets
{
    /// <summary>
    /// List out all filesnames that match the specified filter
    /// </summary>
    public static class FindFile
    {
        public static void Process()
        {
            Console.Write("\nEnter Filter: ");
            string input = Console.ReadLine()?.Trim();

            Console.Write("Recursive [y/n]: ");
            bool recursive = Console.ReadLine()?.Trim() != "n";

            UnityAssetsUtils.StartOperation();

            var files = Directory.EnumerateFiles(UnityAssetsUtils.WorkingDirectory, $"*{input}*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            Console.WriteLine($"\n[File Name]");
            foreach (var file in files)
                Console.WriteLine(file);

            var directories = Directory.EnumerateDirectories(UnityAssetsUtils.WorkingDirectory, $"*{input}*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            Console.WriteLine($"\n[Folder Name]");
            foreach (var dir in directories)
                Console.WriteLine(dir);

            Console.WriteLine(string.Empty);
            UnityAssetsUtils.StopOperation();
            UnityAssetsUtils.Pause(true);
        }
    }
}
