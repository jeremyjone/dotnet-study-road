using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.Consumer
{
    public static class Receive
    {
        static Receive()
        {
            Connection = RabbitMQFactory.Create();
            Channel = Connection.CreateModel();
        }

        private static IConnection Connection { get; set; }
        private static IModel Channel { get; set; }

        public static void Close()
        {
            Channel.Dispose();
            Connection.Close();
        }


        public static void Basic()
        {
            // 声明队列
            Channel.QueueDeclare("queue", false, false, false, null);

            #region 创建基于事件的消费者

            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"收到消息：{message}");
            };

            Channel.BasicConsume("queue", true, consumer);

            #endregion



            #region 基于时间轮询的消费者

            //while (true)
            //{
            //    // 从管道中获取数据
            //    var response = Channel.BasicGet("queue", true);
            //    if (response != null)
            //    {
            //        var message = Encoding.UTF8.GetString(response.Body.ToArray());
            //        Console.WriteLine($"收到消息：{message}");
            //    }
                
            //    // 每隔1秒轮询一次
            //    Thread.Sleep(1000);
            //}

            #endregion
        }

        public static void Worker()
        {
            const string queueName = "worker_queue";
            Channel.QueueDeclare(queueName, false, false, false, null);

            // 创建基于事件的消费者
            var consumer = new EventingBasicConsumer(Channel);

            // 设置 prefetchCount
            Channel.BasicQos(0, 1, false);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"收到消息：{message}");
            };

            Channel.BasicConsume(queueName, true, consumer);
        }

        public static void Fanout()
        {
            const string exchangeName = "fanout_exchange";
            //const string queueName1 = "fanout_queue1";
            const string queueName2 = "fanout_queue2";
            //const string queueName3 = "fanout_queue3";

            // 声明交换机
            Channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout);

            // 声明3个队列
            //Channel.QueueDeclare(queueName1, false, false, false, null);
            Channel.QueueDeclare(queueName2, false, false, false, null);
            //Channel.QueueDeclare(queueName3, false, false, false, null);

            // 将队列绑定到交换机
            //Channel.QueueBind(queueName1, exchangeName, "");
            Channel.QueueBind(queueName2, exchangeName, "");
            //Channel.QueueBind(queueName3, exchangeName, "");

            // 创建基于事件的消费者
            var consumer = new EventingBasicConsumer(Channel);

            // 设置 prefetchCount
            Channel.BasicQos(0, 1, false);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"收到消息：{message}");
            };

            // 消费第二个队列中的消息
            Channel.BasicConsume(queueName2, true, consumer);
        }

        public static void Direct()
        {
            const string exchangeName = "direct_exchange";
            const string queueName1 = "direct_queue1";

            // 声明交换机
            Channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);

            // 声明3个队列
            Channel.QueueDeclare(queueName1, false, false, false, null);

            // 将队列绑定到交换机
            Channel.QueueBind(queueName1, exchangeName, "c");

            // 创建基于事件的消费者
            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"收到消息：{message}，key：{ea.RoutingKey}");
            };

            // 消费第一个队列中的消息
            Channel.BasicConsume(queueName1, true, consumer);
        }

        public static void Topic()
        {
            const string exchangeName = "topic_exchange";
            const string queueName1 = "topic_queue1";

            // 声明交换机
            Channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);

            // 声明3个队列
            //Channel.QueueDeclare(queueName1, false, false, false, null);

            // 将队列绑定到交换机
            //Channel.QueueBind(queueName1, exchangeName, "data.delete");

            // 创建基于事件的消费者
            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"收到消息：{message}，key：{ea.RoutingKey}");
            };

            // 消费第一个队列中的消息
            Channel.BasicConsume(queueName1, true, consumer);
        }
    }
}
