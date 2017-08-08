using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace DotNetFunction
{
    public static class SampleHelloDotNetFunction
    {
        [FunctionName("SampleHelloDotNetFunction")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Function, "get")]HttpRequestMessage request)
        {
            var name = request.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
                .Value;

            var responseMessage = string.Empty;
            var httpStatusCode = HttpStatusCode.OK;
            if(!GetResponseMessage(name, out responseMessage))
            {
                httpStatusCode = HttpStatusCode.BadRequest;
            }

            return request.CreateResponse(httpStatusCode, responseMessage);
        }

        /// <summary>
        /// Get the response message to display according the name value passed either in the query string.
        /// </summary>
        /// <param name="nameInQueryString">The name value in the query string.</param>=
        /// <param name="responseMessage">The out parameter containing the response message.</param>
        /// <returns>Return true if an appropriate name was provided and set a response message accordingly. Otherwise return false and set an help message.</returns>
        public static bool GetResponseMessage(string nameInQueryString, out string responseMessage)
        {
            responseMessage = string.IsNullOrWhiteSpace(nameInQueryString)
                ? "Please pass a 'name' on the query string."
                : $"Hello, {nameInQueryString}!";

            return !string.IsNullOrWhiteSpace(nameInQueryString);
        }
    }
}