using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PhotonTcp.Compilation;

namespace PhotonTcp
{
    public abstract class Service
    {
        
    }

    public abstract class Service<T> : Service
    {
        private readonly IMethodBuilder _methodBuilder;
        private readonly Func<T> _impl;
        private List<Method> _methods;

        internal Service(T implementation, IMethodBuilder methodBuilder) : this(implementation)
        {
            _methodBuilder = methodBuilder;
        }
        
        public Service(T implementation)
        {
            if (!typeof(T).IsInterface) throw new InvalidOperationException("<T> must be an interface");
            _impl = () => implementation;
        }

        private T GetImpl() => _impl();
        
        public object[] Call(int method, object[] args) => _methods[method].Run(GetImpl(), args);

        public void Compile()
        {
            var type = typeof(T);
            _methods = type.GetMethods()
                .Select(m => _methodBuilder.Build(m))
                .ToList();
        }
        
        public void Start()
        {
            //if (Running) throw new InvalidOperationException("Server is already running");
           // Running = true;

            ThreadPool.QueueUserWorkItem((a) => Listen());
        }

        protected abstract void Listen();

    }
}