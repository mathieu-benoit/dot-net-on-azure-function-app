using Xunit;

namespace DotNetFunction.UnitTests
{
    public class SampleHelloDotNetFunctionTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void EmptyNameInQueryString_Should_ReturnHelpMessage(string nameInQueryString)
        {
            //Arrange
            var responseMessage = string.Empty;

            //Act
            var result = SampleHelloDotNetFunction.GetResponseMessage(nameInQueryString, out responseMessage);

            //Assert
            Assert.False(result);
            Assert.Equal("Please pass a 'name' on the query string.", responseMessage);
        }

        [Theory]
        [InlineData("John")]
        [InlineData("Test")]
        public void NotEmptyNameInQueryString_Should_ReturnMessageWithNameInQueryString(string nameInQueryString)
        {
            //Arrange
            var responseMessage = string.Empty;

            //Act
            var result = SampleHelloDotNetFunction.GetResponseMessage(nameInQueryString, out responseMessage);

            //Assert
            Assert.True(result);
            Assert.Equal($"Hello, {nameInQueryString}!", responseMessage);
        }
    }
}
