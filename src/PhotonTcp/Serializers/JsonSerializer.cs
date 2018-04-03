using System.IO;
using System.Runtime.Serialization.Json;

namespace PhotonTcp.Serializers
{
    public class JsonSerializer<T> : ISerializer<T>
    {
        private readonly DataContractJsonSerializer _serializer;
        
        public JsonSerializer()
        {
            _serializer = new DataContractJsonSerializer(typeof(T));
        }

        public T ReadFromStream(Stream stream) => (T)_serializer.ReadObject(stream);

        public void WriteToStream(T obj, Stream stream) => _serializer.WriteObject(stream, obj);

        public T FromByteArray(byte[] data) => (T)_serializer.ReadObject(new MemoryStream(data));
        
        public byte[] ToByteArray(T obj)
        {
            using (var ms = new MemoryStream())
            {
                _serializer.WriteObject(ms, obj);
                return ms.ToArray();
            }
        }
        
        

    }
}