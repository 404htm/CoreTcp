using System;

namespace CoreTcp.Scratch
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new TcpServer<ITest>(new Test());
            server.Start(5555);
            
            var client = new TcpClientFactory(5555);
            client.Send("Hello World!");
            client.Send("Hello again.");
        }
    }

    class Test : ITest
    {
        
    }

    interface ITest
    {
        
    }
}