namespace Utils.UnityAssets
{
    static class Funcs
    {
        public static string ConvolutedGetFolder(string path)
        {
            string dir = Path.GetDirectoryName(path);
            string[] folders = dir.Split(Path.DirectorySeparatorChar);

            return folders[^1];
        }

        public static bool FindKey(ref byte[] data, byte[] filter)
        {
            for (int i = 0; i < data.Length - filter.Length + 1; i++)
            {
                if (data.Skip(i).Take(filter.Length).SequenceEqual(filter))
                    return true;
            }

            return false;
        }
    }
}
