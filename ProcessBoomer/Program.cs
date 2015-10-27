using System;

using DreamCheeky;

using static ProcessBoomer.ProcessKiller;

namespace ProcessBoomer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var redButton = new BigRedButton())
            {
                redButton.ButtonPressed += (sender, eventArgs) =>
                {
                    TryKillProcess();
                };

                Console.ReadLine();
            }
        }

        static void TryKillProcess()
        {
            try
            {
                KillProcess();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        static void KillProcess()
        {
            KilledProcess process = ProcessKiller.KillRandomProcess();
            Console.WriteLine(string.Format(
                KILLED_PROCESS_INFORMATION, process.Name, process.Id));
        }

        static readonly string KILLED_PROCESS_INFORMATION =
            @"Killed ""{0}"" with ID {1}";
    }
}
