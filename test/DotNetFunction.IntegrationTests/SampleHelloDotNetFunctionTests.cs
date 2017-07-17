using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading.Tasks;

namespace DotNetFunction.IntegrationTests
{
    [TestClass]
    public class SampleHelloDotNetFunctionTests
    {
        public TestContext TestContext { get; set; }
        private string Url = "http://localhost:7071/api/SampleHelloDotNetFunction";

        [TestInitialize()]
        public void MyTestInitialize()
        {
            if (!string.IsNullOrEmpty(TestContext.Properties["Url"] as string)) 
            {
                //Set URL from a build
                Url = TestContext.Properties["Url"].ToString();
            }
        }

        [TestMethod]
        [TestCategory("IntegrationTests")]
        public async Task EmptyNameShouldSendNotFound()
        {
            //Arrange
            var httpClient = new HttpClient();
            var urlTested = Url;

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
            var urlTested = $"{Url}/{nameTested}";

            //Act
            var response = await httpClient.GetAsync(urlTested);
            var text = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.AreEqual(text, $"\"Hello, {nameTested}!\"");
        }

    }
}
