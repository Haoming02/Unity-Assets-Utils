using Python.Runtime;

namespace Utils.UnityAssets
{
    /// <summary>
    /// Restore stripped Unity version
    /// </summary>
    public static class InjectVersion
    {
        public static async Task Process()
        {
            if (!CommonFuncs.PythonInit())
                return;

            Console.Write("Enter the Unity Version (eg. 2020.3.42f1): ");
            string UnityVersion = Console.ReadLine()?.Trim();

            var files = Directory.GetFiles(UnityAssetsUtils.WorkingDirectory);
            int l = files.Length;

            List<Task> writeTask = new List<Task>();

            using (Py.GIL())
            {
                dynamic UnityPy;

                try
                {
                    UnityPy = Py.Import("UnityPy");
                }
                catch (PythonException)
                {
                    Console.WriteLine("UnityPy is not Installed...");
                    Console.WriteLine("Run \"pip install unitypy\" to install!");
                    UnityAssetsUtils.Pause();
                    return;
                }

                UnityAssetsUtils.StartOperation();

                for (int i = 0; i < l; i++)
                {
                    if (!CommonFuncs.VerifyHeader(files[i]))
                        continue;

                    dynamic env = UnityPy.load(files[i]);

                    foreach (var obj in env.assets)
                    {
                        obj.unity_version = UnityVersion;
                        obj.save();
                    }

                    byte[] data = env.file.save();
                    writeTask.Add(File.WriteAllBytesAsync(files[i], data));
                }
            }

            await Task.WhenAll(writeTask);

            Console.WriteLine("Version Unstripped!");
            UnityAssetsUtils.StopOperation();
            UnityAssetsUtils.Pause();
        }
    }
}
