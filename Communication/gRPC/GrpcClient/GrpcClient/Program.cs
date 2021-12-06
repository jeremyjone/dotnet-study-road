using System;
using System.Threading.Tasks;
using Grpc.Net.Client;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Greeter.GreeterClient(channel);
            var reply = await client.SayHelloAsync(new HelloRequest {Name = "DotNetConsoleClient"});

            Console.WriteLine("Greeting:" + reply.Message);
        }
    }
}
