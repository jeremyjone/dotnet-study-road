using RabbitMQ.Client;

namespace RabbitMQ.Producer
{
    public static class RabbitMQFactory
    {
        public static IConnection Create()
        {
            return new ConnectionFactory
            {
                UserName = "admin",
                Password = "qaz123",
                HostName = "localhost",
                Port = 5672
            }.CreateConnection();
        }
    }
}
