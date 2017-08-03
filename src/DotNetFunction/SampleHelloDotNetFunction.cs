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
        [FunctionName("SampleHelloDotNetFunction")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")]HttpRequestMessage request)
        {
            var name = request.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
                .Value;

            dynamic data = await request.Content.ReadAsAsync<object>();

            var responseMessage = string.Empty;
            HttpStatusCode httpStatusCode = HttpStatusCode.OK;
            if(!GetResponseMessage(name, data?.name, out responseMessage))
            {
                httpStatusCode = HttpStatusCode.BadRequest;
            }

            return request.CreateResponse(httpStatusCode, responseMessage);
        }

        /// <summary>
        /// Get the response message to display according the name value passed either in the query string or in the request body. 
        /// Note: The query string has priority over the request body.
        /// </summary>
        /// <param name="nameInQueryString">The name value in the query string.</param>
        /// <param name="nameInRequestBody">The name value in the request body.</param>
        /// <param name="responseMessage">The out parameter containing the response message.</param>
        /// <returns>Return true if an appropriate name was provided and set a response message accordingly. Otherwise return false and set an help message.</returns>
        public static bool GetResponseMessage(string nameInQueryString, string nameInRequestBody, out string responseMessage)
        {
            var name = !string.IsNullOrWhiteSpace(nameInQueryString) ? nameInQueryString : nameInRequestBody;

            responseMessage = string.IsNullOrWhiteSpace(name)
                ? "Please pass a name on the query string or in the request body."
                : $"Hello, {name}!";

            return !string.IsNullOrWhiteSpace(name);
        }
    }
}