using Xunit;

namespace DotNetFunction.UnitTests
{
    public class SampleHelloDotNetFunctionTests
    {
        [Theory]
        [InlineData("", "")]
        [InlineData(" ", "")]
        [InlineData("", " ")]
        [InlineData(" ", " ")]
        [InlineData(null, null)]
        public void EmptyNameInQueryString_And_EmptyNameInRequestBody_Should_ReturnHelpMessage(string nameInQueryString, string nameInRequestBody)
        {
            //Arrange
            var responseMessage = string.Empty;

            //Act
            var result = SampleHelloDotNetFunction.GetResponseMessage(nameInQueryString, nameInRequestBody, out responseMessage);

            //Assert
            Assert.False(result);
            Assert.Equal("Please pass a name on the query string or in the request body.", responseMessage);
        }

        [Theory]
        [InlineData("", "John")]
        [InlineData(" ", "Test")]
        [InlineData(null, "Peter")]
        public void EmptyNameInQueryString_And_NotEmptyNameInRequestBody_Should_ReturnMessageWithNameInRequestBody(string nameInQueryString, string nameInRequestBody)
        {
            //Arrange
            var responseMessage = string.Empty;

            //Act
            var result = SampleHelloDotNetFunction.GetResponseMessage(nameInQueryString, nameInRequestBody, out responseMessage);

            //Assert
            Assert.True(result);
            Assert.Equal($"Hello, {nameInRequestBody}!", responseMessage);
        }

        [Theory]
        [InlineData("John", "")]
        [InlineData("Test", " ")]
        [InlineData("Peter", null)]
        public void NotEmptyNameInQueryString_And_EmptyNameInRequestBody_Should_ReturnMessageWithNameInQueryString(string nameInQueryString, string nameInRequestBody)
        {
            //Arrange
            var responseMessage = string.Empty;

            //Act
            var result = SampleHelloDotNetFunction.GetResponseMessage(nameInQueryString, nameInRequestBody, out responseMessage);

            //Assert
            Assert.True(result);
            Assert.Equal($"Hello, {nameInQueryString}!", responseMessage);
        }

        [Theory]
        [InlineData("John", "Body")]
        [InlineData("Test", "Request")]
        public void NotEmptyNameInQueryString_And_NotEmptyNameInRequestBody_Should_ReturnMessageWithNameInQueryString(string nameInQueryString, string nameInRequestBody)
        {
            //Arrange
            var responseMessage = string.Empty;

            //Act
            var result = SampleHelloDotNetFunction.GetResponseMessage(nameInQueryString, nameInRequestBody, out responseMessage);

            //Assert
            Assert.True(result);
            Assert.Equal($"Hello, {nameInQueryString}!", responseMessage);
        }
    }
}
