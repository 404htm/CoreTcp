using System.Reflection;

namespace CoreTcp.Compilation
{
    internal interface IMethodBuilder
    {
        Method Build(MethodInfo method);
    }
}