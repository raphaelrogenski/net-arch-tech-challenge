using Microsoft.AspNetCore.Mvc;
using NetArchTechChallenge.Shared.Application.DTOs;
using NetArchTechChallenge.Shared.Application.Services;

namespace NetArchTechChallenge.Commands.Controllers
{
    [ApiController]
    [Route("api/contacts")]
    public class ContactUpdateController : ControllerBase
    {
        private readonly ContactCommandsService _contactService;

        public ContactUpdateController(ContactCommandsService contactService)
        {
            _contactService = contactService;
        }

        [HttpPut()]
        public IActionResult Update(ContactDto contact)
        {
            try
            {
                _contactService.Update(contact);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
