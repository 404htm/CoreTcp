using System;
using System.Linq;
using System.Reflection;

namespace CoreTcp.Compilation
{
    internal class MethodBuilder : IMethodBuilder
    {
        public Method Build(MethodInfo method)
        {
            var @params = method.GetParameters();
            var outs = Enumerable.Range(0, @params.Length)
                .Where(i => @params[i].IsOut)
                .ToArray();

            Func<object, object[], object> invoke = method.Invoke;

            object[] Run(object impl, object[] args)
            {
                var result = invoke(impl, args);
                var output = new object[outs.Length + 1];
                output[0] = result;
                outs.Select(i => args[i]).ToArray().CopyTo(output, 1);
                return output;
            }

            return new Method(method.Name, method.ReturnType, Run);
        }

    }
}