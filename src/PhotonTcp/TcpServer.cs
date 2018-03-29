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

                var message = StreamUtils.ReadFromSocket<object[]>(socket);
                
                var method = typeof(T).GetMethod((string)message[1]);

                if (method.ReturnType != typeof(void))
                {
                    var args = new object[message.Length - 2];
                    Array.Copy(message,2,args,0, args.Length);
                    var result = method.Invoke(Service, args);
                    var resultArr = new object[1] {result};

                    var ser = new DataContractJsonSerializer(typeof(object[]));
                    byte[] data;
                    using (var ms = new MemoryStream())
                    {
                        ser.WriteObject(ms, resultArr);
                        data = ms.ToArray();
                    }

                    socket.Send(data);
                }

                
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