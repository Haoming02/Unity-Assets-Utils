namespace Utils.UnityAssets
{
    static class FlattenFolder
    {
        public static async Task Process()
        {
            Console.WriteLine($"=== Type YES to Flatten Folder \"{UnityAssetsUtils.WorkingDirectory}\" ===");
            Console.Write("Confirm: ");

            if (Console.ReadLine()?.Trim() != "YES")
            {
                Console.WriteLine("Operation Canceled...");
                return;
            }

            UnityAssetsUtils.StartOperation();

            var files = Directory.GetFiles(UnityAssetsUtils.WorkingDirectory, "*", SearchOption.AllDirectories);
            int l = files.Length;

            Task[] tasks = new Task[l];

            for (int i = 0; i < l; i++)
            {
                int fileIndex = i;

                tasks[i] = Task.Run(() =>
                {
                    // Console.WriteLine($"Moving from \"{files[fileIndex]}\" to \"{Path.Combine(UnityAssetsUtils.WorkingDirectory, Path.GetFileName(files[fileIndex]))}\"");
                    File.Move(files[fileIndex], Path.Combine(UnityAssetsUtils.WorkingDirectory, Path.GetFileName(files[fileIndex])));
                    return;
                });
            }

            await Task.WhenAll(tasks);

            var folders = Directory.GetDirectories(UnityAssetsUtils.WorkingDirectory);

            foreach (var folder in folders)
            {
                // Console.WriteLine($"Deleting \"{folder}\"");
                Directory.Delete(folder, false);
            }

            Console.WriteLine("Folder Flattened!");
            UnityAssetsUtils.StopOperation();
            UnityAssetsUtils.Pause();
        }
    }
}
