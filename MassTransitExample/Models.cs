using System;

namespace MassTransitExample
{
    public class Message
    {
        public string Text => Guid.NewGuid().ToString("N").Substring(0, 10);
    }

    public interface OrderSubmitted
    {
        Guid OrderId { get; }
    }
}