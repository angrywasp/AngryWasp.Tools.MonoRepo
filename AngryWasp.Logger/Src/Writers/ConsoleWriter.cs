using System;
using System.IO;

namespace AngryWasp.Logger
{
    public class ConsoleWriter : ILogWriter
    {
        private TextWriter output;
        private ConsoleColor color = ConsoleColor.White;

        public void SetColor(ConsoleColor color)
        {
            this.color = color;
            Console.ForegroundColor = color;
        }

        public ConsoleWriter()
        {
            output = Console.Out;
        }

        public void Close()
        {
            output.Flush();
            output.Close();
        }

        public void WriteInfo(string value)
        {
            Console.ForegroundColor = color;
            output.WriteLine(value);
        }

        public void WriteWarning(string value)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            output.WriteLine(value);
            Console.ForegroundColor = color;
        }

        public void WriteError(string value)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            output.WriteLine(value);
            Console.ForegroundColor = color;
        }
    }
}
