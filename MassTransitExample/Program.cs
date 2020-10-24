using System.Threading.Tasks;

namespace MassTransitExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await RabbitMqConsoleListenerHandler.BusSender(new Message());
            await RabbitMqConsoleListener.OrdersSendReceive.SenderOrder();

            await RabbitMqConsoleListenerHandler.BusReceive();
            await RabbitMqConsoleListener.OrdersSendReceive.ConsumeOrder();
        }

    }
}
