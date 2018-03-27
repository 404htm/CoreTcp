using System.Collections;
using System.Collections.Generic;

namespace CoreTcp.Servers
{
    public class Server
    {
        Dictionary<int, Service> _services = new Dictionary<int, Service>();
        
        public Server Register<T>(T service)
        {
            
            return this;
        }
    }
}