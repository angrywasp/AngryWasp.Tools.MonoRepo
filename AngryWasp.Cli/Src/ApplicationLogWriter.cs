using AngryWasp.Logger;
using System;
using System.Collections.Generic;

namespace AngryWasp.Cli
{
    public class ApplicationLogWriter : ILogWriter
    {
        private List<(ConsoleColor, string)> buffer;

        private ConsoleColor color = ConsoleColor.White;

        public List<(ConsoleColor, string)> Buffer => buffer;

        private static ApplicationLogWriter attachedLogWriter;

        public static bool HideInfo { get; set; } = true;

        public void SetColor(ConsoleColor color)
        {
            this.color = color;
        }

        public ApplicationLogWriter(List<(ConsoleColor, string)> buffer)
        {
            this.buffer = buffer;
        }

        public void Close() { }

        public static void WriteBuffered(string message, ConsoleColor color = ConsoleColor.White)
        {
            if (attachedLogWriter == null)
                attachedLogWriter = (ApplicationLogWriter)Log.Instance.GetWriter("buffer");

            attachedLogWriter.Buffer.Add((color, message));
        }

        public static void WriteImmediate(string message) => Console.WriteLine(message);

        public static void WriteImmediate(string message, ConsoleColor color)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ForegroundColor = oldColor;
        }

        public void WriteInfo(string value)
        {
            if (HideInfo)
                return;

            value += Environment.NewLine;
            buffer.Add((color, value));
        }

        public void WriteWarning(string value)
        {
            value += Environment.NewLine;
            buffer.Add((ConsoleColor.Yellow, value));
        }

        public void WriteError(string value)
        {
            value += Environment.NewLine;
            buffer.Add((ConsoleColor.Red, value));
        }
    }
}
