using System.Text;

namespace Utils.UnityAssets
{
    static class ByteTrimmer
    {
        private const int threshold = 69420;

        private const string HEADER = "UnityFS";
        private static byte[] HeaderBytes => Encoding.UTF8.GetBytes(HEADER);

        public static async Task Process()
        {
            if (!UnityAssetsUtils.isSilent)
                Console.WriteLine("\nTrimming Dummy Bytes...");

            UnityAssetsUtils.StartOperation();

            var files = Directory.GetFiles(UnityAssetsUtils.WorkingDirectory);
            int l = files.Length;

            Task[] tasks = new Task[l];

            for (int i = 0; i < l; i++)
            {
                int fileIndex = i;

                tasks[i] = Task.Run(() =>
                {
                    var data = File.ReadAllBytes(files[fileIndex]);
                    var index1 = FindHeader(ref data);

                    if (index1 == -1)
                    {
                        if (!UnityAssetsUtils.isSilent)
                            Console.WriteLine($"No Header Found! Skipping File: \"{Path.GetFileName(files[fileIndex])}\"...");
                        return;
                    }

                    var index2 = FindHeader(ref data, index1 + 1);

                    if (index2 > 0)
                        File.WriteAllBytes(files[fileIndex], data.Skip(index2).ToArray());
                    else if (index1 > 0)
                        File.WriteAllBytes(files[fileIndex], data.Skip(index1).ToArray());

                    return;
                });
            }

            await Task.WhenAll(tasks);
            Console.WriteLine("Headers Fixed!");
            UnityAssetsUtils.StopOperation();
            UnityAssetsUtils.Pause();
        }

        private static int FindHeader(ref byte[] data, int from = 0)
        {
            int limit = Math.Min(data.Length - HeaderBytes.Length + 1, threshold);
            for (int i = from; i < limit; i++)
            {
                if (data.Skip(i).Take(HeaderBytes.Length).SequenceEqual(HeaderBytes))
                    return i;
            }

            return -1;
        }
    }
}
