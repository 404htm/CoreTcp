using PhotonTcp.Compilation;
using Xunit;

namespace PhotonTcp.Test
{
    public class MethodBuilderTests
    {
        [Fact]
        public void RunWorksOnGetter()
        {
            var instance = new TestClass();
            var method = GetMethod("get_Value");
            var underTest = new MethodBuilder();

            instance.Value = 3;
            var result = underTest.Build(method);
            var output = result.Run(instance, new object[0]);

            Assert.Equal(3, output[0]);
        }
        
        [Fact]
        public void RunReturnsSingleOutParamOnVoidMethod()
        {
            var instance = new TestClass();
            var method = GetMethod(nameof(ITestInterface.AddOut));
            var underTest = new MethodBuilder();

            var result = underTest.Build(method);
            var output = result.Run(instance, new object[]{2,3,0});

            Assert.Equal(2, output.Length);
            Assert.Equal(null, output[0]);
            Assert.Equal(5, output[1]);
        }
        
        [Fact]
        public void RunWorksOnVoidMethodNoParams()
        {
            var instance = new TestClass();
            var method = GetMethod(nameof(ITestInterface.Increment));
            var underTest = new MethodBuilder();

            var result = underTest.Build(method);
            result.Run(instance, new object[0]);

            Assert.Equal(1, instance.Value);
        }

        [Fact]
        public void NameIsExtractedFromMethod()
        {
            var method = GetMethod(nameof(ITestInterface.Increment));
            var underTest = new MethodBuilder();

            var result = underTest.Build(method);
            
            Assert.Equal("Increment", result.Name);
        }
        
        [Fact]
        public void ReturnTypeIsExtractedFromMethod()
        {
            var method = GetMethod(nameof(ITestInterface.Increment));
            var underTest = new MethodBuilder();

            var result = underTest.Build(method);
            
            Assert.Equal(typeof(void), result.Return);
        }

        private System.Reflection.MethodInfo GetMethod(string name) => typeof(ITestInterface).GetMethod(name);
        
        internal interface ITestInterface
        {
            int Value { get; set; }
            void Increment();
            void AddOut(int a, int b, out int c);
        }
        
        internal class TestClass : ITestInterface
        {
            private int _counter = 0;

            public int Value
            {
                get => _counter;
                set => _counter = value;
            }

            public void Increment() => _counter++;
            public void AddOut(int a, int b, out int c) => c = a + b;
        }
    }
}