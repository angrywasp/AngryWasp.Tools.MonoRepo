using System.Threading.Tasks;

namespace AngryWasp.Cli
{
    public interface IApplicationCommand
    {
        Task<bool> Handle(string command);
    }
}