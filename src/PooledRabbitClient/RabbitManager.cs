using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace PooledRabbitClient
{
    /// <summary>
    /// Şu adresten başladım, detaylarına oradan bakabilirsiniz:
    /// https://www.c-sharpcorner.com/article/publishing-rabbitmq-message-in-asp-net-core/
    /// </summary>
    public class RabbitManager : IRabbitManager
    {
        private readonly DefaultObjectPool<IModel> _objectPool;

        public RabbitManager(IPooledObjectPolicy<IModel> objectPolicy, IOptions<RabbitOptions> options)
        {
            _objectPool = new DefaultObjectPool<IModel>(objectPolicy, options.Value.MaxChannelCount);
            
        }

        public void Publish<T>(T message, string exchange, string routeKey) where T : class
        {
            if (message == null)
            {
                return;
            }
            var bytesToSend = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            using var channel = new PoolObject<IModel>(_objectPool);
            var properties = channel.Item.CreateBasicProperties();
            properties.Persistent = true;
            channel.Item.BasicPublish(exchange, routeKey, properties, bytesToSend);
        }

        public void ExchangeDeclare(string exchange, string type, bool durable, bool autoDelete, IDictionary<string, object> arguments)
        {
            using var channel = new PoolObject<IModel>(_objectPool);
            channel.Item.ExchangeDeclare(exchange, type, durable, autoDelete, arguments);
        }

        public QueueDeclareOk QueueDeclare(string queue, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object> arguments)
        {
            using var channel = new PoolObject<IModel>(_objectPool);
            return channel.Item.QueueDeclare(queue, durable, exclusive, autoDelete, arguments);
        }

    }

}
