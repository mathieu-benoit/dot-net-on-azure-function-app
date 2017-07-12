using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetFunction.UnitTests
{
    [TestClass]
    public class SampleTests
    {
        [TestMethod]
        public void TwoPlusTwoEqualsFour()
        {
            //Arrange
            var expectedResult = 4;

            //Act
            var result = Add(2, 2);

            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void TwoPlusTwoNotEqualsFive()
        {
            //Arrange
            var unExpectedResult = 5;

            //Act
            var result = Add(2, 2);

            //Assert
            Assert.AreNotEqual(unExpectedResult, result);
        }

        int Add(int x, int y)
        {
            return x + y;
        }
    }
}
