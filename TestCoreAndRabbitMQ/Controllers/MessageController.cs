using System.Text;
using System.Threading.Channels;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TestCoreAndRabbitMQ.RabbitMQ;

namespace TestCoreAndRabbitMQ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly RabbitMQService _rabbitMQService;
        public MessageController(RabbitMQService rabbitMQService)
        {
            _rabbitMQService = rabbitMQService;
        }

        [HttpPost]
        public IActionResult PostMessage([FromBody] string message)
        {
            using (var channel = _rabbitMQService.CreateChannel())
            {
                channel.QueueDeclare(queue: "my_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: " ", routingKey: "my_queue", basicProperties: null, body: body);

            }
            return Ok("Message published successfully");
        }

        [HttpGet]
        public IActionResult GetMessages() 
        {
            using(var channel = _rabbitMQService.CreateChannel())
            {
                channel.QueueDeclare(queue: "my_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(message);
                };

                channel.BasicConsume(queue: "my_queue", autoAck: true, consumer: consumer);
            }

            // Return stored messages or any other logic based on your requirements
            return Ok("Messages retrieved successfully");
        }
    }
}
