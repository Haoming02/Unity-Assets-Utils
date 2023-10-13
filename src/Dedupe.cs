namespace Utils.UnityAssets
{
    /// <summary>
    /// Delete files already found in another cache Folder
    /// </summary>
    public static class Dedupe
    {
        private static string subfolder;

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
            CreateFolder();

            var files = Directory.GetFiles(UnityAssetsUtils.WorkingDirectory);
            int l = files.Length;

            Task[] tasks = new Task[l];

            for (int index = 0; index < l; index++)
            {
                int i = index;

                tasks[index] = Task.Run(() =>
                {
                    if (cache.Contains(Path.GetFileName(files[i])))
                        File.Move(files[i], Path.Combine(subfolder, Path.GetFileName(files[i])));
                });
            }

            await Task.WhenAll(tasks);

            Console.WriteLine("Duplicate Files Identified!");
            UnityAssetsUtils.StopOperation();
            UnityAssetsUtils.Pause();
        }

        private static void CacheReferenceFiles(string cacheDirectory, out List<string> cache)
        {
            cache = new List<string>();
            var files = Directory.GetFiles(cacheDirectory);

            foreach (string file in files)
                cache.Add(Path.GetFileName(file));
        }

        private static void CreateFolder()
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

                string input = Console.ReadLine()?.Trim();

                if (input.Length > UnityAssetsUtils.SAFE_GUARD && Directory.Exists(input))
                    return input;
                else if (input.ToLower().Contains("return"))
                    return null;
                else
                {
                    Console.WriteLine("Invalid Path...");
                    UnityAssetsUtils.Pause();
                    Console.Clear();
                }
            } while (true);
        }
    }
}
