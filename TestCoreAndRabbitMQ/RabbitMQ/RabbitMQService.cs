using RabbitMQ.Client;

namespace TestCoreAndRabbitMQ.RabbitMQ
{
    public class RabbitMQService
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly IConnection _connection;
        private readonly IConfiguration _configuration;

        public RabbitMQService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionFactory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQ:HostName"],
                UserName = _configuration["RabbigMQ:UserName"],
                Password = _configuration["RabbigMQ:Password"]
            };
            _connection = _connectionFactory.CreateConnection();
        }

        public IModel CreateChannel()
        {
            return _connection.CreateModel(); 
        }
    }
}
