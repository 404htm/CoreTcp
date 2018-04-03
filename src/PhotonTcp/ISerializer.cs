using System.IO;

namespace PhotonTcp
{
    public interface ISerializer<T>
    {
        T ReadFromStream(Stream stream);
        void WriteToStream(T obj, Stream stream);
        T FromByteArray(byte[] data);
        byte[] ToByteArray(T obj);
    }
}