using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AngryWasp.Logger
{
    public class FileLogWriter : ILogWriter
    {
        private StreamWriter output;
        private static SemaphoreSlim writeLock = new SemaphoreSlim(1, 1);

        public FileLogWriter(string logFilePath)
        {
            output = new StreamWriter(new FileStream(logFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite));
        }

        public void WriteInfo(string value)
        {
            Task.Run(async () => {
                await writeLock.WaitAsync().ConfigureAwait(false);
                try
                {
                    value = value.Trim();
                    if (string.IsNullOrEmpty(value))
                        return;

                    output.WriteLine(value);
                    output.Flush();
                }
                finally { writeLock.Release(); }
            });
        }

        public void WriteWarning(string value) => WriteInfo(value);

        public void WriteError(string value) => WriteInfo(value);

        public void Close()
        {
            output.Flush();
            output.Close();
        }

        public void SetColor(ConsoleColor color) { }
    }
}
