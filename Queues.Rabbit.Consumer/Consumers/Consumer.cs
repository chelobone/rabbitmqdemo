using MassTransit;
using Queues.Rabbit.Shared;

namespace Queues.Rabbit.Consumers.Consumer
{
    public class Consumer : IConsumer<Client>
    {
        public async Task Consume(ConsumeContext<Client> context)
        {
            var data = context.Message;

            if (data != null)
            {
                //context.
                new DAClient().ConnectionToDatabase(data);
            }
        }
    }
}
