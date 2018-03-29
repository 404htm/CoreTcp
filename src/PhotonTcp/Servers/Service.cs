using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CoreTcp.Compilation;

namespace Photon
{
    public abstract class Service
    {
        
    }

    public class Service<T> : Service
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

        
        
    }
}