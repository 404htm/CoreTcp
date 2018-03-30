using System.Reflection;

namespace PhotonTcp.Compilation
{
    internal interface IMethodBuilder
    {
        Method Build(MethodInfo method);
    }
}