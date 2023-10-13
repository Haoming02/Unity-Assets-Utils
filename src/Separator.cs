using System.Text;

namespace Utils.UnityAssets
{
    /// <summary>
    /// Attempt to categorize all assets into different folders
    /// </summary>
    public static class Separator
    {
        private static readonly string[] AssetTypes = { "media", "mesh", "texture", "anim", "shader", "misc" };
        private static string ToPath(string asset) => Path.Combine(UnityAssetsUtils.WorkingDirectory, asset);

        private static void CreateFolders()
        {
            foreach (var assetType in AssetTypes)
            {
                var folder = ToPath(assetType);
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
            }
        }

        public static async Task Process()
        {
            if (!UnityAssetsUtils.IsSilent)
                Console.WriteLine("\n=== Categorizing Assets ===");

            UnityAssetsUtils.StartOperation();
            CreateFolders();

            var files = Directory.GetFiles(UnityAssetsUtils.WorkingDirectory);
            int l = files.Length;

            Task[] tasks = new Task[l];

            for (int index = 0; index < l; index++)
            {
                int i = index;

                tasks[index] = Task.Run(() =>
                {
                    if (!CommonFuncs.VerifyHeader(files[i]))
                    {
                        Console.WriteLine($"\tNo Header Found! Skipping File: \"{Path.GetFileName(files[i])}\"");
                        return;
                    }

                    var data = File.ReadAllBytes(files[i]);

                    if (CommonFuncs.FindKey(ref data, AssetKeys.BlendShape) > 0)
                        File.Move(files[i], Path.Combine(ToPath("mesh"), Path.GetFileName(files[i])));
                    else if (CommonFuncs.FindKey(ref data, AssetKeys.AudioClip) > 0)
                        File.Move(files[i], Path.Combine(ToPath("media"), Path.GetFileName(files[i])));
                    else if (CommonFuncs.FindKey(ref data, AssetKeys.Video) > 0)
                        File.Move(files[i], Path.Combine(ToPath("media"), Path.GetFileName(files[i])));
                    else if (CommonFuncs.FindKey(ref data, AssetKeys.Sprite) > 0)
                        File.Move(files[i], Path.Combine(ToPath("texture"), Path.GetFileName(files[i])));
                    else if (CommonFuncs.FindKey(ref data, AssetKeys.Mesh) > 0)
                        File.Move(files[i], Path.Combine(ToPath("mesh"), Path.GetFileName(files[i])));
                    else if (CommonFuncs.FindKey(ref data, AssetKeys.Texture) > 0)
                        File.Move(files[i], Path.Combine(ToPath("texture"), Path.GetFileName(files[i])));
                    else if (CommonFuncs.FindKey(ref data, AssetKeys.Animation) > 0)
                        File.Move(files[i], Path.Combine(ToPath("anim"), Path.GetFileName(files[i])));
                    else if (CommonFuncs.FindKey(ref data, AssetKeys.Keyframe) > 0)
                        File.Move(files[i], Path.Combine(ToPath("anim"), Path.GetFileName(files[i])));
                    else if (CommonFuncs.FindKey(ref data, AssetKeys.Shader) > 0)
                        File.Move(files[i], Path.Combine(ToPath("shader"), Path.GetFileName(files[i])));
                    else
                    {
                        if (!UnityAssetsUtils.IsSilent)
                            Console.WriteLine($"No Keywords Found in File: \"{Path.GetFileName(files[i])}\"");
                        File.Move(files[i], Path.Combine(ToPath("misc"), Path.GetFileName(files[i])));
                    }
                });
            }

            await Task.WhenAll(tasks);

            Console.WriteLine("Files Separated!");
            UnityAssetsUtils.StopOperation();
            UnityAssetsUtils.Pause();
        }

        private static class AssetKeys
        {
            public static readonly byte[] AudioClip = Encoding.UTF8.GetBytes("Audio");
            public static readonly byte[] Animation = Encoding.UTF8.GetBytes(".anim");
            public static readonly byte[] BlendShape = Encoding.UTF8.GetBytes("BlendShape");
            public static readonly byte[] Keyframe = Encoding.UTF8.GetBytes("Keyframe");
            public static readonly byte[] Mesh = Encoding.UTF8.GetBytes("Mesh");
            public static readonly byte[] Sprite = Encoding.UTF8.GetBytes("Sprite");
            public static readonly byte[] Texture = Encoding.UTF8.GetBytes("Texture");
            public static readonly byte[] Shader = Encoding.UTF8.GetBytes("Shader");
            public static readonly byte[] Video = Encoding.UTF8.GetBytes("Video");
        }
    }
}
