namespace Utils.UnityAssets
{
    /// <summary>
    /// Main Console UI
    /// </summary>
    internal class UnityAssetsUtils
    {
        public static string WorkingDirectory { get; private set; }

        private static AvailableModes currentMode;
        private enum AvailableModes
        {
            Byte_Trimmer = 1,
            Dedupe = 2,
            Flatte_Folder = 3,
            Find_Filter = 4,
            Separator = 5,
            Change_Directory = 6,
            Toggle_Mode = 7,
            Exit = 0
        };

        public static bool IsSilent { get; private set; }
        public static bool IsAlt { get; private set; }

        private static bool useTimer;
        private static System.Diagnostics.Stopwatch Timer = null;

        static async Task Main(string[] args)
        {
            Console.WriteLine("===== Welcome to Unity Assets Utilities =====");
            WorkingDirectory = string.Empty;

            IsSilent = args.Contains("silent");
            IsAlt = args.Contains("alt");
            useTimer = args.Contains("timer");

            if (useTimer)
            {
                Timer = new System.Diagnostics.Stopwatch();
                Console.WriteLine("\tLaunching with Timer...");
            }

            WorkingDirectory = SetDirectory(true);

            await ProgramLoop();
        }

        private static void PrintFunctions()
        {
            foreach (var val in Enum.GetValues(typeof(AvailableModes)))
            {
                if ((int)val > 0)
                    Console.WriteLine($"[{(int)val}] {val.ToString().Replace('_', ' ')}");
            }

            Console.WriteLine("[0] Exit");
        }

        private static async Task ProgramLoop()
        {
            bool exit = false;

            do
            {
                Console.Clear();
                Console.WriteLine($"Working Directory: \"{WorkingDirectory}\"");
                Console.WriteLine($"Mode: {(IsAlt ? "Alt." : "Normal")}\n");

                PrintFunctions();

                Console.Write("\nChoose a Function: ");

                if (!Enum.TryParse(Console.ReadLine()?.Trim(), out currentMode) || !Enum.IsDefined(typeof(AvailableModes), currentMode))
                {
                    Console.WriteLine("Invalid Input!");
                    Pause();
                    continue;
                }

                switch (currentMode)
                {
                    default:
                        throw new SystemException();

                    case AvailableModes.Byte_Trimmer:
                        await ByteTrimmer.Process();
                        break;

                    case AvailableModes.Dedupe:
                        await Dedupe.Process();
                        break;

                    case AvailableModes.Find_Filter:
                        await FindFilter.Process();
                        break;

                    case AvailableModes.Flatte_Folder:
                        if (await FlattenFolder.Process())
                            if (IsAlt) IsAlt = false;
                        break;

                    case AvailableModes.Separator:
                        await Separator.Process();
                        break;

                    case AvailableModes.Change_Directory:
                        WorkingDirectory = SetDirectory();
                        if (IsAlt) IsAlt = false;
                        break;

                    case AvailableModes.Toggle_Mode:
                        IsAlt = !IsAlt;
                        break;

                    case AvailableModes.Exit:
                        exit = true;
                        break;
                }
            } while (!exit);

            Environment.Exit(0);
            return;
        }

        /// <summary>
        /// Prevent accidentally messing up a system root folder
        /// </summary>
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
                else if (!init && input.ToLower().Equals("return"))
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
            Console.WriteLine($"\tTook: {Timer.ElapsedMilliseconds / 1000.0f:N4}s");
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
