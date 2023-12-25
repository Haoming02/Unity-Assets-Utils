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
            Exit = 0,
            Byte_Trimmer = 1,
            Flatte_Folder = 2,
            Find_Filter = 3,
            Find_File = 4,
            Separator = 5,
            Dedupe = 6,
            Change_Directory = 7,
            Toggle_Mode = 8,
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
            CheckStructure();

            await ProgramLoop();
        }

        private static void ListFunctions()
        {
            foreach (var val in Enum.GetValues(typeof(AvailableModes)))
            {
                if ((int)val > 0)
                    Console.WriteLine($"[{(int)val}] {val.ToString().Replace('_', ' ')}");
            }

            Console.WriteLine("[0] Exit");
        }

        private static bool FunctionSelection()
        {
            string id = Console.ReadLine()?.Trim();

            if (!Enum.TryParse(id, out currentMode))
                return false;

            if (!Enum.IsDefined(typeof(AvailableModes), currentMode))
                return false;

            return true;
        }

        private static async Task ProgramLoop()
        {
            bool exit = false;

            do
            {
                Console.Clear();
                Console.WriteLine($"Working Directory: \"{WorkingDirectory}\"");
                Console.WriteLine($"Mode: {(IsAlt ? "Alt." : "Normal")}\n");

                ListFunctions();

                Console.Write("\nChoose a Function: ");

                if (!FunctionSelection())
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

                    case AvailableModes.Find_File:
                        FindFile.Process();
                        break;

                    case AvailableModes.Flatte_Folder:
                        if (await FlattenFolder.Process())
                            if (IsAlt) IsAlt = false;
                        break;

                    case AvailableModes.Separator:
                        Separator.Process();
                        break;

                    case AvailableModes.Change_Directory:
                        WorkingDirectory = SetDirectory();
                        if (IsAlt) IsAlt = false;
                        CheckStructure();
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

                string input = Console.ReadLine()?.Trim('"').Trim();

                if (input.Length > SAFE_GUARD && Directory.Exists(input))
                    return input;
                else if (!init && input.ToLower().Equals("return"))
                    return WorkingDirectory;

                Console.WriteLine("Invalid Path...");
                Pause();
                Console.Clear();
            } while (true);
        }

        private static void CheckStructure()
        {
            bool maybeAlt = CommonFuncs.DetectStructure();

            if (maybeAlt && !IsAlt)
            {
                Console.WriteLine($"\n\tWorkingDirectory seems to be in Alt. structure;\n\tRecommended to switch mode with [{(int)AvailableModes.Toggle_Mode}] first!\n");
                Pause();
            }

            if (!maybeAlt && IsAlt)
            {
                Console.WriteLine($"\n\tWorkingDirectory doesn't seems to be in Alt. structure...\n\tRecommended to switch mode with [{(int)AvailableModes.Toggle_Mode}] first!\n");
                Pause();
            }
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
