using Python.Runtime;

namespace Utils.UnityAssets
{
    /// <summary>
    /// Remove the Alpha channel of images
    /// </summary>
    public static class FillAlpha
    {
        private static readonly string[] AvailableFormats = new string[] { ".png", ".jpg", ".jpeg", ".tiff", ".bmp", ".gif" };

        private static bool VerifyExtension(string path)
        {
            return AvailableFormats.Contains(Path.GetExtension(path));
        }

        public static void Process()
        {
            if (!CommonFuncs.PythonInit())
                return;

            if (!UnityAssetsUtils.IsSilent)
                Console.WriteLine("\n=== Filling Alpha ===");

            var files = Directory.GetFiles(UnityAssetsUtils.WorkingDirectory);
            int l = files.Length;

            var ptr = PythonEngine.BeginAllowThreads();
            using (Py.GIL())
            {
                dynamic openCV;

                try
                {
                    openCV = Py.Import("cv2");
                }
                catch (PythonException)
                {
                    Console.WriteLine("OpenCV is not Installed...");
                    Console.WriteLine("Run \"pip install opencv-python\" to install!");
                    UnityAssetsUtils.Pause();
                    return;
                }

                UnityAssetsUtils.StartOperation();

                for (int i = 0; i < l; i++)
                {
                    if (!VerifyExtension(files[i]))
                        continue;

                    dynamic rgb = openCV.imread(files[i], openCV.IMREAD_COLOR);
                    openCV.imwrite(files[i], rgb);
                }
            }
            PythonEngine.EndAllowThreads(ptr);

            Console.WriteLine("Alpha Channel Removed!");
            UnityAssetsUtils.StopOperation();
            UnityAssetsUtils.Pause();
        }
    }
}
