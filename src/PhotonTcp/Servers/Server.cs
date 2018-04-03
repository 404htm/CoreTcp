using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using CoreTcp.Utility;
using PhotonTcp.Serializers;

namespace PhotonTcp.Servers
{    
    public abstract class Server : IDisposable
    {
        protected bool _running;
        protected readonly List<Service> _services = new List<Service>();
        private readonly Encoder _encoder = new Encoder();
        private readonly ISerializer<object[]> _serializer = new JsonSerializer<object[]>();
        
        public Server Register<T>(T implementation)
        {
            var svc = new Service<T>(implementation);
            _services.Add(svc);
            return this;
        }
        

        public void Start()
        {
            if (_running) throw new InvalidOperationException("Server is already running");
            _running = true;

            ThreadPool.QueueUserWorkItem((a) => Listen());
        }
        
        public void Stop()
        {
            //TODO: Stuff
            _running = false;
        }


        protected void InvokeService(Socket socket)
        {
            byte[] bytes = _encoder.ReadData(socket);
            var items = _serializer.FromByteArray(bytes);
            var svcId = (int)items[0];
            var svc = GetImplementation(svcId);
            var result = svc.Call((int)items[1], items.Skip(2).ToArray());
            Reply(socket, result);
        }

        protected void Reply(Socket socket, object[] response)
        {
            var bytes = _serializer.ToByteArray(response);
            socket.Send(bytes);
            socket.Close();
        }

        protected void EnqueueRequest(Socket socket)
        {
            ThreadPool.QueueUserWorkItem(a => InvokeService(socket));
        }
        
        protected Service GetImplementation(int id) => _services.ElementAt(id);

        
        protected abstract void Listen();
        public abstract void Dispose();
    }
}