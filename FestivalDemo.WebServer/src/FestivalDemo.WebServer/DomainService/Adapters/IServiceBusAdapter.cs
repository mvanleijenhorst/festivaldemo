using FestivalDemo.WebServer.Common.Commands;

namespace FestivalDemo.WebServer.DomainService.Adapters
{
    public interface IServiceBusAdapter
    {
        void AddMessage(string queueName, object message);

        bool TryGetMessage(string queueName, out object? message);
    }
}
