using System.Text;

namespace Utils.UnityAssets
{
    /// <summary>
    /// Attempt to categorize all assets into different folders
    /// </summary>
    public static class Separator
    {
        private static void CreateFolders(out string folder)
        {
            folder = Path.Combine(UnityAssetsUtils.WorkingDirectory, "Non-Asset");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
        }

        private static bool CheckFolder(string path)
        {
            foreach (var _ in Directory.EnumerateFiles(path))
                return true;

            return false;
        }

        public static void Process()
        {
            if (!UnityAssetsUtils.IsSilent)
                Console.WriteLine("\n=== Separating Assets ===");

            UnityAssetsUtils.StartOperation();

            CreateFolders(out var nonAssetFolder);
            var files = Directory.EnumerateFiles(UnityAssetsUtils.WorkingDirectory);

            foreach (var file in files)
            {
                if (!CommonFuncs.VerifyHeader(file))
                {
                    File.Move(file, Path.Combine(nonAssetFolder, Path.GetFileName(file)));
                }
            }

            if (!CheckFolder(nonAssetFolder))
                Directory.Delete(nonAssetFolder);

            Console.WriteLine("Files Separated!");
            UnityAssetsUtils.StopOperation();
            UnityAssetsUtils.Pause();
        }
    }
}
