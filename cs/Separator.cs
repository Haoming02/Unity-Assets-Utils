namespace Utils.UnityAssets
{
    /// <summary>Move non-asset files into a subfolder</summary>
    public static class Separator
    {
        private static string CreateFolders()
        {
            string folder = Path.Combine(UnityAssetsUtils.WorkingDirectory, "Non-Asset");
            Directory.CreateDirectory(folder);
            return folder;
        }

        public static void Process()
        {
            if (!UnityAssetsUtils.IsSilent)
                Console.WriteLine("\n=== Separating Assets ===");

            string naFolder = CreateFolders();
            var files = Directory.EnumerateFiles(UnityAssetsUtils.WorkingDirectory, "*", SearchOption.AllDirectories);

            Parallel.ForEach(files, (file) =>
            {
                if (CommonFuncs.VerifyHeader(file))
                    return;

                string relpath = Path.GetRelativePath(UnityAssetsUtils.WorkingDirectory, file);
                string target = Path.Combine(naFolder, relpath);

                if (File.Exists(target)) File.Delete(target);
                Directory.CreateDirectory(Path.GetDirectoryName(target));
                File.Move(file, target);
            });

            if (!Directory.EnumerateFiles(naFolder).Any())
                Directory.Delete(naFolder);

            Console.WriteLine("Files Separated!");
            UnityAssetsUtils.Pause();
        }
    }
}
