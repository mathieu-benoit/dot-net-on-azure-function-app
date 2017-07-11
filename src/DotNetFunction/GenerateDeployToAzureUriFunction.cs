using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Net;
using System.Net.Http;

namespace DotNetFunction
{
    public static class GenerateDeployToAzureUriFunction
    {
        [FunctionName("GenerateDeployToAzureUri")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "GenerateDeployToAzureUri/{sourceUri}")]HttpRequestMessage request, string sourceUri, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            var generatedUri = "Hello " + sourceUri;

            log.Info(string.Format("Request: {0} with Response: {1}.", sourceUri, generatedUri));
            
            return request.CreateResponse(HttpStatusCode.OK, generatedUri);
        }
    }
}