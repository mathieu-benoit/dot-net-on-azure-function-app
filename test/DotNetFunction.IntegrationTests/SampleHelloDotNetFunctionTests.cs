using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DotNetFunction.IntegrationTests
{
    public class SampleHelloDotNetFunctionTests
    {
        private string FunctionUrl = "http://localhost:7071/api/SampleHelloDotNetFunction?code=123";

        public SampleHelloDotNetFunctionTests()
        {
            var appSettings = ConfigurationManager.AppSettings;
            var baseUrlParameter = appSettings["functionUrl"];
            FunctionUrl = string.IsNullOrEmpty(baseUrlParameter) || baseUrlParameter == "#{functionUrl}#" ? FunctionUrl : baseUrlParameter;
        }

        [Fact]
        public async Task Get_WrongFunctionKey_ShouldSendUnauthorized()
        {
            //Arrange
            var httpClient = new HttpClient();
            var uri = new Uri(FunctionUrl);
            var urlTested = uri.AbsoluteUri.Replace(uri.Query, "?code=123&name=test");

            //Act
            var response = await httpClient.GetAsync(urlTested);

            //Assert
            //Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);//TMP: should be replaced by the line above when this issue will be fixed: https://github.com/Azure/azure-webjobs-sdk-script/issues/1752
        }

        [Fact]
        public async Task Get_EmptyName_ShouldSendBadRequestAndHelpMessage()
        {
            //Arrange
            var httpClient = new HttpClient();
            var urlTested = FunctionUrl;

            //Act
            var response = await httpClient.GetAsync(urlTested);
            var text = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("\"Please pass a 'name' on the query string.\"", text);
        }

        [Fact]
        public async Task Put_ShouldSendNotFound()
        {
            //Arrange
            var httpClient = new HttpClient();
            var urlTested = FunctionUrl;
            var httpContent = new StringContent(string.Empty);

            //Act
            var response = await httpClient.PutAsync(urlTested, httpContent);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Post_ShouldSendNotFound()
        {
            //Arrange
            var httpClient = new HttpClient();
            var urlTested = FunctionUrl;
            var httpContent = new StringContent(string.Empty);

            //Act
            var response = await httpClient.PostAsync(urlTested, httpContent);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ShouldSendNotFound()
        {
            //Arrange
            var httpClient = new HttpClient();
            var urlTested = FunctionUrl;

            //Act
            var response = await httpClient.DeleteAsync(urlTested);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Get_WithNameInQueryString_ShouldSendOK()
        {
            //Arrange
            var httpClient = new HttpClient();
            var name = "john";
            var urlTested = $"{FunctionUrl}&name={name}";

            //Act
            var response = await httpClient.GetAsync(urlTested);
            var text = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal($"\"Hello, {name}!\"", text);
        }

    }
}
