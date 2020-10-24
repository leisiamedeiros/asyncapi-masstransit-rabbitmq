using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;

namespace MassTransitExample
{
    public class RabbitMqConsoleListenerHandler
    {
        public static async Task BusSender(Message message)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                sbc.Host("rabbitmq://localhost");
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            await bus.StartAsync(source.Token);

            // publish
            await bus.Publish<Message>(message);

            Console.WriteLine("The message was published!");
            await bus.StopAsync();
        }

        public static async Task BusReceive()
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                sbc.Host("rabbitmq://localhost");

                sbc.ReceiveEndpoint("bus_teste", ep =>
                {
                    ep.Handler<Message>(context =>
                    {
                        return Console.Out.WriteLineAsync($"Received: {context.Message.Text}");
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

