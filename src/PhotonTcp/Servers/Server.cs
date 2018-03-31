using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PhotonTcp.Servers
{    
    public class Server
    {
        protected readonly List<Service> _services = new List<Service>();
        
        public Server Register<T>(T implementation)
        {
            var svc = new Service<T>(implementation);
            
            
            _services.Add(svc);
            return this;
        }

        protected Service GetImplementation(int id) => _services.ElementAt(id);

    }
}