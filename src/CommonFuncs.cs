using Python.Runtime;
using System.Diagnostics;

namespace Utils.UnityAssets
{
    /// <summary>
    /// Some common functions...
    /// </summary>
    public static class CommonFuncs
    {
        public const string HEADER = "UnityFS";

        private enum PythonStatus { Uninit, Success, Fail };
        private static PythonStatus status = PythonStatus.Uninit;

        private static string TryGetPythonVersion()
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";

            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;

            cmd.Start();

            cmd.StandardInput.WriteLine("python --version");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();

            while (!cmd.StandardOutput.EndOfStream)
            {
                var line = cmd.StandardOutput.ReadLine();
                if (line.Contains("Python"))
                    return line;
            }

            throw new FileNotFoundException();
        }

        public static bool PythonInit()
        {
            switch (status)
            {
                case PythonStatus.Success:
                    return true;

                case PythonStatus.Fail:
                    Console.WriteLine("Restart the Program to retry initializing Python...");
                    UnityAssetsUtils.Pause();
                    return false;

                default:
                    string minorVersion;

                    try
                    {
                        string version = TryGetPythonVersion();
                        Console.WriteLine($"Detected Python Version: {version}");
                        minorVersion = version.Split(".")[1];
                    }
                    catch
                    {
                        Console.WriteLine("Failed to detect Python automatically...");
                        Console.WriteLine("Please enter the Python version manually (If you have 3.10.6, enter 10)");

                        Console.Write("Minor Version: ");
                        minorVersion = Console.ReadLine()?.Trim();
                    }

                    try
                    {
                        Runtime.PythonDLL = string.Format($"python3{minorVersion}.dll");
                        PythonEngine.Initialize();
                        status = PythonStatus.Success;
                        return true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Unable to start Python...");

                        if (!UnityAssetsUtils.IsSilent)
                            Console.WriteLine(e.Message);

                        UnityAssetsUtils.Pause();

                        status = PythonStatus.Fail;
                        return false;
                    }
            }
        }

        public static bool VerifyHeader(string path)
        {
            using (var r = new StreamReader(path))
            {
                var buffer = new char[HEADER.Length];
                r.ReadBlock(buffer, 0, HEADER.Length);

                return buffer.SequenceEqual(HEADER);
            }
        }

        public static string GetFolder(string path)
        {
            string dir = Path.GetDirectoryName(path);
            string[] folders = dir.Split(Path.DirectorySeparatorChar);

            return folders[^1];
        }

        public static int FindKey(ref byte[] data, byte[] filter, int from = 0, int threshold = int.MaxValue)
        {
            int dataLength = data.Length;
            int filterLength = filter.Length;
            int limit = Math.Min(dataLength - filterLength, threshold);

            for (int i = from; i <= limit; i++)
            {
                if (data.Skip(i).Take(filterLength).SequenceEqual(filter))
                    return i;
            }

            return -1;
        }
    }
}
