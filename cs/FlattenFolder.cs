namespace Utils.UnityAssets
{
    /// <summary>Move all files inside subfolders into the working directory</summary>
    public static class FlattenFolder
    {
        public static bool Process()
        {
            Console.WriteLine($"!!! Type \"YES\" to flatten folder \"{UnityAssetsUtils.WorkingDirectory}\" !!!");
            Console.WriteLine($"[IMPORTANT] This operation is NOT reversible!");

            Console.Write("Confirm: ");
            bool confirm = Console.ReadLine().Trim().Equals("YES");

            if (!confirm)
            {
                Console.WriteLine("Operation Canceled...");
                UnityAssetsUtils.Pause();
                return false;
            }

            var files = Directory.EnumerateFiles(UnityAssetsUtils.WorkingDirectory, UnityAssetsUtils.IsAlt ? "__data" : "*", SearchOption.AllDirectories);

            Parallel.ForEach(files, (file) =>
                File.Move(file, Path.Combine(UnityAssetsUtils.WorkingDirectory, UnityAssetsUtils.IsAlt ? $"__{CommonFuncs.GetFolder(file)}" : Path.GetFileName(file)))
            );

            var folders = Directory.EnumerateDirectories(UnityAssetsUtils.WorkingDirectory, "*", SearchOption.AllDirectories);
            Parallel.ForEach(folders, Directory.Delete);

            Console.WriteLine("Folder Flattened!");
            UnityAssetsUtils.Pause();
            return true;
        }
    }
}
