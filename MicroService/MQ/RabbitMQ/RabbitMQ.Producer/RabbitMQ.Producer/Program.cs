using System;

namespace RabbitMQ.Producer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            // 基础一对一发送
            //Send.Basic();

            // 工作队列
            //Send.Worker();

            // 扇形交换机
            //Send.Fanout();

            // 精准发送交换机
            //Send.Direct();

            // 匹配模式
            Send.Topic();

            Console.ReadKey();
        }
    }
}
