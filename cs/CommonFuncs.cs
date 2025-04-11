using System.Text;

namespace Utils.UnityAssets
{
    public static class CommonFuncs
    {
        public const string HEADER = "UnityFS";

        private static byte[] _headerBytes = null;
        public static byte[] HeaderBytes
        {
            get
            {
                _headerBytes ??= Encoding.UTF8.GetBytes(HEADER);
                return _headerBytes;
            }
        }

        /// <summary>Check if the file has the Unity asset header</summary>
        public static bool VerifyHeader(string path)
        {
            var buffer = new byte[HEADER.Length];
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                if (fs.Length < HeaderBytes.Length)
                    return false;
                fs.Read(buffer, 0, HeaderBytes.Length);
            }
            return buffer.SequenceEqual(HeaderBytes);
        }

        /// <summary>Return the name of the folder that the given path is in</summary>
        public static string GetFolder(string path)
        {
            string dir = Path.GetDirectoryName(path);
            string[] folders = dir.Split(Path.DirectorySeparatorChar);
            return folders[^1];
        }

        /// <summary>Check if a given filter exists in the data</summary>
        /// <returns>Index of the filter if exists;<br/><b>-1</b> otherwise</returns>
        public static int FindKey(ref byte[] data, byte[] filter, int from = 0, int threshold = int.MaxValue)
        {
            int dataLength = data.Length;
            int filterLength = filter.Length;
            int limit = Math.Min(dataLength - filterLength, threshold);

            for (int i = from; i <= limit; i++)
            {
                bool found = true;
                for (int j = 0; j < filterLength; j++)
                {
                    if (data[i + j] == filter[j])
                        continue;

                    found = false;
                    break;
                }

                if (found)
                    return i;
            }

            return -1;
        }

        /// <summary>Optimized the display of a path to a fixed length</summary>
        public static string TrimFilePath(string path, int length)
        {
            if (path.Length <= length)
                return path;

            int half = (length - 3) / 2;
            return $"{path[..half]}...{path[^half..]}";
        }
    }
}
