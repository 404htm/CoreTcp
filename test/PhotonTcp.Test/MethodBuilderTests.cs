using PhotonTcp.Compilation;
using Xunit;

namespace PhotonTcp.Test
{
    public class MethodBuilderTests
    {
        [Fact]
        public void Test1()
        {
            var method = typeof(ITestInterface).GetMethod("BasicMethod");
            var underTest = new MethodBuilder();
            
            //Assert.Equal("BasicMethod", underTest.Name);
        }

        internal interface ITestInterface
        {
            void BasicMethod();
        }
    }
}