using System.IO;

namespace CoreTcp
{
    public interface ICommClient
    {
        void Send(string methodName);
        Stream GetStream();
    }
}