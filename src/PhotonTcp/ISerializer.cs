using System.IO;

namespace PhotonTcp
{
    public interface ISerializer<T>
    {
        T Read(Stream stream);
        void Write(T obj, Stream stream);
        byte[] GetBytes(T data);
    }
}