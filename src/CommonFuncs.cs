using System.Text;

namespace Utils.UnityAssets
{
    /// <summary>
    /// Some common functions...
    /// </summary>
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

        /// <summary>
        /// Check if the Unity asset header exists in the file
        /// </summary>
        /// <returns><b>True</b> if exists;<br/><b>False</b> otherwise</returns>
        public static bool VerifyHeader(string path)
        {
            var buffer = new char[HEADER.Length];

            using (var r = new StreamReader(path))
                r.ReadBlock(buffer, 0, HEADER.Length);

            return buffer.SequenceEqual(HEADER);
        }

        /// <summary>
        /// Return the name of the folder that the given path is in
        /// </summary>
        public static string GetFolder(string path)
        {
            string dir = Path.GetDirectoryName(path);
            string[] folders = dir.Split(Path.DirectorySeparatorChar);

            return folders[^1];
        }

        /// <summary>
        /// Check if a given filter exists in the data
        /// </summary>
        /// <returns>Index of the filter if exists;<br/><b>-1</b> otherwise</returns>
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

        /// <summary>
        /// Optimized the display of a path to a fixed length
        /// </summary>
        public static string TrimFilePath(string path, int length)
        {
            if (path.Length <= length)
                return path;

            int half = (length - 1) / 2 - 1;
            return $"{path[..half]}...{path[^half..]}";
        }

        /// <summary>
        /// Automatically detects if the WorkingDirectory is in Alt. structure
        /// </summary>
        /// <returns><b>True</b> if so;<br/><b>False</b> otherwise</returns>
        public static bool DetectStructure(byte limit = 4)
        {
            var folders = Directory.EnumerateDirectories(UnityAssetsUtils.WorkingDirectory);
            byte i = 0;

            foreach (var folder in folders)
            {
                if (RecursiveFolderCheck(folder, limit))
                    return true;

                i++;

                if (i > limit)
                    break;
            }

            return false;
        }

        private static bool RecursiveFolderCheck(string folder, byte limit)
        {
            var data = File.Exists(Path.Combine(folder, "__data"));
            var info = File.Exists(Path.Combine(folder, "__info"));
            if (data && info)
                return true;

            byte i = 0;

            var subfolders = Directory.EnumerateDirectories(folder);
            foreach (var subfolder in subfolders)
            {
                if (RecursiveFolderCheck(subfolder, limit))
                    return true;

                i++;

                if (i > limit)
                    break;
            }

            return false;
        }
    }
}
