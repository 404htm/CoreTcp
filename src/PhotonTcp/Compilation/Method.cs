using System;
using System.Linq;
using System.Reflection;

namespace PhotonTcp.Compilation
{
    using Call = Func<object, object[], object[]>;
    
    internal class Method
    {

        public Method(string name, Type returnType, Call call)
        {
            Name = name;
            Return = returnType;
            HasReturnValue = Return != typeof(void);
            
            _func = call;
        }

        private readonly Call _func;
        
        public string Name { get; }
        public Type Return { get; }
        public bool HasReturnValue { get; }

        public object[] Run(object impl, object[] args) => _func(impl, args);


        
    }
}