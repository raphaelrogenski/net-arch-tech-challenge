using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using NetArchTechChallenge.Shared.Application.Services;
using System.Web;

namespace NetArchTechChallenge.Query.Functions
{
    public class ContactEmailAlreadyExistsFunction
    {
        private readonly ContactQueryService service;

        public ContactEmailAlreadyExistsFunction(ContactQueryService service)
        {
            this.service = service;
        }

        [Function("ContactEmailAlreadyExists")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Contacts/EmailAlreadyExists")] HttpRequest req)
        {
            var id = GetId(req.QueryString);
            var email = GetEmail(req.QueryString);

            var list = service.ContactEmailAlreadyExists(email, id);

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

        private string GetEmail(QueryString queryString)
        {
            string email = null;

            if (queryString != null && queryString.Value != null)
            {
                var query = HttpUtility.ParseQueryString(queryString.Value);
                email = query["email"];
            }

            return email;
        }
    }
}
