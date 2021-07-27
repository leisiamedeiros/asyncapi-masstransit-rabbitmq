using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace MassTransitExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = Init();

            // example 01
            await PublishMessage.BusSender(new Message(), config);
            await PublishMessage.BusReceive(config);

            // example 02
            await OrdersSendReceive.SenderOrder(config);
            await OrdersSendReceive.ConsumeOrder(config);

            // example 03 RoutingKey (Direct Exchange)
            await SubmitOrderKey.SenderSubmitOrder(config);
            await SubmitOrderKey.ConsumeSubmitOrder(config);
        }

        private static IConfigurationRoot Init()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
        }

    }
}
