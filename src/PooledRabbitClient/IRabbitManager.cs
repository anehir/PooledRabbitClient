using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

namespace PooledRabbitClient
{
    public interface IRabbitManager
    {
        void Publish<T>(T message, string exchange, string routeKey) where T : class;
        void ExchangeDeclare(string exchange, string type, bool durable, bool autoDelete, IDictionary<string, object> arguments);
        QueueDeclareOk QueueDeclare(string queue, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object> arguments);
    }

}
