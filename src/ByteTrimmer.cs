namespace Utils.UnityAssets
{
    /// <summary>
    /// Remove the leading dummy bytes of asset files
    /// </summary>
    public static class ByteTrimmer
    {
        /// <summary>
        /// Larger value takes longer to search for non-asset files;<br/>
        /// but also safer for the actual asset files
        /// </summary>
        private const int threshold = 4096;

        private static async Task TrimTask(string path)
        {
            var data = await File.ReadAllBytesAsync(path);

            int index1 = CommonFuncs.FindKey(ref data, CommonFuncs.HeaderBytes, 0, threshold);
            if (index1 < 0)
            {
                if (!UnityAssetsUtils.IsSilent)
                    Console.WriteLine($"\tNo Header Found! Skipping File: \"{Path.GetFileName(path)}\"...");
                return;
            }

            int index2 = CommonFuncs.FindKey(ref data, CommonFuncs.HeaderBytes, index1 + CommonFuncs.HeaderBytes.Length, threshold);

            if (index2 > 0)
                await File.WriteAllBytesAsync(path, data.Skip(index2).ToArray());

            else if (index1 > 0)
                await File.WriteAllBytesAsync(path, data.Skip(index1).ToArray());
        }

        public static async Task Process()
        {
            if (!UnityAssetsUtils.IsSilent)
                Console.WriteLine("\n=== Trimming Dummy Bytes ===");

            UnityAssetsUtils.StartOperation();

            var files = Directory.GetFiles(UnityAssetsUtils.WorkingDirectory, "*", SearchOption.AllDirectories);
            int l = files.Length;

            Task[] tasks = new Task[l];

            for (int i = 0; i < l; i++)
            {
                int index = i;
                tasks[index] = TrimTask(files[index]);
            }

            await Task.WhenAll(tasks);

            Console.WriteLine("Headers Fixed!");
            UnityAssetsUtils.StopOperation();
            UnityAssetsUtils.Pause();
        }
    }
}
