using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DotNetFunction
{
    public static class SampleHelloDotNetFunction
    {
        [FunctionName("SampleHelloDotNetFunction2")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")]HttpRequestMessage request)
        {
            // parse query parameter
            string name = request.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
                .Value;

            // Get request body
            dynamic data = await request.Content.ReadAsAsync<object>();

            // Set name to query string or body data
            name = name ?? data?.name;

            return name == null
                ? request.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
                : request.CreateResponse(HttpStatusCode.OK, $"Hello, {name}!");
        }
    }
}