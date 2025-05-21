using Microsoft.AspNetCore.Mvc;
using NetArchTechChallenge.Shared.Application.Services;

namespace NetArchTechChallenge.Commands.Controllers
{
    [ApiController]
    [Route("api/contacts")]
    public class ContactDeleteController : ControllerBase
    {
        private readonly ContactCommandsService _contactService;

        public ContactDeleteController(ContactCommandsService contactService)
        {
            _contactService = contactService;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _contactService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
