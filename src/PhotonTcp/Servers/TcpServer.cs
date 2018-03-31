using System.Net;
using System.Net.Sockets;

namespace PhotonTcp.Servers
{
    public class TcpServer : Server
    {
        public TcpServer(IPAddress ip, int port)
        {
            
        }

        private async void Listen()
        {
            var listener = new TcpListener(ip, port);
            while (true)
            {
                var socket = await listener.AcceptSocketAsync();
                EnqueueRequest(socket);
            }
        }
        
        public void Dispose()
        {
            Console.WriteLine("Server - Shutting Down.");
            _Listener?.Stop();
        }
}