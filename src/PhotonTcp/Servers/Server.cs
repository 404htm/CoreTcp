using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using CoreTcp.Utility;

namespace PhotonTcp.Servers
{    
    public abstract class Server : IDisposable
    {
        protected bool _Running;
        protected readonly List<Service> _Services = new List<Service>();
        private readonly Encoder _encoder = new Encoder();
        
        public Server Register<T>(T implementation)
        {
            var svc = new Service<T>(implementation);
            _Services.Add(svc);
            return this;
        }
        

        public void Start()
        {
            if (_Running) throw new InvalidOperationException("Server is already running");
            _Running = true;

            ThreadPool.QueueUserWorkItem((a) => Listen());
        }
        
        public void Stop()
        {
            _Running = false;
        }


        protected void DelegateToService(Socket socket)
        {
            byte[] bytes = _encoder.Read(socket);
            var items = _serializer.Read(bytes);
            var svcId = (int)items[0];
            var svc = GetImplementation(svcId);
            var result = svc.Call(items.Skip(1).ToArray());
            
        }

        protected void EncodeAndReply(Socket socket)
        {
        }

        protected void EnqueueRequest(Socket socket)
        {
            ThreadPool.QueueUserWorkItem(a => DelegateToService(socket));
        }
        
        protected Service GetImplementation(int id) => _Services.ElementAt(id);

        protected abstract void Listen();

        protected abstract void Reply(object[] result);

        public abstract void Dispose();
    }
}