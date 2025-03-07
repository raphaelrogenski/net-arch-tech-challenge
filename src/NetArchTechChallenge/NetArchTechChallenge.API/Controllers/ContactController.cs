using Microsoft.AspNetCore.Mvc;
using NetArchTechChallenge.Shared.Application.DTOs;
using NetArchTechChallenge.Shared.Application.Services;

namespace NetArchTechChallenge.API.Controllers
{
    [ApiController]
    [Route("api/contacts")]
    public class ContactController : ControllerBase
    {
        private readonly ContactAPIService _contactService;

        public ContactController(ContactAPIService contactService)
        {
            this._contactService = contactService;
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
