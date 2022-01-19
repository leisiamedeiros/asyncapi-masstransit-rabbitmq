using MassTransit;
using MasstransitConfig.Events;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Saunter.Attributes;
using System.Threading.Tasks;

namespace MasstransitConfig.Consumers
{
    [AsyncApi]
    public class ContactCreatedEventConsumer : IConsumer<ContactCreated>
    {
        ILogger<ContactCreatedEventConsumer> _logger;

        public ContactCreatedEventConsumer(ILogger<ContactCreatedEventConsumer> logger)
        {
            _logger = logger;
        }

        [Channel("contact-created-event")]
        [SubscribeOperation(typeof(ContactCreated), Summary = "Subscribe to a ContactCreated event")]
        public async Task Consume(ConsumeContext<ContactCreated> context)
        {
            var contact = JsonConvert.SerializeObject(context.Message.Contact);

            _logger.LogInformation("Contact: {0}", contact);

            await Task.CompletedTask;
        }
    }
}
