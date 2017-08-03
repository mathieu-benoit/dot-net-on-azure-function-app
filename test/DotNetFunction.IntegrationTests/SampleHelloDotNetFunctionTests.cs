using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DotNetFunction.IntegrationTests
{
    public class SampleHelloDotNetFunctionTests
    {
        private string BaseUrl = "https://localhost:7071";

        public SampleHelloDotNetFunctionTests()
        {
            var appSettings = ConfigurationManager.AppSettings;
            var baseUrlParameter = appSettings["BaseUrl"];
            BaseUrl = string.IsNullOrEmpty(baseUrlParameter) || baseUrlParameter == "#{BaseUrl}#" ? BaseUrl : baseUrlParameter;
            BaseUrl = BaseUrl + "/api/SampleHelloDotNetFunction";
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
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("\"Please pass a name on the query string or in the request body.\"", text);
        }

        [Fact]
        public async Task Post_ShouldSendNotFound()
        {
            //Arrange
            var httpClient = new HttpClient();
            var urlTested = BaseUrl;
            var name = "test";
            var httpContent = new StringContent($"name={name}");

            //Act
            var response = await httpClient.PostAsync(urlTested, httpContent);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
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
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ShouldSendNotFound()
        {
            //Arrange
            var httpClient = new HttpClient();
            var urlTested = BaseUrl;

            //Act
            var response = await httpClient.DeleteAsync(urlTested);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task NotEmptyNameShouldSendOK()
        {
            //Arrange
            var httpClient = new HttpClient();
            var name = "john";
            var urlTested = $"{BaseUrl}?name={name}";

            //Act
            var response = await httpClient.GetAsync(urlTested);
            var text = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal($"\"Hello, {name}!\"", text);
        }

    }
}
