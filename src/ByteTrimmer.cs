using System.Text;

namespace Utils.UnityAssets
{
    /// <summary>
    /// Remove the leading dummy bytes of asset files
    /// </summary>
    public static class ByteTrimmer
    {
        /// <summary>
        /// Larger value takes longer to search for non-asset files;
        /// but also safer for real asset files
        /// </summary>
        private const int threshold = 4096;

        private static byte[] HeaderBytes => Encoding.UTF8.GetBytes(CommonFuncs.HEADER);

        public static async Task Process()
        {
            if (!UnityAssetsUtils.IsSilent)
                Console.WriteLine("\n=== Trimming Dummy Bytes ===");

            UnityAssetsUtils.StartOperation();

            var files = Directory.GetFiles(UnityAssetsUtils.WorkingDirectory);
            int l = files.Length;

            Task[] tasks = new Task[l];

            for (int index = 0; index < l; index++)
            {
                int i = index;

                tasks[index] = Task.Run(() =>
                {
                    var data = File.ReadAllBytes(files[i]);

                    int index1 = CommonFuncs.FindKey(ref data, HeaderBytes, 0, threshold);
                    if (index1 < 0)
                    {
                        if (!UnityAssetsUtils.IsSilent)
                            Console.WriteLine($"\tNo Header Found! Skipping File: \"{Path.GetFileName(files[i])}\"...");
                        return;
                    }

                    int index2 = CommonFuncs.FindKey(ref data, HeaderBytes, index1 + HeaderBytes.Length, threshold);

                    if (index2 > 0)
                        File.WriteAllBytes(files[i], data.Skip(index2).ToArray());
                    else if (index1 > 0)
                        File.WriteAllBytes(files[i], data.Skip(index1).ToArray());
                });
            }

            await Task.WhenAll(tasks);

            Console.WriteLine("Headers Fixed!");
            UnityAssetsUtils.StopOperation();
            UnityAssetsUtils.Pause();
        }
    }
}
