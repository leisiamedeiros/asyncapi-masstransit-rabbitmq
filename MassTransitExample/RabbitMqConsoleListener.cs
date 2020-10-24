using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MassTransitExample;

namespace RabbitMqConsoleListener
{
    public class OrdersSendReceive
    {
        public static async Task SenderOrder()
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                sbc.Host("rabbitmq://localhost");
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            await bus.StartAsync(source.Token);

            // publish
            await bus.Publish<OrderSubmitted>(new { OrderId = Guid.NewGuid() });

            Console.WriteLine("The message was published!");
            await bus.StopAsync();
        }

        public static async Task ConsumeOrder()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
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
                Console.WriteLine("Order Submitted: {0}", context.Message.OrderId);
            }
        }
    }
}