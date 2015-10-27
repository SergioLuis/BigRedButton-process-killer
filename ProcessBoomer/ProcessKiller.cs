using System;
using System.ComponentModel;
using System.Diagnostics;

namespace ProcessBoomer
{
    internal static class ProcessKiller
    {
        internal struct KilledProcess
        {
            internal int Id;
            internal string Name;
        }

        internal static KilledProcess KillRandomProcess()
        {
            InitRandom();

            Process[] processList = Process.GetProcesses();
            int badLuckProcess = mRand.Next(0, processList.Length - 1);

            try
            {
                Process processToKill = processList[badLuckProcess];
                KilledProcess killedProcessInfo = new KilledProcess
                {
                    Id = processToKill.Id,
                    Name = processToKill.ProcessName
                };

                processToKill.Kill();
                processToKill.WaitForExit();

                mRetries = 0;

                return killedProcessInfo;
            }
            catch (Win32Exception)
            {
                mRetries++;

                if (mRetries == MAX_RETRIES)
                {
                    string errorMessage = string.Format(
                        "Max retries ({0}) reached. Could not kill process.",
                        MAX_RETRIES);
                    throw new Exception(errorMessage);
                }

                // The process was being killed before we tried to.
                // We don't have permission to kill the selected process.
                // Let's try again, shall we?
                return KillRandomProcess();
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format(
                    "Something bad happened while trying to kill process: {0}",
                    ex.Message);
                throw new Exception(errorMessage);
            }
            
        }

        static void InitRandom()
        {
            if (mRand != null)
                return;

            mRand = new Random(666);
        }

        static Random mRand;
        static int mRetries = 0;
        static readonly int MAX_RETRIES = 3;
    }
}
