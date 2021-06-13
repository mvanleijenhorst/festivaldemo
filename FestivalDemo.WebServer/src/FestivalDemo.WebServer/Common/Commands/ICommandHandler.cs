using System.Threading.Tasks;

namespace FestivalDemo.WebServer.Common.Commands
{
    public interface ICommandHandler<T>
    {
        void HandleCommand(T command);
    }
}
