namespace Utils.UnityAssets
{
    /// <summary>Remove the leading dummy bytes of asset files</summary>
    public static class ByteTrimmer
    {
        private const int threshold = 2048;

        private static void TrimTask(string path)
        {
            var data = File.ReadAllBytes(path);

            int index1 = CommonFuncs.FindKey(ref data, CommonFuncs.HeaderBytes, 0, threshold);

            if (index1 < 0)
            {
                if (!UnityAssetsUtils.IsSilent)
                    Console.WriteLine($"\tNo Header Found! Skipping \"{Path.GetFileName(path)}\"");
                return;
            }

            int index2 = CommonFuncs.FindKey(ref data, CommonFuncs.HeaderBytes, index1 + CommonFuncs.HeaderBytes.Length, threshold);

            if (index2 > 0)
                File.WriteAllBytes(path, data.Skip(index2).ToArray());
            else if (index1 > 0)
                File.WriteAllBytes(path, data.Skip(index1).ToArray());
        }

        public static void Process()
        {
            if (!UnityAssetsUtils.IsSilent)
                Console.WriteLine("\n=== Trimming Dummy Bytes ===");

            var files = Directory.EnumerateFiles(UnityAssetsUtils.WorkingDirectory, "*", SearchOption.AllDirectories);
            Parallel.ForEach(files, TrimTask);

            Console.WriteLine("Headers Fixed!");
            UnityAssetsUtils.Pause();
        }
    }
}
