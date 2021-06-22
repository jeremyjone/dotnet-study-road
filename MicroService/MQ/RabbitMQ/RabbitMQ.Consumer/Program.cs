using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.Consumer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("消费者启动");

            // 基础一对一
            //Receive.Basic();

            // 工作队列
            //Receive.Worker();

            // 扇形交换机
            //Receive.Fanout();

            // 精准交换机
            //Receive.Direct();

            // 匹配模式
            Receive.Topic();

            Console.ReadKey();
            Receive.Close();
        }
    }
}
