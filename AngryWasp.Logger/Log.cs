using System;
using System.Collections.Generic;

namespace AngryWasp.Logger
{
    public interface ILogWriter
    {
        void WriteInfo(string value);

        void WriteWarning(string value);

        void WriteError(string value);

        void Close();

        void SetColor(ConsoleColor color);
    }

    public class Log
    {
        public const string LineBreakDouble = "=========================================================================";
        public const string LineBreakSingle = "-------------------------------------------------------------------------";

        private Dictionary<string, ILogWriter> writers = new Dictionary<string, ILogWriter>();

        public bool CrashOnError { get; set; } = false;

        private static Log instance = null;

        public static Log Instance => instance;

        public static Log CreateInstance(bool consoleOut = false)
        {
            if (instance != null)
                return instance;

            instance = new Log(consoleOut);
            return instance;
        }

        public Log(bool consoleOut = false)
        {
            if (consoleOut)
                AddWriter("console", new ConsoleWriter());
        }

        //todo: this will reset the console color. 
        //need to fix that
        public bool SupressConsoleOutput
        {
            set
            {
                if (value)
                    RemoveWriter("console");
                else
                    AddWriter("console", new ConsoleWriter());
            }
        }

        public void AddWriter(string name, ILogWriter writer)
        {
            if (writers.ContainsKey(name))
            {
                writers[name] = writer;
                return;
            }

            writers.Add(name, writer);
        }

        public bool RemoveWriter(string name)
        {
            if (!writers.ContainsKey(name))
                return false;

            writers.Remove(name);

            return true;
        }

        public ILogWriter GetWriter(string name)
        {
            if (!writers.ContainsKey(name))
                return null;

            return writers[name];
        }

        public void SetColor(ConsoleColor color)
        {
            foreach (var w in writers.Values)
                w.SetColor(color);
        }

        public void Write(string message)
        {
            foreach (ILogWriter writer in writers.Values)
                writer.WriteInfo(message);
        }

        public string WriteInfo(string message)
        {
            message = $"Info: {message}";
            foreach (ILogWriter writer in writers.Values)
                writer.WriteInfo(message);

            return message;
        }

        public string WriteWarning(string message)
        {
            message = $"Warning: {message}";
            foreach (ILogWriter writer in writers.Values)
                writer.WriteWarning(message);

            return message;
        }

        public string WriteError(string message)
        {
            message = $"Error: {message}";
            foreach (ILogWriter writer in writers.Values)
                writer.WriteError(message);

            if (CrashOnError)
            {
                Shutdown();
                Environment.Exit(0);
            }

            return message;
        }

        public void WriteFatal(string message)
        {
            message = $"Fatal: {message}";
            foreach (ILogWriter writer in writers.Values)
                writer.WriteError(message);

            Shutdown();
            Environment.Exit(0);
        }

        public Exception WriteException(Exception ex, string additionalMessage = null)
        {
            WriteExceptionInternal(ex, additionalMessage);
            return ex;
        }


        public Exception WriteFatalException(Exception ex, string additionalMessage = null)
        {
            WriteException(ex, additionalMessage);
            WriteFatal("Application has encountered a fatal exception");
            return ex;
        }

        private string WriteExceptionInternal(Exception ex, string additionalMessage = null)
        {
            if (additionalMessage == null)
            {
                string s = WriteError($"{ex.Message}\r\nStack:\r\n{ex.StackTrace}");
                if (ex.InnerException != null)
                    s += WriteExceptionInternal(ex.InnerException);

                return s;
            }
            else
            {
                string s = WriteError($"{additionalMessage}\r\nException:\r\n{ex.Message}\r\nStack:\r\n{ex.StackTrace}");
                if (ex.InnerException != null)
                    s += WriteExceptionInternal(ex.InnerException, additionalMessage);

                return s;
            }
        }

        public void Shutdown()
        {
            foreach (ILogWriter writer in writers.Values)
                writer.Close();
        }
    }
}
