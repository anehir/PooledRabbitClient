using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
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
            services.Configure<RabbitOptions>(configuration.GetSection("rabbit"));
            return AddServices(services);
        }

        public static IServiceCollection AddRabbit(this IServiceCollection services, RabbitOptions rabbitOptions)
        {
            services.Configure<RabbitOptions>(o => rabbitOptions.CopyTo(o));
            return AddServices(services);
        }

        private static IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<IPooledObjectPolicy<IModel>, ChannelPooledObjectPolicy>();
            services.AddSingleton<IRabbitManager, RabbitManager>();
            return services;
        }

    }
}
