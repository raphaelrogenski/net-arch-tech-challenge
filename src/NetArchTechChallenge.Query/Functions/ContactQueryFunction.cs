using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using NetArchTechChallenge.Shared.Application.Services;
using NetArchTechChallenge.Shared.Domain.Entities;
using System.Web;

namespace NetArchTechChallenge.Query.Functions
{
    public class ContactQueryFunction
    {
        private readonly ContactQueryService service;

        public ContactQueryFunction(ContactQueryService service)
        {
            this.service = service;
        }

        [Function("ContactQuery")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Contacts")] HttpRequest req)
        {
            var ddd = GetDDD(req.QueryString);

            var list = service.List(ddd);

            return new OkObjectResult(list);
        }

        private string GetDDD(QueryString queryString)
        {
            string ddd = null;

            if (queryString != null && queryString.Value != null)
            {
                var query = HttpUtility.ParseQueryString(queryString.Value);
                ddd = query["ddd"];
            }

            return ddd;
        }
    }
}
