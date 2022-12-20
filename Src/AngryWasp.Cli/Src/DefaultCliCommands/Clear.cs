using AngryWasp.Logger;
using System.Reflection;
using System.Threading.Tasks;

namespace AngryWasp.Cli.DefaultCommands
{
    [ApplicationCommand("clear", "Clears the console")]
    public class Clear : IApplicationCommand
    {
        public Task<bool> Handle(string command)
        {
            Application.Clear();
            var an = Assembly.GetEntryAssembly().GetName();
            Log.Instance.WriteInfo($"{an.Name}: {an.Version}");
            return Task.FromResult(true);
        }
    }
}
