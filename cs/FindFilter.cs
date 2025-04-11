using System.Text;

namespace Utils.UnityAssets
{
    /// <summary>List out all assets that contain the specified filter</summary>
    public static class FindFilter
    {
        private const int ColumnWidth = 36;

        public static void Process()
        {
            if (!UnityAssetsUtils.IsSilent)
                Console.WriteLine("\n=== Finding Assets ===");

            Console.Write("\nEnter Filter: ");
            string input = Console.ReadLine().Trim();
            byte[] filter = Encoding.UTF8.GetBytes(input);

            Console.Write("Recursive [y/N]: ");
            bool recursive = !Console.ReadLine().Trim().Equals("N");
            var option = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            var files = Directory.EnumerateFiles(UnityAssetsUtils.WorkingDirectory, "*", option);
            var sb = new StringBuilder();

            Parallel.ForEach(files, (file) =>
            {
                var data = File.ReadAllBytes(file);
                if (CommonFuncs.FindKey(ref data, filter) > 0)
                {
                    var info = new FileInfo(file);
                    string key = UnityAssetsUtils.IsAlt ? CommonFuncs.GetFolder(info.FullName) : info.Name;

                    lock (sb)
                    {
                        sb.AppendLine($"{CommonFuncs.TrimFilePath(key, ColumnWidth),-ColumnWidth}  {(float)info.Length / 1024:N2} KB");
                    }
                }
            });

            if (sb.Length > 0)
            {
                Console.WriteLine($"\n{(UnityAssetsUtils.IsAlt ? "[Folder Name]" : "[File Name]"),-ColumnWidth}  [File Size]");
                Console.Write(sb.ToString());
            }
            else
                Console.WriteLine("\nNo result was found...");

            Console.WriteLine(string.Empty);
            UnityAssetsUtils.Pause(true);
        }
    }
}
