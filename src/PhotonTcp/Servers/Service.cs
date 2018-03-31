using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using PhotonTcp.Compilation;

namespace PhotonTcp
{
    public abstract class Service
    {
        protected List<Method> _Methods;
        
        protected abstract object GetImpl();
        public object[] Call(int method, object[] args) => _Methods[method].Run(GetImpl(), args);
        
    }

    public class Service<T> : Service
    {
        private readonly IMethodBuilder _methodBuilder;
        
        private readonly Func<T> _impl;
        protected override object GetImpl() => _impl();

        internal Service(T implementation, IMethodBuilder methodBuilder) : this(implementation)
        {
            _methodBuilder = methodBuilder;
        }
        
        public Service(T implementation)
        {
            if (!typeof(T).IsInterface) throw new InvalidOperationException("<T> must be an interface");
            _impl = () => implementation;
        }


        public void Compile()
        {
            var type = typeof(T);
            _Methods = type.GetMethods()
                .Select(m => _methodBuilder.Build(m))
                .ToList();
        }



    }
}