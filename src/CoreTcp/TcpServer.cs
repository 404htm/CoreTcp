using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoreTcp.Clients;
using CoreTcp.Utility;

namespace CoreTcp
{
    public class TcpServer<T> : IDisposable
    {
        private TcpListener _listener;
        public bool Running { get; private set; }
        public T Service { get; }

        /// <param name="service">Object to handle incoming requests.</param>
        public TcpServer(T service)
        {
            if (!typeof(T).IsInterface) throw new InvalidOperationException("<T> must be an interface");

            Service = service;
        }


        public void Start(int port, IPAddress ip = null)
        {
            if (Running) throw new InvalidOperationException("Server is already running");
            Running = true;

            ip = ip ?? IPAddress.Any;
            _listener = new TcpListener(ip, port);
            ThreadPool.QueueUserWorkItem((a) => Start());
        }
        
        private void Start()
        {
            _listener.Start();
            
            while (Running)
            {
                var socket = _listener.AcceptSocket();
                Console.WriteLine("Conected");

               
                //var enc = new UTF8Encoding();
                //Console.WriteLine("Data:");
                //Console.WriteLine(enc.GetString(data));

                var message = StreamUtils.ReadFromSocket<Request>(socket);
                Console.WriteLine("Read object.");

                var method = typeof(T).GetMethod(message.Method);

                if (method.ReturnType != typeof(void))
                {
                    var @return = new Return();
                    var respSer = new DataContractSerializer(typeof(Return));
                    @return.Value = method.Invoke(Service, message.Params);
                    var str = new MemoryStream();

                    respSer.WriteObject(str, @return);
                    socket.Send(str.ToArray());
                }

                
                
                Console.WriteLine(message.Method);
                Console.WriteLine();

                socket.Close();
               
            }
        }
        
        


        public void Dispose()
        {
            Console.WriteLine("Server - Shutting Down.");
            _listener?.Stop();
        }
    }
}