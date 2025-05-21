using Microsoft.AspNetCore.Mvc;
using NetArchTechChallenge.Shared.Application.DTOs;
using NetArchTechChallenge.Shared.Application.Services;

namespace NetArchTechChallenge.Commands.Controllers
{
    [ApiController]
    [Route("api/contacts")]
    public class ContactCreateController : ControllerBase
    {
        private readonly ContactCommandsService _contactService;

        public ContactCreateController(ContactCommandsService contactService)
        {
            _contactService = contactService;
        }

        [HttpPost()]
        public IActionResult Create(ContactDto contact)
        {
            try
            {
                _contactService.Create(contact);
                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
