using System;
using System.Data;
using System.IO;
using System.Linq.Expressions;
using System.Net.Cache;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Castle.DynamicProxy;
using CoreTcp.Clients;
using CoreTcp.Utility;

namespace CoreTcp
{
    //TODO: Make a real factory
    public static class StuffFactory
    {
        public static IAppService Create()
        {
            ProxyGenerator generator = new ProxyGenerator();
            ICommClient client = new TcpCommClient(10002, "127.0.0.1");
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
            var str = _client.GetStream();
            var req = new object[2 + invocation.Arguments.Length];
            req[1] = invocation.Method.Name;
            Array.Copy(invocation.Arguments, 0, req, 2, invocation.Arguments.Length);
            StreamUtils.WriteToStream(typeof(object[]), req, str);
            

            if ( invocation.Method.ReturnType != typeof(void))
            {
                //var response = 
                
                var serReturn = new DataContractJsonSerializer(typeof(object[]));
                var result = (object[])serReturn.ReadObject(str);
                invocation.ReturnValue = result[0]; //@return.Value;
            }
            //invocation.Proceed();
            
        }


    }

    public interface IAppService
    {
        void SendString();
        int Add(int a, int b);
        string Property { get; set; }
    }
    
}
