using System.Text;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using TestCoreAndRabbitMQ.RabbitMQ;

namespace TestCoreAndRabbitMQ.Controllers
{
    public class RabbitMQController : Controller
    {
        private readonly RabbitMQService _rabbitMQService;
        public RabbitMQController(RabbitMQService rabbitMQService)
        {
            _rabbitMQService = rabbitMQService;
        }

        public IActionResult Index()
        {
            using (var channel = _rabbitMQService.CreateChannel())
            {
                channel.QueueDeclare(queue: "my_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                string message = "Hello RabbitMQ!";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "", routingKey: "my_queue", basicProperties: null, body: body);
            }
            return View();
        }
    }

    
}
