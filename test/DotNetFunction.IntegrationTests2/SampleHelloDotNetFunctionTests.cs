using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DotNetFunction.IntegrationTests
{
    public class Class1
    {
        private string BaseUrl = "https://mabenoittest.azurewebsites.net/api/SampleHelloDotNetFunction";

        public Class1()
        {
            var appSettings = ConfigurationManager.AppSettings;
            var baseUrlParameter = appSettings["BaseUrl"];
            BaseUrl = string.IsNullOrEmpty(baseUrlParameter) || baseUrlParameter == "#{BaseUrl}#" ? BaseUrl : baseUrlParameter;
        }

        [Fact]
        public async Task EmptyNameShouldSendNotFound()
        {
            //Arrange
            var httpClient = new HttpClient();
            var urlTested = BaseUrl;

            //Act
            var response = await httpClient.GetAsync(urlTested);

            //Assert
            Assert.Equal(response.StatusCode, System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task NotEmptyNameShouldSendOK()
        {
            //Arrange
            var httpClient = new HttpClient();
            var nameTested = "john";
            var urlTested = $"{BaseUrl}/{nameTested}";

            //Act
            var response = await httpClient.GetAsync(urlTested);
            var text = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(response.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.Equal(text, $"\"Hello, {nameTested}!\"");
        }

    }
}
