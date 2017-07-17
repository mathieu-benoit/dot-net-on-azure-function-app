using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Net;
using System.Net.Http;

namespace DotNetFunction
{
    public static class SampleHelloDotNetFunction
    {
        [FunctionName("SampleHelloDotNetFunction")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SampleHelloDotNetFunction/{name}")]HttpRequestMessage request, string name, TraceWriter log)
        {
            var responseText = InternalSampleHelloDotNetFunction(name);

            log?.Info($"Response text: {responseText}");

            return request.CreateResponse(HttpStatusCode.OK, responseText);
        }

        public static string InternalSampleHelloDotNetFunction(string name)
        {
            var responseText = "Hello, World!";

            if (!string.IsNullOrEmpty(name))
                responseText = $"Hello, {name}!";

            return responseText;
        }
    }
}