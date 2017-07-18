using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading.Tasks;

namespace DotNetFunction.IntegrationTests
{
    [TestClass]
    public class SampleHelloDotNetFunctionTests
    {
        public TestContext TestContext { get; set; }
        private string BaseUrl = "http://localhost:7071/api/SampleHelloDotNetFunction";

        [TestInitialize()]
        public void TestInitialize()
        {
            if (TestContext.Properties["BaseUrl"] != null)
            {
                //Set the BaseURL from a build
                BaseUrl = TestContext.Properties["BaseUrl"].ToString();
            }
        }

        [TestMethod]
        [TestCategory("IntegrationTests")]
        public async Task EmptyNameShouldSendNotFound()
        {
            //Arrange
            var httpClient = new HttpClient();
            var urlTested = BaseUrl;

            //Act
            var response = await httpClient.GetAsync(urlTested);

            //Assert
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.NotFound);
        }

        [TestMethod]
        [TestCategory("IntegrationTests")]
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
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.AreEqual(text, $"\"Hello, {nameTested}!\"");
        }

    }
}
