using System;
using System.Collections.Generic;

namespace Photon
{
    public abstract class Service
    {
        
    }

    public class Service<T> : Service
    {
        private Func<T> _impl;
        private List<object> _methods;
        
        public Service(T implementation)
        {
            _impl = () => implementation;
        }

        public void Compile()
        {
            var type = typeof(T);
            type.GetMethods();
        }

        public object[] Call(int method, object[] args)
        {
            return null;
        }
        
    }
}