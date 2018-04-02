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

        public T Read(Stream stream) => (T)_serializer.ReadObject(stream);

        public void Write(T obj, Stream stream) => _serializer.WriteObject(stream, obj);

        
        public byte[] GetBytes(T data)
        {
            using (var ms = new MemoryStream())
            {
                _serializer.WriteObject(ms, data);
                return ms.ToArray();
            }
        }
    }
}