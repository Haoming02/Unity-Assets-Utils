namespace Utils.UnityAssets
{
    /// <summary>Main Console UI</summary>
    internal class UnityAssetsUtils
    {
        internal static string WorkingDirectory { get; private set; }
        internal static bool IsSilent { get; private set; }
        internal static bool IsAlt { get; private set; }

        private enum Operations
        {
            Exit,
            Byte_Trimmer,
            Flatten_Folder,
            Find_Filter,
            Find_File,
            Separator,
            Dedupe,
            Change_Directory,
            Toggle_Mode,
        };

        static void Main(string[] args)
        {
            Console.WriteLine("===== Welcome to Unity Assets Utilities =====");

            IsSilent = args.Contains("silent");
            IsAlt = args.Contains("alt");

            WorkingDirectory = SetDirectory(init: true);
            VerifyStructure();

            ProgramLoop();
            Environment.Exit(0);
        }

        internal static string SetDirectory(bool init = false)
        {
            // Prevent accidentally modifying a system root folder
            const int SAFE_GUARD = 4;

            while (true)
            {
                if (init)
                    Console.Write("Enter the Path to Assets: ");
                else
                    Console.Write("Enter the Path to Assets (Enter \"return\" to Cancel): ");

                string input = Console.ReadLine().Trim().Trim('"').Trim();

                if (!init && input.ToLower().Equals("return"))
                    return WorkingDirectory;
                if (input.Length > SAFE_GUARD && Directory.Exists(input))
                    return input;

                Console.WriteLine("Path is Invalid...");
                Pause();
                Console.Clear();
            }
        }

        private static void VerifyStructure()
        {
            bool data = Directory.EnumerateFiles(WorkingDirectory, "__data", SearchOption.AllDirectories).Any();
            bool info = Directory.EnumerateFiles(WorkingDirectory, "__info", SearchOption.AllDirectories).Any();
            bool alt = data && info;

            if (alt && !IsAlt)
            {
                Console.WriteLine("\n\tWorkingDirectory seems to be in Alt. structure");
                Console.WriteLine($"\tRecommended to switch mode with [{(int)Operations.Toggle_Mode}] first!\n");
                Pause();
            }

            if (!alt && IsAlt)
            {
                Console.WriteLine("\n\tWorkingDirectory doesn't seem to be in Alt. structure");
                Console.WriteLine($"\tRecommended to switch mode with [{(int)Operations.Toggle_Mode}] first!\n");
                Pause();
            }
        }

        private static void ProgramLoop()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine($"Working Directory: \"{WorkingDirectory}\"");
                Console.Write(IsSilent ? "Silent ; " : string.Empty);
                Console.WriteLine($"Mode: {(IsAlt ? "Alt." : "Normal")}\n");

                ListFunctions();

                Console.Write("\nChoose a Function: ");

                if (!SelectFunction(out Operations currentOperation))
                {
                    Console.WriteLine("Invalid ID...");
                    Pause();
                    continue;
                }

                switch (currentOperation)
                {
                    case Operations.Byte_Trimmer:
                        ByteTrimmer.Process();
                        break;

                    case Operations.Dedupe:
                        Dedupe.Process();
                        break;

                    case Operations.Find_Filter:
                        FindFilter.Process();
                        break;

                    case Operations.Find_File:
                        FindFile.Process();
                        break;

                    case Operations.Flatten_Folder:
                        if (FlattenFolder.Process())
                            IsAlt = false;
                        break;

                    case Operations.Separator:
                        Separator.Process();
                        break;

                    case Operations.Change_Directory:
                        WorkingDirectory = SetDirectory();
                        VerifyStructure();
                        break;

                    case Operations.Toggle_Mode:
                        IsAlt = !IsAlt;
                        VerifyStructure();
                        break;

                    case Operations.Exit:
                        exit = true;
                        break;

                    default:
                        throw new SystemException();
                }
            }
        }

        private static void ListFunctions()
        {
            string[] ops = Enum.GetNames(typeof(Operations));
            for (int i = 1; i < ops.Length; i++)
                Console.WriteLine($"[{i}] {ops[i].Replace('_', ' ')}");
            Console.WriteLine($"[0] {ops[0]}");
        }

        private static bool SelectFunction(out Operations currentOperation)
        {
            string id = Console.ReadLine().Trim();
            if (!Enum.TryParse(id, out currentOperation)) return false;
            if (!Enum.IsDefined(typeof(Operations), currentOperation)) return false;
            return true;
        }

        public static void StartOperation() { }
        public static void StopOperation() { }

        internal static void Pause(bool force = false)
        {
            if (IsSilent && !force) return;
            Console.WriteLine("Press ENTER to Continue...");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
        }
    }
}
