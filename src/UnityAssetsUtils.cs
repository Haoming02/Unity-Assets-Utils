using Python.Runtime;
using uint8 = System.Byte;

namespace Utils.UnityAssets
{
    /// <summary>
    /// Main Console UI
    /// </summary>
    internal class UnityAssetsUtils
    {
        public static string WorkingDirectory { get; private set; }

        private static uint8 currentMode;
        private static readonly Dictionary<string, uint8> AvailableModes = new()
        {
            {"Byte Trimmer", 1},
            {"Dedupe", 2},
            {"Flatten Folder", 3},
            {"Find Filter", 4},
            {"Fill Alpha", 5},
            {"Inject Version", 6},
            {"Separator", 7},
            {"Change Directory", 8},
            {"Toggle Mode", 9},
            {"Exit", 0}
        };

        private const uint8 INVALID = 255;

        public static bool IsSilent { get; private set; }
        public static bool IsAlt { get; private set; }

        private static bool useTimer;
        private static System.Diagnostics.Stopwatch Timer = null;

        static async Task Main(string[] args)
        {
            AppContext.SetSwitch("Switch.System.Runtime.Serialization.UseUnsafeTypeForwarders", true);

            Console.WriteLine("===== Welcome to Unity Assets Utilities =====");
            WorkingDirectory = string.Empty;

            IsSilent = false;
            IsAlt = false;
            useTimer = false;

            foreach (string arg in args)
            {
                if (arg.ToLower().Contains("silent"))
                    IsSilent = true;
                else if (arg.ToLower().Contains("alt"))
                    IsAlt = true;
                else if (arg.ToLower().Contains("timer"))
                    useTimer = true;
            }

            if (useTimer)
            {
                Timer = new System.Diagnostics.Stopwatch();
                Console.WriteLine("\tLaunching with Timer...");
            }

            currentMode = INVALID;

            WorkingDirectory = SetDirectory(true);
            await ProgramLoop();
        }

        private static async Task ProgramLoop()
        {
            do
            {
                Console.Clear();
                Console.WriteLine($"Working Directory: \"{WorkingDirectory}\"");
                Console.WriteLine($"Mode: {(IsAlt ? "Alt." : "Normal")}\n");

                foreach (var pair in AvailableModes)
                    Console.WriteLine($"[{pair.Value}] {pair.Key}");

                Console.Write("\nChoose a Function: ");

                try { currentMode = uint8.Parse(Console.ReadLine()?.Trim()); }
                catch { currentMode = INVALID; }

                if (currentMode >= AvailableModes.Count)
                {
                    Console.WriteLine("Invalid Input!");
                    Pause();
                    continue;
                }

                if (currentMode == AvailableModes["Byte Trimmer"])
                {
                    await ByteTrimmer.Process(); continue;
                }
                if (currentMode == AvailableModes["Dedupe"])
                {
                    await Dedupe.Process(); continue;
                }
                if (currentMode == AvailableModes["Find Filter"])
                {
                    await FindFilter.Process(); continue;
                }
                if (currentMode == AvailableModes["Fill Alpha"])
                {
                    await Task.Run(() => { FillAlpha.Process(); }); continue;
                }
                if (currentMode == AvailableModes["Flatten Folder"])
                {
                    if (await FlattenFolder.Process())
                    {
                        if (IsAlt) IsAlt = false;
                    }
                    continue;
                }
                if (currentMode == AvailableModes["Inject Version"])
                {
                    await InjectVersion.Process(); continue;
                }
                if (currentMode == AvailableModes["Separator"])
                {
                    await Separator.Process(); continue;
                }
                if (currentMode == AvailableModes["Change Directory"])
                {
                    WorkingDirectory = SetDirectory();
                    if (IsAlt) IsAlt = false;
                    continue;
                }
                if (currentMode == AvailableModes["Toggle Mode"])
                {
                    IsAlt = !IsAlt; continue;
                }
            } while (currentMode != AvailableModes["Exit"]);

            try
            {
                PythonEngine.Shutdown();
            }
            catch
            {
                Console.WriteLine("Python Closed...");
            }
            finally
            {
                Environment.Exit(0);
            }
        }

        // Prevent accidentally messing up the system root folder
        public const int SAFE_GUARD = 4;

        private static string SetDirectory(bool init = false)
        {
            do
            {
                if (init)
                    Console.Write("Enter the Path to Assets: ");
                else
                    Console.Write("Enter the Path to Assets (Enter \"return\" to Cancel): ");

                string input = Console.ReadLine()?.Trim();

                if (input.Length > SAFE_GUARD && Directory.Exists(input))
                    return input;
                else if (!init && input.ToLower().Contains("return"))
                    return WorkingDirectory;
                else
                {
                    Console.WriteLine("Invalid Path...");
                    Pause();
                    Console.Clear();
                }
            } while (true);
        }

        public static void StartOperation() { if (useTimer) Timer.Start(); }
        public static void StopOperation()
        {
            if (!useTimer) return;
            Timer.Stop();
            Console.WriteLine($"Took: {Timer.ElapsedMilliseconds / 1000.0f:N4}s");
        }

        public static void Pause(bool force = false)
        {
            if (!IsSilent || useTimer || force)
            {
                Console.WriteLine("Press ENTER to Continue...");
                while (Console.ReadKey().Key != ConsoleKey.Enter) { }
            }
        }
    }
}
