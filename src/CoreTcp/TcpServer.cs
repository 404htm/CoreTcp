using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace CoreTcp
{
    public class TcpServer<T>
    {
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
            ThreadPool.QueueUserWorkItem((a) => Listen(port, ip));
        }
        
        private void Listen(int port, IPAddress ip = null)
        {
            var listener = new TcpListener(ip, port);
            listener.Start();
            
            while (Running)
            {
                var socket = listener.AcceptSocket();
                Console.WriteLine("Conected");


                    byte[] data = new byte[500];
                    int size = socket.Receive(data);
                    Console.WriteLine("Data received");
                    for (var i = 0; i < size; i++) Console.Write(Convert.ToChar(data[i]));


                    socket.Close();
               
            }
        }


    }
}