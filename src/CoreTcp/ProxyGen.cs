using System;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Castle.DynamicProxy;
using CoreTcp.Clients;

namespace CoreTcp
{
    //TODO: Make a real factory
    public static class StuffFactory
    {
        public static IAppService Create()
        {
            ProxyGenerator generator = new ProxyGenerator();
            ICommClient client = new TcpCommClient(10001, "127.0.0.1");
            return generator.CreateInterfaceProxyWithoutTarget<IAppService>(new TcpInterceptor(client));
        }
    }
    
    public class TcpInterceptor : IInterceptor
    {
        private readonly ICommClient _client;
        
        public TcpInterceptor(ICommClient client)
        {
            _client = client;
        }
        
        public void Intercept(IInvocation invocation)
        {
            //invocation.Method.Name
            Console.Write($"Log: Method Called: {invocation.Method.Name}");
            _client.Send(invocation.Method.Name);
            //invocation.Proceed();

        }

    }

    
    public interface IAppService
    {
        void SendString();
        string Property { get; set; }
    }
    
}
