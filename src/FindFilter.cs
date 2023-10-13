using System.Text;

namespace Utils.UnityAssets
{
    /// <summary>
    /// List out all assets that contain specified filter
    /// </summary>
    public static class FindFilter
    {
        public static async Task Process()
        {
            Console.Write("\nEnter Filter: ");

            string input = Console.ReadLine()?.Trim();
            byte[] filter = Encoding.UTF8.GetBytes(input);

            Console.Write("\nRecursive [y/n]: ");
            bool recursive = Console.ReadLine()?.Trim() != "n";

            UnityAssetsUtils.StartOperation();

            var files = Directory.GetFiles(UnityAssetsUtils.WorkingDirectory, "*.*", (recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));
            int l = files.Length;

            Console.WriteLine($"\n{(UnityAssetsUtils.IsAlt ? "[Folder Name]" : "[File Name]"),-36}  [File Size]");

            Task[] tasks = new Task[l];

            for (int index = 0; index < l; index++)
            {
                int i = index;

                tasks[index] = Task.Run(() =>
                {
                    var data = File.ReadAllBytes(files[i]);
                    if (CommonFuncs.FindKey(ref data, filter) > 0)
                    {
                        var info = new FileInfo(files[i]);
                        string key = (UnityAssetsUtils.IsAlt ? CommonFuncs.GetFolder(info.FullName) : info.Name);

                        Console.WriteLine($"{key[..Math.Min(key.Length, 36)],-36}  {(float)info.Length / 1024:N2} KB");
                    }

                    return;
                });
            }

            await Task.WhenAll(tasks);

            UnityAssetsUtils.StopOperation();
            UnityAssetsUtils.Pause(true);
        }
    }
}
