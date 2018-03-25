using System;

namespace CoreTcp.Scratch
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new TcpServer<ITest>(new Test());
            server.Start(10001);
            
            var proxy = StuffFactory.Create();
            proxy.SendString();
            proxy.Property = "Test";
            

            
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