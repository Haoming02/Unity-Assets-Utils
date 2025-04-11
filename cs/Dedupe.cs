namespace Utils.UnityAssets
{
    /// <summary>Move files which already exist in an old cache folder into a subfolder</summary>
    public static class Dedupe
    {
        private static HashSet<string> CacheReferenceFiles(string cacheDirectory)
        {
            var cache = new HashSet<string>();

            var files = Directory.EnumerateFiles(cacheDirectory, "*", SearchOption.AllDirectories);
            foreach (string file in files)
                cache.Add(Path.GetRelativePath(cacheDirectory, file));

            return cache;
        }

        private static string CreateFolder()
        {
            string subfolder = Path.Combine(UnityAssetsUtils.WorkingDirectory, "duplicated");
            Directory.CreateDirectory(subfolder);
            return subfolder;
        }

        public static void Process()
        {
            if (!UnityAssetsUtils.IsSilent)
                Console.WriteLine("\n=== Removing Duplicated Files ===");

            string cacheDirectory = UnityAssetsUtils.SetDirectory();
            if (cacheDirectory.Equals(UnityAssetsUtils.WorkingDirectory))
            {
                Console.WriteLine("Operation Canceled...");
                UnityAssetsUtils.Pause();
                return;
            }

            var cache = CacheReferenceFiles(cacheDirectory);
            string subfolder = CreateFolder();

            var files = Directory.EnumerateFiles(UnityAssetsUtils.WorkingDirectory, "*", SearchOption.AllDirectories);

            Parallel.ForEach(files, (file) =>
            {
                string relpath = Path.GetRelativePath(UnityAssetsUtils.WorkingDirectory, file);
                if (!cache.Contains(relpath))
                    return;

                string target = Path.Combine(subfolder, relpath);
                if (File.Exists(target)) File.Delete(target);

                Directory.CreateDirectory(Path.GetDirectoryName(target));
                File.Move(file, target);
            });

            Console.WriteLine("Duplicate Files Identified!");
            UnityAssetsUtils.Pause();
        }
    }
}
