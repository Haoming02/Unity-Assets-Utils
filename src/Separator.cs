using System.Text;

namespace Utils.UnityAssets
{
    static class Separator
    {
        private static readonly string[] AssetTypes = { "audio", "mesh", "texture", "anim", "shader", "misc" };
        private static string toPath(string asset) => Path.Combine(UnityAssetsUtils.WorkingDirectory, asset);

        private static void CreateFolders()
        {
            foreach (var assetType in AssetTypes)
            {
                var folder = toPath(assetType);
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
            }
        }

        public static async Task Process()
        {
            UnityAssetsUtils.StartOperation();
            CreateFolders();

            var files = Directory.GetFiles(UnityAssetsUtils.WorkingDirectory);
            int l = files.Length;

            Task[] tasks = new Task[l];

            for (int i = 0; i < l; i++)
            {
                int fileIndex = i;

                tasks[i] = Task.Run(() =>
                {
                    var data = File.ReadAllBytes(files[fileIndex]);
                    if (FindKey(ref data, AssetKeys.AudioClip))
                        File.Move(files[fileIndex], Path.Combine(toPath("audio"), Path.GetFileName(files[fileIndex])));
                    else if (FindKey(ref data, AssetKeys.Mesh))
                        File.Move(files[fileIndex], Path.Combine(toPath("mesh"), Path.GetFileName(files[fileIndex])));
                    else if (FindKey(ref data, AssetKeys.Texture) || FindKey(ref data, AssetKeys.Sprite))
                        File.Move(files[fileIndex], Path.Combine(toPath("texture"), Path.GetFileName(files[fileIndex])));
                    else if (FindKey(ref data, AssetKeys.Keyframe))
                        File.Move(files[fileIndex], Path.Combine(toPath("anim"), Path.GetFileName(files[fileIndex])));
                    else if (FindKey(ref data, AssetKeys.Shader))
                        File.Move(files[fileIndex], Path.Combine(toPath("shader"), Path.GetFileName(files[fileIndex])));
                    else
                    {
                        if (!UnityAssetsUtils.isSilent)
                            Console.WriteLine($"No Keywords Found in File: \"{Path.GetFileName(files[fileIndex])}\"");
                        File.Move(files[fileIndex], Path.Combine(toPath("misc"), Path.GetFileName(files[fileIndex])));
                    }
                });
            }

            await Task.WhenAll(tasks);
            UnityAssetsUtils.StopOperation();
            UnityAssetsUtils.Pause();
        }

        private static bool FindKey(ref byte[] data, byte[] filter)
        {
            for (int i = 0; i < data.Length - filter.Length + 1; i++)
            {
                if (data.Skip(i).Take(filter.Length).SequenceEqual(filter))
                    return true;
            }

            return false;
        }

        private static class AssetKeys
        {
            public static readonly byte[] AudioClip = Encoding.UTF8.GetBytes("AudioClip");
            public static readonly byte[] Mesh = Encoding.UTF8.GetBytes("Mesh");
            public static readonly byte[] Texture = Encoding.UTF8.GetBytes("exture");
            public static readonly byte[] Sprite = Encoding.UTF8.GetBytes("prite");
            public static readonly byte[] Keyframe = Encoding.UTF8.GetBytes("Keyframe");
            public static readonly byte[] Shader = Encoding.UTF8.GetBytes("Shader");
        }
    }
}
