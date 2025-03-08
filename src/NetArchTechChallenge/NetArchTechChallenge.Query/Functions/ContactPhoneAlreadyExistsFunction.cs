using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using NetArchTechChallenge.Shared.Application.Services;
using NetArchTechChallenge.Shared.Domain.Entities;
using System.Web;

namespace NetArchTechChallenge.Query.Functions
{
    public class ContactPhoneAlreadyExistsFunction
    {
        private readonly ContactQueryService service;

        public ContactPhoneAlreadyExistsFunction(ContactQueryService service)
        {
            this.service = service;
        }

        [Function("ContactPhoneAlreadyExists")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Contacts/PhoneAlreadyExists")] HttpRequest req)
        {
            var id = GetId(req.QueryString);
            var ddd = GetDDD(req.QueryString);
            var phone = GetPhone(req.QueryString);

            var list = service.ContactPhoneAlreadyExists(ddd, phone, id);

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

        private string GetPhone(QueryString queryString)
        {
            string phone = null;

            if (queryString != null && queryString.Value != null)
            {
                var query = HttpUtility.ParseQueryString(queryString.Value);
                phone = query["phone"];
            }

            return phone;
        }
    }
}
