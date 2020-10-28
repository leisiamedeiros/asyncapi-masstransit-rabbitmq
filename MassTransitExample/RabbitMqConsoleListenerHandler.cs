using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace MassTransitExample
{
    public class RabbitMqConsoleListenerHandler
    {
        public static async Task BusSender(Message message, IConfiguration _configuration)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                sbc.Host(_configuration["rabbitmq:url"]);
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            await bus.StartAsync(source.Token);

            // publish
            await bus.Publish<Message>(message);

            Console.WriteLine("BusSender: The message was published!");
            await bus.StopAsync();
        }

        public static async Task BusReceive(IConfiguration _configuration)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                sbc.Host(_configuration["rabbitmq:url"]);

                sbc.ReceiveEndpoint("bus_teste", ep =>
                {
                    ep.Handler<Message>(context =>
                    {
                        return Console.Out.WriteLineAsync($"BusReceive Received: {context.Message.Text}");
                    });
                });
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            await bus.StartAsync(source.Token);

            try
            {
                Console.WriteLine("Press enter to exit");
                await Task.Run(() => Console.ReadLine());
            }
            finally
            {
                await bus.StopAsync();
            }
        }

    }

}

