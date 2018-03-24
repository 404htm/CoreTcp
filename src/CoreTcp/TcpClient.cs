using System.Net.Sockets;
using System.Text;

namespace CoreTcp
{
    public class TcpClientFactory
    {
        private int _port;

        public TcpClientFactory(int port)
        {
            _port = port;
        }

        public void Send(string message)
        {
            var client = new TcpClient("127.0.0.1", _port);
            var str = client.GetStream();

            var enc = new UTF8Encoding();
            var bytes = enc.GetBytes(message);

            str.Write(bytes, 0, bytes.Length);
            str.Flush();
            client.Close();
        }
    }
}