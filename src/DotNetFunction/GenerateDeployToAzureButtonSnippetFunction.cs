using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Net;
using System.Net.Http;

namespace DotNetFunction
{
    public static class GenerateDeployToAzureButtonSnippetFunction
    {
        [FunctionName("GenerateDeployToAzureButtonSnippet")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "GenerateDeployToAzureButtonSnippet/{sourceUri}")]HttpRequestMessage request, string sourceUri, TraceWriter log)
        {
            var encodedSourceUri = System.Web.HttpUtility.UrlEncode(sourceUri);
            var generatedSnippet = $"<a href=\"https://portal.azure.com/#create/Microsoft.Template/uri/{encodedSourceUri}\" target=\"_blank\"><img src=\"http://azuredeploy.net/deploybutton.png\"></a>";

            log.Info($"Source URI: {sourceUri} - Encoded URI: {encodedSourceUri} - Generated Snippet: {generatedSnippet}.");
            
            return request.CreateResponse(HttpStatusCode.OK, generatedSnippet);
        }
    }
}