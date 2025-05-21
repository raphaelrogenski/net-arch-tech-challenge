using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using NetArchTechChallenge.Shared.Application.Services;
using NetArchTechChallenge.Shared.Domain.Entities;
using System.Web;

namespace NetArchTechChallenge.Query.Functions
{
    public class ContactNameAlreadyExistsFunction
    {
        private readonly ContactQueryService service;

        public ContactNameAlreadyExistsFunction(ContactQueryService service)
        {
            this.service = service;
        }

        [Function("ContactNameAlreadyExists")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Contacts/NameAlreadyExists")] HttpRequest req)
        {
            var id = GetId(req.QueryString);
            var name = GetName(req.QueryString);

            var list = service.ContactNameAlreadyExists(name, id);

            return new OkObjectResult(list);
        }

        private Guid GetId(QueryString queryString)
        {
            try
            {
                string id = null;

                if (queryString != null && queryString.Value != null)
                {
                    var query = HttpUtility.ParseQueryString(queryString.Value);
                    id = query["id"];
                }

                return Guid.Parse(id);
            }
            catch (Exception)
            {
                return Guid.Empty;
            }
        }

        private string GetName(QueryString queryString)
        {
            string name = null;

            if (queryString != null && queryString.Value != null)
            {
                var query = HttpUtility.ParseQueryString(queryString.Value);
                name = query["name"];
            }

            return name;
        }
    }
}
