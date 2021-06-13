using System.Threading;
using System.Threading.Tasks;

namespace FestivalDemo.WebServer.Common.Commands
{
    public interface ICommandDispatcher<T>
    {
        void Dispatch(T command);
    }
}
