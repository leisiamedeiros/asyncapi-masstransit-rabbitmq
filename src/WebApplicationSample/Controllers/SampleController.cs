using MassTransit;
using MasstransitConfig.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Polyjuice.Potions;
using Saunter.Attributes;
using System.Threading.Tasks;

namespace WebApplicationSample.Controllers
{
    [ApiController]
    [AsyncApi]
    [Route("api/[controller]")]
    public class SampleController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public SampleController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        /// <summary>
        /// Publish a contact created event
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Channel("MasstransitConfig.Events:ContactCreated")]
        [PublishOperation(typeof(ContactCreated), Summary = "This event represents the creation of a contact")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Publish()
        {
            var contactCreatedEvent = new ContactCreated { Contact = new Contact(Name.FirstName, Name.LastName) };

            await _publishEndpoint.Publish(contactCreatedEvent);

            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
