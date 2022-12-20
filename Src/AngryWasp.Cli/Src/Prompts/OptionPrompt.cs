using System;
using System.Collections.Generic;
using System.Linq;

namespace AngryWasp.Cli.Prompts
{
    public static class OptionPrompt
    {
        public static bool Get(string text, IEnumerable<object> options, out int selection)
        {
            ConsoleColor startColor = Console.ForegroundColor;
            selection = -1;

            text += $" (1 - {options.Count()})";
            ApplicationLogWriter.WriteImmediate($"{text}{Environment.NewLine}", ConsoleColor.Cyan);

            for (int i = 0; i < options.Count(); i++)
                ApplicationLogWriter.WriteImmediate($"{i + 1}: {options.ElementAt(i)}{Environment.NewLine}", ConsoleColor.Cyan);

            int result = 0;

            var response = new List<char>();
            while (true)
            {
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
                string rString = new string(response.ToArray());
                if (int.TryParse(rString, out result) && result <= options.Count())
                {
                    selection = result - 1;
                    Console.ForegroundColor = startColor;
                    return true;
                }

                response.Clear();
                ApplicationLogWriter.WriteImmediate($"Invalid option: {rString}");
                ApplicationLogWriter.WriteImmediate($"{text}{Environment.NewLine}", ConsoleColor.Cyan);

                continue;
            }
        }
    }
}