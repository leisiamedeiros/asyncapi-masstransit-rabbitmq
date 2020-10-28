using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace MassTransitExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = Init();

            await RabbitMqConsoleListenerHandler.BusSender(new Message(), config);
            await RabbitMqConsoleListener.OrdersSendReceive.SenderOrder(config);

            await RabbitMqConsoleListenerHandler.BusReceive(config);
            await RabbitMqConsoleListener.OrdersSendReceive.ConsumeOrder(config);
        }

        private static IConfigurationRoot Init()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
        }

    }
}
