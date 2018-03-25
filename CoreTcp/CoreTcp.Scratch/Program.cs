using System;
using CoreTcp.Clients;

namespace CoreTcp.Scratch
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var server = new TcpServer<ITest>(new Test()))
            {
                
                server.Start(10001);
            
                var proxy = StuffFactory.Create();
                proxy.SendString();
                proxy.Property = "Test";

                var client = new TcpCommClient(10001, "127.0.0.1");
                client.Send("Test raw client");
            }

            

            
            //var client = new TcpClientFactory(5556);
            //client.Send("Hello World!");
            //client.Send("Hello again.");
        }
    }

    class Test : ITest
    {
        
    }

    interface ITest
    {
        
    }
}