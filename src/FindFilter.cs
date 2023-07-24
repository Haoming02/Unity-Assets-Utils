using System.Text;

namespace Utils.UnityAssets
{
    static class FindFilter
    {
        public static async Task Process()
        {
            Console.Write("\nEnter Filter: ");

            string? input = Console.ReadLine()?.Trim();
            byte[] filter = Encoding.UTF8.GetBytes(input);

            bool recursive = UnityAssetsUtils.isSilent;

            if (!UnityAssetsUtils.isSilent)
            {
                Console.Write("\nRecursive [y/n]: ");
                recursive = Console.ReadLine() == "y";
            }

            UnityAssetsUtils.StartOperation();

            var files = recursive ? Directory.GetFiles(UnityAssetsUtils.WorkingDirectory, "*.*", SearchOption.AllDirectories) : Directory.GetFiles(UnityAssetsUtils.WorkingDirectory);
            int l = files.Length;

            Console.WriteLine($"\n{"[File Name]",-36}[File Size]");

            Task[] tasks = new Task[l];

            for (int i = 0; i < l; i++)
            {
                int fileIndex = i;

                tasks[i] = Task.Run(() =>
                {
                    var data = File.ReadAllBytes(files[fileIndex]);
                    if (LocateFilter(ref data, ref filter))
                    {
                        var info = new FileInfo(files[fileIndex]);
                        Console.WriteLine($"{info.Name,-36}{(float)info.Length / 1024:N2} KB");
                    }

                    return;
                });
            }

            await Task.WhenAll(tasks);
            UnityAssetsUtils.StopOperation();
            UnityAssetsUtils.Pause(true);
        }

        private static bool LocateFilter(ref byte[] data, ref byte[] filter)
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
