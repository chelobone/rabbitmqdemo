using MassTransit;
using Queues.Rabbit.Shared;

namespace Queues.Rabbit.Consumers.Consumer
{
    public class ConsumerNotification : IConsumer<Notification>
    {
        public async Task Consume(ConsumeContext<Notification> context)
        {
            var data = context.Message;

            if (data != null)
            {

            }
        }
    }
}
