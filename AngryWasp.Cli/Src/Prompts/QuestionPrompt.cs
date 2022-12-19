using AngryWasp.Cli;
using System;
using System.Collections.Generic;

namespace AngryWasp.Cli.Prompts
{
    public enum QuestionPrompt_Response
    {
        Undefined,
        Yes,
        No
    }

    public static class QuestionPrompt
    {
        public static bool Get(string text, out QuestionPrompt_Response answer)
        {
            ConsoleColor startColor = Console.ForegroundColor;
            answer = QuestionPrompt_Response.Undefined;

            text += " (Yes/No)";
            ApplicationLogWriter.WriteImmediate($"{text}{Environment.NewLine}", ConsoleColor.Cyan);

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
                string rString = new string(response.ToArray()).ToLower();
                switch (rString)
                {
                    case "yes":
                    case "y":
                        {
                            Console.ForegroundColor = startColor;
                            answer = QuestionPrompt_Response.Yes;
                            return true;
                        }
                    case "no":
                    case "n":
                        {
                            Console.ForegroundColor = startColor;
                            answer = QuestionPrompt_Response.No;
                            return true;
                        }
                    default: break;
                }

                response.Clear();
                ApplicationLogWriter.WriteImmediate($"Invalid option: {rString}");
                ApplicationLogWriter.WriteImmediate($"{text}{Environment.NewLine}", ConsoleColor.Cyan);

                continue;
            }
        }
    }
}