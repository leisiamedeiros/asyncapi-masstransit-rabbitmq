using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.RabbitMqTransport;
using MasstransitConfig.Consumers;
using MasstransitRabbitmqConfig.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MasstransitConfig.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureRabbitmq(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                AddConsumers(x);

                x.SetKebabCaseEndpointNameFormatter();

                x.UsingRabbitMq((context, cfg) =>
                {
                    ConfigureHost(cfg, configuration);

                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddMassTransitHostedService();

            return services;
        }

        private static void ConfigureHost(IRabbitMqBusFactoryConfigurator cfg, IConfiguration configuration)
        {
            var opt = configuration.GetSection(nameof(RabbitConfigurationOptions)).Get<RabbitConfigurationOptions>();

            cfg.Host(opt.Host, "/", h =>
            {
                h.Username(opt.Username);
                h.Password(opt.Password);
            });
        }

        private static void AddConsumers(IServiceCollectionBusConfigurator config)
        {
            config.AddConsumer<ContactCreatedEventConsumer>();
        }
    }
}
