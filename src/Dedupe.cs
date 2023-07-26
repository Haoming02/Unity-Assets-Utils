namespace Utils.UnityAssets
{
    static class Dedupe
    {
        public static async Task Process()
        {
            SetCacheDirectory(out string cacheDirectory);

            UnityAssetsUtils.StartOperation();

            CacheReferenceFiles(cacheDirectory, out string[] cache);

            var files = Directory.GetFiles(UnityAssetsUtils.WorkingDirectory);
            int l = files.Length;

            Task[] tasks = new Task[l];

            for (int i = 0; i < l; i++)
            {
                int fileIndex = i;

                tasks[i] = Task.Run(() =>
                {
                    if (cache.Contains(Path.GetFileName(files[fileIndex])))
                        File.Move(files[fileIndex], files[fileIndex] + ".old");
                });
            }

            await Task.WhenAll(tasks);
            Console.WriteLine("Duplicate Files Identified!");
            UnityAssetsUtils.StopOperation();
            UnityAssetsUtils.Pause();
        }

        private static void CacheReferenceFiles(string cacheDirectory, out string[] cache)
        {
            var files = Directory.GetFiles(cacheDirectory);
            int l = files.Length;

            string[] temp = new string[l];

            Parallel.For(0, l, index =>
            {
                temp[index] = Path.GetFileName(files[index]);
            });

            cache = temp;
        }

        private static void SetCacheDirectory(out string cacheDirectory)
        {
            cacheDirectory = string.Empty;

            do
            {
                if (cacheDirectory != string.Empty)
                    Console.WriteLine("Invalid Input!");

                Console.Write("Enter the Path to Cache: ");
                cacheDirectory = Console.ReadLine();

                if (cacheDirectory == null)
                    Environment.Exit(-1);

            } while (!Directory.Exists(cacheDirectory));
        }
    }
}
