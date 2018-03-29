using System.IO;
using System.Net.Sockets;
using System.Text;

namespace CoreTcp.Clients
{
    public class TcpCommClient : ICommClient
    {
        private readonly int _port;
        private readonly string _hostname;
        
        public TcpCommClient(int port, string hostname)
        {
            _port = port;
            _hostname = hostname;
        }

        public void Send(string message)
        {
            
            var client = new TcpClient(_hostname, _port);
            var str = client.GetStream();

            var enc = new UTF8Encoding();
            var bytes = enc.GetBytes(message);

            str.Write(bytes, 0, bytes.Length);

            str.Flush();
            client.Close();
            
        }
        
        public Stream GetStream()
        {  
            var client = new TcpClient(_hostname, _port);
            return client.GetStream();
        }
    }


}