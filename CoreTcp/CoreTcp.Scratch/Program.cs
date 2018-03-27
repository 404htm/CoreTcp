using System;
using System.Linq.Expressions;
using CoreTcp.Clients;

namespace CoreTcp.Scratch
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var server = new TcpServer<IAppService>(new AppService()))
            {
                
                server.Start(10002);
            
                var proxy = StuffFactory.Create();
                //proxy.SendString();
                var result = proxy.Add(2, 2);
                Console.WriteLine($"Return value from proxy: {result}");
                //proxy.Property = "Test";

                var client = new TcpCommClient(10002, "127.0.0.1");
                client.Send("Test raw client");
                
                
            }

            
            


            //var client = new TcpClientFactory(5556);
            //client.Send("Hello World!");
            //client.Send("Hello again.");
        }
    }

    class AppService: IAppService
    {
        public void SendString()
        {
            Console.WriteLine("String sent");
        }

        public int Add(int a, int b)
        {
            return a + b;
        }

        public string Property { get; set; }
    }

}