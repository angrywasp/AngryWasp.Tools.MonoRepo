using System;
using System.Collections.Generic;

namespace AngryWasp.Cli.Prompts
{
    public static class InputPrompt
    {
        public static bool Get(string text, out string selection)
        {
            ConsoleColor startColor = Console.ForegroundColor;
            selection = null;

            ApplicationLogWriter.WriteImmediate($"{text}{Environment.NewLine}", ConsoleColor.Cyan);
            var response = new List<char>();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                ConsoleKeyInfo i = Console.ReadKey(true);
                if (i.Key == ConsoleKey.Enter)
                    break;
                if (i.Key == ConsoleKey.Escape)
                    return false;
                else if (i.Key == ConsoleKey.Backspace)
                {
                    if (response.Count > 0)
                    {
                        response.RemoveAt(response.Count - 1);
                        Console.Write("\b \b");
                    }
                }
                else if (i.KeyChar != '\u0000') // KeyChar == '\u0000' if the key pressed does not correspond to a printable character, e.g. F1, Pause-Break, etc
                {
                    response.Add(i.KeyChar);
                    Console.Write(i.KeyChar);
                }
            }

            Console.WriteLine();

            Console.ForegroundColor = startColor;
            selection = new string(response.ToArray()).Trim();

            return true;
        }
    }
}