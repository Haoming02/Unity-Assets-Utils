using System.Text;

namespace Utils.UnityAssets
{
    /// <summary>
    /// List out all assets that contain the specified filter
    /// </summary>
    public static class FindFilter
    {
        private const int ColumnWidth = 37;

        private static async Task FindTask(string path, byte[] filter)
        {
            var data = await File.ReadAllBytesAsync(path);
            if (CommonFuncs.FindKey(ref data, filter) > 0)
            {
                var info = new FileInfo(path);
                string key = (UnityAssetsUtils.IsAlt ? CommonFuncs.GetFolder(info.FullName) : info.Name);

                Console.WriteLine($"{CommonFuncs.TrimFilePath(key, ColumnWidth),-ColumnWidth}  {(float)info.Length / 1024:N2} KB");
            }
        }

        public static async Task Process()
        {
            Console.Write("\nEnter Filter: ");

            string input = Console.ReadLine()?.Trim();
            byte[] filter = Encoding.UTF8.GetBytes(input);

            Console.Write("Recursive [y/n]: ");
            bool recursive = Console.ReadLine()?.Trim() != "n";

            UnityAssetsUtils.StartOperation();

            var files = Directory.GetFiles(UnityAssetsUtils.WorkingDirectory, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            int l = files.Length;

            Console.WriteLine($"\n{(UnityAssetsUtils.IsAlt ? "[Folder Name]" : "[File Name]"),-ColumnWidth}  [File Size]");

            Task[] tasks = new Task[l];

            for (int i = 0; i < l; i++)
            {
                int index = i;
                tasks[index] = FindTask(files[index], filter);
            }

            await Task.WhenAll(tasks);

            Console.WriteLine(string.Empty);
            UnityAssetsUtils.StopOperation();
            UnityAssetsUtils.Pause(true);
        }
    }
}
