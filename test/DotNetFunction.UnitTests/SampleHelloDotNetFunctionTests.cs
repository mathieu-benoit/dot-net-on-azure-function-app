using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetFunction.UnitTests
{
    [TestClass]
    public class SampleHelloDotNetFunctionTests
    {
        [TestMethod]
        [TestCategory("UnitTests")]
        public void EmptyNameShouldReturnHelloWorld()
        {
            //Arrange
            var name = string.Empty;

            //Act
            var result = SampleHelloDotNetFunction.InternalSampleHelloDotNetFunction(name);

            //Assert
            Assert.AreEqual(result, "Hello, World!");
        }

        [TestMethod]
        [TestCategory("UnitTests")]
        public void NotEmptyNameShouldReturnAppropriateHello()
        {
            //Arrange
            var name = "John";

            //Act
            var result = SampleHelloDotNetFunction.InternalSampleHelloDotNetFunction(name);

            //Assert
            Assert.AreEqual(result, $"Hello, {name}!");
        }
    }
}
