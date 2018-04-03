using System;
using System.Net.Sockets;

namespace CoreTcp.Utility
{
    internal class Encoder
    {
        private const int SIZE_HEADER_LEN = 4;
            
        public byte[] ReadData(Socket socket)
        {
            byte[] sizeData = new byte[SIZE_HEADER_LEN];
            socket.Receive(sizeData);
            //if (BitConverter.IsLittleEndian) Array.Reverse(sizeData);
            var size = BitConverter.ToInt32(sizeData, 0);
            Console.WriteLine($"Size read from socket: {size}");
            
            byte[] buffer = new byte[size];
            socket.Receive(buffer);
            return buffer;
        }
    }
}