
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TestCoreAndRabbitMQ.RabbitMQ
{
    public class MessageConsumerService : BackgroundService
    {
        private readonly RabbitMQService _rabbitmqService;

        public MessageConsumerService(RabbitMQService rabbitmqService)
        {
            _rabbitmqService = rabbitmqService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var channel = _rabbitmqService.CreateChannel())
            {
                channel.QueueDeclare(queue: "my_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine("Recieved message: {0}", message);
                };
                channel.BasicConsume(queue: "my_queue", autoAck: true, consumer: consumer);
                await Task.CompletedTask;
            }
        }
    }
}
