using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Prometheus;
using System.Net;
using System.Text;

namespace NetArchTechChallenge.Query.Functions
{
    public class MetricsFunction
    {
        [Function("MetricsFunction")]
        public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "metrics")] HttpRequestData req,
        FunctionContext executionContext)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; version=0.0.4");

            await using var memoryStream = new MemoryStream();
            await Metrics.DefaultRegistry.CollectAndExportAsTextAsync(memoryStream);
            memoryStream.Position = 0;

            using var reader = new StreamReader(memoryStream, Encoding.UTF8);
            var content = await reader.ReadToEndAsync();

            await response.WriteStringAsync(content);

            return response;
        }
    }
}
