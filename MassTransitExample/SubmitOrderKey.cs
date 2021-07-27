using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace MassTransitExample
{
    public class SubmitOrderKey
    {
        public static async Task SenderSubmitOrder(IConfiguration configuration)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Send<SubmitOrder>(x =>
                {
                    // use customerType for the routing key
                    x.UseRoutingKeyFormatter(context => context.Message.CustomerType);

                    // multiple conventions can be set, in this case also CorrelationId
                    x.UseCorrelationId(context => context.TransactionId);
                });
                //Keeping in mind that the default exchange config for your published type will be the full typename of your message
                //we explicitly specify which exchange the message will be published to. So it lines up with the exchange we are binding our
                //consumers too.
                cfg.Message<SubmitOrder>(x => x.SetEntityName("submitorder"));
                //Also if your publishing your message: because publishing a message will, by default, send it to a fanout queue. 
                //We specify that we are sending it to a direct queue instead. In order for the routingkeys to take effect.
                cfg.Publish<SubmitOrder>(x => x.ExchangeType = ExchangeType.Direct);
            });

            await Send(bus);
        }

        private static async Task Send(IBusControl bus)
        {
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await bus.StartAsync(source.Token);

            // publish
            await bus.Publish<SubmitOrder>(
                new
                {
                    CustomerType = "PRIORITY",
                    TransactionId = Guid.NewGuid()
                }
            );
            Console.WriteLine("SubmitOrderKey: The SubmitOrder was published!");

            await bus.StopAsync();
        }

        public static async Task ConsumeSubmitOrder(IConfiguration configuration)
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username(configuration["rabbitmq:username"]);
                    h.Password(configuration["rabbitmq:password"]);
                });

                cfg.ReceiveEndpoint("priority-orders", x =>
                {
                    x.ConfigureConsumeTopology = false;

                    x.Consumer<OrderConsumer>();

                    x.Bind("submitorder", s =>
                    {
                        s.RoutingKey = "PRIORITY";
                        s.ExchangeType = ExchangeType.Direct;
                    });
                });

                cfg.ReceiveEndpoint("regular-orders", x =>
                {
                    x.ConfigureConsumeTopology = false;

                    x.Consumer<OrderConsumer>();

                    x.Bind("submitorder", s =>
                    {
                        s.RoutingKey = "REGULAR";
                        s.ExchangeType = ExchangeType.Direct;
                    });
                });
            });

            await Consume(busControl);
        }

        private static async Task Consume(IBusControl busControl)
        {
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await busControl.StartAsync(source.Token);

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();

            await busControl.StopAsync();
        }
    }

    public class OrderConsumer :
        IConsumer<SubmitOrder>
    {
        public async Task Consume(ConsumeContext<SubmitOrder> context)
        {
            Console.WriteLine("OrderConsumer SubmitOrder: {0}", context.Message.CustomerType);
            Console.WriteLine("OrderConsumer SubmitOrder: {0}", context.Message.TransactionId);
            await Task.CompletedTask;
        }
    }
}