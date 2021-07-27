using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MassTransitExample;
using Microsoft.Extensions.Configuration;

namespace MassTransitExample
{
    public class OrdersSendReceive
    {
        public static async Task SenderOrder(IConfiguration configuration)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                sbc.Host(configuration["rabbitmq:url"]);
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            await bus.StartAsync(source.Token);

            // publish
            await bus.Publish<OrderSubmitted>(new { OrderId = Guid.NewGuid(), Name = "MyOrder" });

            Console.WriteLine("SenderOrder: The message was published!");
            await bus.StopAsync();
        }

        public static async Task ConsumeOrder(IConfiguration configuration)
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username(configuration["rabbitmq:username"]);
                    h.Password(configuration["rabbitmq:password"]);
                });

                cfg.ReceiveEndpoint("order-events-listener", e =>
                {
                    e.Consumer<OrderSubmittedEventConsumer>();
                });
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            try
            {
                Console.WriteLine("Press enter to exit");

                await Task.Run(() => Console.ReadLine());
            }
            finally
            {
                await busControl.StopAsync();
            }
        }

        class OrderSubmittedEventConsumer :
            IConsumer<OrderSubmitted>
        {
            public async Task Consume(ConsumeContext<OrderSubmitted> context)
            {
                Console.WriteLine("ConsumeOrder: {0}", context.Message.OrderId);
                Console.WriteLine("ConsumeOrder: {0}", context.Message.Name);
                await Task.CompletedTask;
            }
        }
    }
}