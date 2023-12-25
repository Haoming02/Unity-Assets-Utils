namespace Utils.UnityAssets
{
    /// <summary>
    /// Move files that already exist in a cache Folder
    /// </summary>
    public static class Dedupe
    {
        public static async Task Process()
        {
            string cacheDirectory = SetCacheDirectory();

            if (cacheDirectory == null)
            {
                Console.WriteLine("Operation Canceled...");
                UnityAssetsUtils.Pause();
                return;
            }

            if (cacheDirectory.Equals(UnityAssetsUtils.WorkingDirectory))
            {
                Console.WriteLine("Bruh?");
                UnityAssetsUtils.Pause();
                return;
            }

            UnityAssetsUtils.StartOperation();

            CacheReferenceFiles(cacheDirectory, out var cache);
            CreateFolder(out var subfolder);

            var files = Directory.GetFiles(UnityAssetsUtils.WorkingDirectory);
            int l = files.Length;

            Task[] tasks = new Task[l];

            for (int index = 0; index < l; index++)
            {
                int i = index;

                tasks[index] = Task.Run(() =>
                {
                    string filename = Path.GetFileName(files[i]);
                    if (cache.Contains(filename))
                        File.Move(files[i], Path.Combine(subfolder, filename));
                });
            }

            await Task.WhenAll(tasks);

            Console.WriteLine("Duplicate Files Identified!");
            UnityAssetsUtils.StopOperation();
            UnityAssetsUtils.Pause();
        }

        private static void CacheReferenceFiles(string cacheDirectory, out HashSet<string> cache)
        {
            cache = new HashSet<string>();
            var files = Directory.EnumerateFiles(cacheDirectory);

            foreach (string file in files)
                cache.Add(Path.GetFileName(file));
        }

        private static void CreateFolder(out string subfolder)
        {
            subfolder = Path.Combine(UnityAssetsUtils.WorkingDirectory, "duplicated");
            if (!Directory.Exists(subfolder))
                Directory.CreateDirectory(subfolder);
        }

        private static string SetCacheDirectory()
        {
            do
            {
                Console.Write("Enter the Path to Assets (Enter \"return\" to Cancel): ");

                string input = Console.ReadLine()?.Trim('"').Trim();

                if (input.Length > UnityAssetsUtils.SAFE_GUARD && Directory.Exists(input))
                    return input;
                else if (input.ToLower().Equals("return"))
                    return null;

                Console.WriteLine("Invalid Path...");
                UnityAssetsUtils.Pause();
                Console.Clear();
            } while (true);
        }
    }
}
