using System;
using System.IO;
using System.Linq.Expressions;
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

            //invocation.Method.Name
            Console.WriteLine($"InterceptLog: Method: {invocation.Method.Name}");
            var request = new Request
            {
                Method = invocation.Method.Name,
                Params = invocation.Arguments
            };

            var str = _client.GetStream();

            StreamUtils.WriteToStream(typeof(Request), request, str);
            Console.WriteLine("Request writtten to stream");
            //var buffer = new byte[500];
           // str.Read(buffer, 0, 500);
           // var enc = new UTF8Encoding();
            //Console.WriteLine(enc.GetString(buffer));
            
            //_client.Send(invocation.Method.Name);

            if ( invocation.Method.ReturnType != typeof(void))
            {
                var @return = StreamUtils.ReadFromStream<Return>(str);
                
                //var serReturn = new DataContractJsonSerializer(typeof(Return));
                //var @return = (Return)serReturn.ReadObject(str);
                invocation.ReturnValue = @return.Value;
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
