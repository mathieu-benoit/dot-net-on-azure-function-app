using Xunit;

namespace DotNetFunction.UnitTests2
{
    public class SampleHelloDotNetFunctionTests
    {
        [Fact]
        public void EmptyNameShouldReturnHelloWorld()
        {
            //Arrange
            var name = string.Empty;

            //Act
            var result = SampleHelloDotNetFunction.InternalSampleHelloDotNetFunction(name);

            //Assert
            Assert.Equal(result, "Hello, World!");
        }

        [Fact]
        public void NotEmptyNameShouldReturnAppropriateHello()
        {
            //Arrange
            var name = "John";

            //Act
            var result = SampleHelloDotNetFunction.InternalSampleHelloDotNetFunction(name);

            //Assert
            Assert.Equal(result, $"Hello, {name}!");
        }
    }
}
