using System;
using System.Threading.Tasks;

namespace AngryWasp.Cli.DefaultCommands
{
    [ApplicationCommand("help", "Print this help text")]
    public class Help : IApplicationCommand
    {
        public Task<bool> Handle(string command)
        {
            Console.WriteLine();
            foreach (var cmd in Application.Commands)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(cmd.Key);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($": {cmd.Value.Item1}");
            }
            return Task.FromResult(true);
        }
    }
}