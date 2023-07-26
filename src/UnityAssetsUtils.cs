namespace Utils.UnityAssets
{
    class UnityAssetsUtils
    {
        private static readonly Dictionary<string, int> Modes = new()
        {
            {"Flatten Folder", 1},
            {"Byte Trimmer", 2},
            {"Separator", 3},
            {"Dedupe", 4},
            {"Find Filter", 5},
            {"Change Directory", 6},
            {"Toggle Mode", 7},
            {"Exit", 0}
        };

        public static string WorkingDirectory { get; private set; }
        public static bool IsSilent { get; private set; }
        public static bool AltMode { get; private set; }
        private static bool useTimer;
        private static int chosen;

        private static System.Diagnostics.Stopwatch Timer = null;

        static async Task Main(string[] args)
        {
            Console.WriteLine("===== Welcome to Unity Assets Utilities =====");
            WorkingDirectory = string.Empty;

            if (args.Length == 0)
            {
                IsSilent = false;
                useTimer = false;
                AltMode = false;
            }
            else
            {
                IsSilent = args.Contains("silent");
                useTimer = args.Contains("timer");
                AltMode = args.Contains("alt");
            }

            if (useTimer)
            {
                Timer = new System.Diagnostics.Stopwatch();
                Console.WriteLine("\tLaunching with Timer...");
            }

            chosen = 0;

            SetDirectory();
            await ProgramLoop();
        }

        private static async Task ProgramLoop()
        {
            do
            {
                Console.Clear();
                Console.WriteLine($"Working Directory: \"{WorkingDirectory}\"");
                Console.WriteLine($"Mode: {(AltMode ? "Alt." : "Normal")}\n");

                Console.WriteLine($"Choose a Function:");

                foreach (var pair in Modes)
                    Console.WriteLine($"[{pair.Value}] {pair.Key}");

                try { chosen = int.Parse(Console.ReadLine()); }
                catch { chosen = -1; }

                if (chosen < 0 || chosen >= Modes.Count)
                {
                    Console.WriteLine("Invalid Input!");
                    Pause();
                    continue;
                }
                if (chosen == Modes["Flatten Folder"])
                {
                    await FlattenFolder.Process();
                    if (AltMode) AltMode = false;
                    continue;
                }
                if (chosen == Modes["Byte Trimmer"])
                {
                    await ByteTrimmer.Process(); continue;
                }
                if (chosen == Modes["Separator"])
                {
                    await Separator.Process(); continue;
                }
                if (chosen == Modes["Dedupe"])
                {
                    await Dedupe.Process(); continue;
                }
                if (chosen == Modes["Find Filter"])
                {
                    await FindFilter.Process(); continue;
                }
                if (chosen == Modes["Toggle Mode"])
                {
                    AltMode = !AltMode; continue;
                }
                if (chosen == Modes["Change Directory"])
                {
                    WorkingDirectory = null;
                    SetDirectory();
                    if (AltMode) AltMode = false;
                    continue;
                }

            } while (chosen != 0);

            Environment.Exit(0);
        }

        public static void StartOperation() { if (useTimer) Timer.Start(); }
        public static void StopOperation()
        {
            if (!useTimer) return;
            Timer.Stop();
            Console.WriteLine($"Took: {Timer.ElapsedMilliseconds / 1000.0f:N4}s");
        }

        private static void SetDirectory()
        {
            do
            {
                if (WorkingDirectory != string.Empty)
                    Console.WriteLine("Invalid Input!");

                Console.Write("Enter the Path to Assets: ");
                WorkingDirectory = Console.ReadLine();

                if (WorkingDirectory == null)
                    Environment.Exit(-1);

            } while (!Directory.Exists(WorkingDirectory));
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
