using System.Net;
using System.Net.Sockets;

namespace PhotonTcp.Servers
{
    public class TcpServer : Server
    {
        private IPAddress _ip;
        private int _port;
        
        public TcpServer(IPAddress ip, int port)
        {
            _ip = ip;
            _port = port;
        }

        private override async void Listen()
        {
            var listener = new TcpListener(_ip, _port);
            while (_Running)
            {
                var socket = await listener.AcceptSocketAsync();
                EnqueueRequest(socket);
            }
        }

        protected override void Reply(object[] result)
        {
            throw new System.NotImplementedException();
        }

        public override void Dispose()
        {
            //Console.WriteLine("Server - Shutting Down.");
            //_Listener?.Stop();
        }
}