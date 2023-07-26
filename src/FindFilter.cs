using System.Text;

namespace Utils.UnityAssets
{
    static class FindFilter
    {
        public static async Task Process()
        {
            Console.Write("\nEnter Filter: ");

            string input = Console.ReadLine()?.Trim();
            byte[] filter = Encoding.UTF8.GetBytes(input);

            bool recursive = UnityAssetsUtils.IsSilent || UnityAssetsUtils.AltMode;

            if (!UnityAssetsUtils.IsSilent && !UnityAssetsUtils.AltMode)
            {
                Console.Write("\nRecursive [y/n]: ");
                recursive = Console.ReadLine()?.Trim() == "y";
            }

            UnityAssetsUtils.StartOperation();

            var files = Directory.GetFiles(UnityAssetsUtils.WorkingDirectory, "*.*", (recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));
            int l = files.Length;

            Console.WriteLine($"\n{(UnityAssetsUtils.AltMode ? "[Folder Name]" : "[File Name]"),-36}  [File Size]");

            Task[] tasks = new Task[l];

            for (int i = 0; i < l; i++)
            {
                int fileIndex = i;

                tasks[i] = Task.Run(() =>
                {
                    var data = File.ReadAllBytes(files[fileIndex]);
                    if (Funcs.FindKey(ref data, filter))
                    {
                        var info = new FileInfo(files[fileIndex]);
                        string key = (UnityAssetsUtils.AltMode ? Funcs.ConvolutedGetFolder(info.FullName) : info.Name);

                        Console.WriteLine($"{key.Substring(0, Math.Min(key.Length, 36)),-36}  {(float)info.Length / 1024:N2} KB");
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
