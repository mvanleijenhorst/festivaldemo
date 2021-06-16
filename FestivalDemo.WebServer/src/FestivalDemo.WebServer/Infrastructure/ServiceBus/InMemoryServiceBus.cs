using System.Collections.Concurrent;
using FestivalDemo.WebServer.Common;
using FestivalDemo.WebServer.Common.Commands;
using FestivalDemo.WebServer.DomainService.Adapters;

namespace FestivalDemo.WebServer.Infrastructure.InMemory
{
    public class InMemoryServiceBus: IServiceBusAdapter
    {
        public ConcurrentQueue<object> _incomingQueue;
        public ConcurrentQueue<object> _outgoingQueue;

        public InMemoryServiceBus()
        {
            _incomingQueue = new ConcurrentQueue<object>();
            _outgoingQueue = new ConcurrentQueue<object>();
        }

        public void AddMessage(string queueName, object message)
        {
            if (QueueConstant.IncomingQueue.Equals(queueName))
            {
                _incomingQueue.Enqueue(message);
            }

            if (QueueConstant.OutgoingQueue.Equals(queueName))
            {
                _outgoingQueue.Enqueue(message);
            }
        }

        public bool TryGetMessage(string queueName, out object? message)
        {
            if (QueueConstant.IncomingQueue.Equals(queueName))
            {
                return _incomingQueue.TryDequeue(out message);
            }

            if (QueueConstant.OutgoingQueue.Equals(queueName))
            { 
                return _outgoingQueue.TryDequeue(out message);
            }

            message = null;
            return false;
        }
    }
}
