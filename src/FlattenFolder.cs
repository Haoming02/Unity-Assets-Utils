namespace Utils.UnityAssets
{
    /// <summary>
    /// Move all files inside subfolders into the working directory
    /// </summary>
    public static class FlattenFolder
    {
        public static async Task<bool> Process()
        {
            Console.WriteLine($"!!! Type YES to Flatten Folder \"{UnityAssetsUtils.WorkingDirectory}\" !!!");
            Console.WriteLine($"[IMPORTANT] This operation is NOT reversible!");
            Console.Write("Confirm: ");

            if (Console.ReadLine()?.Trim() != "YES")
            {
                Console.WriteLine("Operation Canceled...");
                UnityAssetsUtils.Pause();
                return false;
            }

            UnityAssetsUtils.StartOperation();

            var files = Directory.GetFiles(UnityAssetsUtils.WorkingDirectory, UnityAssetsUtils.IsAlt ? "__data" : "*", SearchOption.AllDirectories);

            int l = files.Length;
            Task[] tasks = new Task[l];

            for (int index = 0; index < l; index++)
            {
                int i = index;

                tasks[index] = Task.Run(() =>
                {
                    // Console.WriteLine($"Moving from \"{files[i]}\" to \"{Path.Combine(UnityAssetsUtils.WorkingDirectory, Path.GetFileName(files[i]))}\"");
                    File.Move(files[i], Path.Combine(UnityAssetsUtils.WorkingDirectory, UnityAssetsUtils.IsAlt ? "__" + CommonFuncs.GetFolder(files[i]) : Path.GetFileName(files[i])));
                });
            }

            await Task.WhenAll(tasks);

            var folders = Directory.GetDirectories(UnityAssetsUtils.WorkingDirectory);

            foreach (var folder in folders)
            {
                if (!UnityAssetsUtils.IsSilent)
                    Console.WriteLine($"Deleting Folder \"{folder}\"");
                Directory.Delete(folder, UnityAssetsUtils.IsAlt);
            }

            Console.WriteLine("Folder Flattened!");
            UnityAssetsUtils.StopOperation();
            UnityAssetsUtils.Pause();

            return true;
        }
    }
}
