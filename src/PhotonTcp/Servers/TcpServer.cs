using System.Net;
using System.Net.Sockets;

namespace PhotonTcp.Servers
{
    public class TcpServer : Server
    {
        private readonly IPAddress _ip;
        private readonly int _port;
        private TcpListener _listener;

        public TcpServer(IPAddress ip, int port)
        {
            _ip = ip;
            _port = port;
        }

        protected override async void Listen()
        {
            _listener = new TcpListener(_ip, _port);
            while (_running)
            {
                var socket = await _listener.AcceptSocketAsync();
                EnqueueRequest(socket);
            }
        }

        public override void Dispose()
        {
            //Console.WriteLine("Server - Shutting Down.");
            _listener?.Stop();
        }
    }
}