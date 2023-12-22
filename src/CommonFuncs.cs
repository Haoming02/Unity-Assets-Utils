namespace Utils.UnityAssets
{
    /// <summary>
    /// Some common functions...
    /// </summary>
    public static class CommonFuncs
    {
        public const string HEADER = "UnityFS";

        public static bool VerifyHeader(string path)
        {
            using (var r = new StreamReader(path))
            {
                var buffer = new char[HEADER.Length];
                r.ReadBlock(buffer, 0, HEADER.Length);

                return buffer.SequenceEqual(HEADER);
            }
        }

        public static string GetFolder(string path)
        {
            string dir = Path.GetDirectoryName(path);
            string[] folders = dir.Split(Path.DirectorySeparatorChar);

            return folders[^1];
        }

        public static int FindKey(ref byte[] data, byte[] filter, int from = 0, int threshold = int.MaxValue)
        {
            int dataLength = data.Length;
            int filterLength = filter.Length;
            int limit = Math.Min(dataLength - filterLength, threshold);

            for (int i = from; i <= limit; i++)
            {
                if (data.Skip(i).Take(filterLength).SequenceEqual(filter))
                    return i;
            }

            return -1;
        }
    }
}
