using System;
using System.Threading.Tasks;

namespace AngryWasp.Cli.DefaultCommands
{
    [ApplicationCommand("exit", "Exit the program")]
    public class Exit : IApplicationCommand
    {
        public Task<bool> Handle(string command)
        {
            Environment.Exit(0);
            return Task.FromResult(true);
        }
    }
}