using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;

namespace RabbitMQ.Producer
{
    public class Send
    {
        public static void Basic()
        {
            // 创建连接
            using var connection = RabbitMQFactory.Create();

            // 创建通道
            using var channel = connection.CreateModel();

            #region 声明队列

            #region 标准声明

            // 1、声明交换机
            // channel.ExchangeDeclare("", ExchangeType.Direct, true, false, null);

            // 2、声明队列
            // channel.QueueDeclare("queue", false, false, false, null);

            // 3、绑定
            // channel.QueueBind("queue", "", "queue");

            #endregion

            // 直接声明队列，交换机将使用默认配置
            channel.QueueDeclare("queue", false, false, false, null);

            #endregion


            #region 消息持久化

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true; // 设置持久化

            #endregion

            // 循环发送50个消息到 RabbitMQ
            const string message = "Producer RabbitMQ Message";
            for (var i = 0; i < 50; i++)
            {
                var body = Encoding.UTF8.GetBytes($"{message}-{i}");
                // 发送消息到队列
                channel.BasicPublish("", "queue", properties, body);
                Console.WriteLine($"发布消息 {message}-{i} 到队列。");
                Thread.Sleep(1000);
            }
        }

        public static void Worker()
        {
            const string queueName = "worker_queue";
            using var connection = RabbitMQFactory.Create();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queueName, false, false, false, null);

            // 循环发送100个消息到 RabbitMQ
            const string message = "Producer RabbitMQ Message";
            for (var i = 0; i < 50; i++)
            {
                var body = Encoding.UTF8.GetBytes($"{message}-{i}");
                // 发送消息到队列
                channel.BasicPublish("", queueName, null, body);
                Console.WriteLine($"发布消息 {message}-{i} 到队列。");
            }
        }

        public static void Fanout()
        {
            const string exchangeName = "fanout_exchange";
            const string queueName1 = "fanout_queue1";
            const string queueName2 = "fanout_queue2";
            const string queueName3 = "fanout_queue3";
            using var connection = RabbitMQFactory.Create();
            using var channel = connection.CreateModel();

            // 声明交换机
            channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout);

            // 声明3个队列
            channel.QueueDeclare(queueName1, false, false, false, null);
            channel.QueueDeclare(queueName2, false, false, false, null);
            channel.QueueDeclare(queueName3, false, false, false, null);

            // 将队列绑定到交换机。不写 routingKey，意味着消息将发送到所有队列
            channel.QueueBind(queueName1, exchangeName, "");
            channel.QueueBind(queueName2, exchangeName, "");
            channel.QueueBind(queueName3, exchangeName, "");

            // 循环发送100个消息到 RabbitMQ
            const string message = "Producer RabbitMQ Message";
            for (var i = 1; i <= 20; i++)
            {
                var body = Encoding.UTF8.GetBytes($"{message}-{i}");
                // 发送消息到交换机
                channel.BasicPublish(exchangeName, "", null, body);
                Console.WriteLine($"发布消息 {message}-{i} 到队列。");
            }
        }

        public static void Direct()
        {
            const string exchangeName = "direct_exchange";
            const string queueName1 = "direct_queue1";
            const string queueName2 = "direct_queue2";
            const string queueName3 = "direct_queue3";
            using var connection = RabbitMQFactory.Create();
            using var channel = connection.CreateModel();

            // 声明交换机
            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);

            // 声明3个队列
            channel.QueueDeclare(queueName1, false, false, false, null);
            channel.QueueDeclare(queueName2, false, false, false, null);
            channel.QueueDeclare(queueName3, false, false, false, null);

            // 将队列绑定到交换机。为了达到直发效果，需要填写 routingKey，会按照该 Key 值匹配发送
            channel.QueueBind(queueName1, exchangeName, "c");
            channel.QueueBind(queueName2, exchangeName, "cpp");
            channel.QueueBind(queueName3, exchangeName, "csharp");

            // 循环发送100个消息到 RabbitMQ
            const string message = "Producer RabbitMQ Message";
            for (var i = 1; i <= 20; i++)
            {
                var body = Encoding.UTF8.GetBytes($"{message}-{i}");

                var routingKeys = new[] {"c", "cpp", "csharp"};
                // 发送消息到交换机
                channel.BasicPublish(exchangeName, routingKeys[i % 3], null, body);
                Console.WriteLine($"发布消息 {message}-{i} 到队列。");
            }
        }

        public static void Topic()
        {
            const string exchangeName = "topic_exchange";
            const string queueName1 = "topic_queue1";
            const string queueName2 = "topic_queue2";
            const string queueName3 = "topic_queue3";
            using var connection = RabbitMQFactory.Create();
            using var channel = connection.CreateModel();

            // 声明交换机
            channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);

            // 声明3个队列
            channel.QueueDeclare(queueName1, false, false, false, null);
            channel.QueueDeclare(queueName2, false, false, false, null);
            channel.QueueDeclare(queueName3, false, false, false, null);

            // 将队列绑定到交换机。Topic 模式下，绑定时可以填写模糊符号 "*" / "#"
            channel.QueueBind(queueName1, exchangeName, "data.*");
            channel.QueueBind(queueName2, exchangeName, "data.#");
            channel.QueueBind(queueName3, exchangeName, "data.update");

            // 循环发送100个消息到 RabbitMQ
            const string message = "Producer RabbitMQ Message";


            foreach (var key in new [] {"data.update", "data.insert", "data.insert.one"})
            {
                for (var i = 1; i <= 10; i++)
                {
                    var body = Encoding.UTF8.GetBytes($"{message}-{i}");

                    // 发送消息到交换机
                    channel.BasicPublish(exchangeName, key, null, body);
                    Console.WriteLine($"发布消息 {message}-{i} 到队列，Key 为 {key}。");
                }
            }
        }
    }
}
