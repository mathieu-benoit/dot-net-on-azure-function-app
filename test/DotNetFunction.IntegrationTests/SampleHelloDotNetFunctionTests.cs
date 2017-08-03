using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DotNetFunction.IntegrationTests
{
    public class SampleHelloDotNetFunctionTests
    {
        private string BaseUrl = "https://localhost:7071/api/SampleHelloDotNetFunction";

        public SampleHelloDotNetFunctionTests()
        {
            var appSettings = ConfigurationManager.AppSettings;
            var baseUrlParameter = appSettings["BaseUrl"];
            BaseUrl = string.IsNullOrEmpty(baseUrlParameter) || baseUrlParameter == "#{BaseUrl}#" ? BaseUrl : baseUrlParameter;
        }

        [Fact]
        public async Task Get_EmptyName_ShouldSendBadRequestAndHelpMessage()
        {
            //Arrange
            var httpClient = new HttpClient();
            var urlTested = BaseUrl;

            //Act
            var response = await httpClient.GetAsync(urlTested);
            var text = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(response.StatusCode, System.Net.HttpStatusCode.BadRequest);
            Assert.Equal(text, "Please pass a name on the query string or in the request body.");
        }

        [Fact]
        public async Task Post_ShouldSendNotAllowed()
        {
            //Arrange
            var httpClient = new HttpClient();
            var urlTested = BaseUrl;
            var name = "test";
            var httpContent = new StringContent($"name={name}");

            //Act
            var response = await httpClient.PostAsync(urlTested, httpContent);

            //Assert
            Assert.Equal(response.StatusCode, System.Net.HttpStatusCode.MethodNotAllowed);
        }

        [Fact]
        public async Task Put_ShouldSendNotAllowed()
        {
            //Arrange
            var httpClient = new HttpClient();
            var urlTested = BaseUrl;
            var name = "test";
            var httpContent = new StringContent($"name={name}");

            //Act
            var response = await httpClient.PutAsync(urlTested, httpContent);

            //Assert
            Assert.Equal(response.StatusCode, System.Net.HttpStatusCode.MethodNotAllowed);
        }

        [Fact]
        public async Task NotEmptyNameShouldSendOK()
        {
            //Arrange
            var httpClient = new HttpClient();
            var name = "john";
            var urlTested = $"{BaseUrl}/{name}";

            //Act
            var response = await httpClient.GetAsync(urlTested);
            var text = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(response.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.Equal(text, $"\"Hello, {name}!\"");
        }

    }
}
