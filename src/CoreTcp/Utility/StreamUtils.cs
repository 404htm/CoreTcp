using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Json;

namespace CoreTcp.Utility
{
    public static class StreamUtils
    {
        public static void WriteToStream(Type type, object obj, Stream stream)
        {
            
            byte[] data;
            var ser = new DataContractJsonSerializer(type);
            using (var ms = new MemoryStream())
            {
                ser.WriteObject(ms, obj);
                data = ms.ToArray();
            }
            
            byte[] intBytes = BitConverter.GetBytes(data.Length);
            //if (BitConverter.IsLittleEndian) Array.Reverse(intBytes);
            
            stream.Write(intBytes,0,4);
            stream.Write(data, 0, data.Length);
        }

        public static T ReadFromSocket<T>(Socket socket)
        {
            byte[] sizeData = new byte[4];
            socket.Receive(sizeData);
            //if (BitConverter.IsLittleEndian) Array.Reverse(sizeData);
            var size = BitConverter.ToInt32(sizeData, 0);
            Console.WriteLine($"Size read from socket: {size}");
            
            byte[] buffer = new byte[size];
            socket.Receive(buffer);
 
            Console.WriteLine($"Data read from socket");
            var ser = new DataContractJsonSerializer(typeof(T));
            ser.ReadObject(new MemoryStream(buffer));
            return (T)ser.ReadObject(new MemoryStream(buffer));
            
        }
        
        public static T ReadFromStream<T>(Stream stream)
        {
            byte[] sizeData = new byte[4];
            stream.Read(sizeData, 0, 4);
            //if (BitConverter.IsLittleEndian) Array.Reverse(sizeData);
            var size = BitConverter.ToInt32(sizeData, 0);
            Console.WriteLine($"Size read from stream: {size}");
            
            byte[] buffer = new byte[size];
            stream.Read(buffer, 0, size);
            Console.WriteLine($"Data read from stream");
            
            var ser = new DataContractJsonSerializer(typeof(T));
            ser.ReadObject(new MemoryStream(buffer));
            return (T)ser.ReadObject(new MemoryStream(buffer));
            
        }
        
        public static void WriteToSocket(Type type, object obj, Socket socket)
        {
            
            byte[] data;
            var ser = new DataContractJsonSerializer(type);
            using (var ms = new MemoryStream())
            {
                ser.WriteObject(ms, obj);
                data = ms.ToArray();
            }
            
            byte[] intBytes = BitConverter.GetBytes(data.Length);
            //if (BitConverter.IsLittleEndian) Array.Reverse(intBytes);

            socket.Send(intBytes);
            socket.Send(data);
        }
    }
}