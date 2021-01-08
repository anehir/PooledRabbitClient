using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace PooledRabbitClient
{
    public static class RabbitServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbit(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitConfig = configuration.GetSection("rabbit");
            services.Configure<RabbitOptions>(rabbitConfig);
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<IPooledObjectPolicy<IModel>, ChannelPooledObjectPolicy>();
            services.AddSingleton<IRabbitManager, RabbitManager>();
            return services;
        }

        public static IServiceCollection AddRabbit(this IServiceCollection services, RabbitOptions rabbitOptions)
        {
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<IPooledObjectPolicy<IModel>, ChannelPooledObjectPolicy>(sp => {
                return new ChannelPooledObjectPolicy(rabbitOptions);
            });
            services.AddSingleton<IRabbitManager, RabbitManager>();
            return services;
        }
    }
}
