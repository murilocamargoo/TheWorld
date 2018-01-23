using Microsoft.AspNetCore.Mvc;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Route("api/trips")]
    public class TripsController : Controller
    {
        private IWorldRepository _worldRepository;

        public TripsController(IWorldRepository worldRepository)
        {
            _worldRepository = worldRepository;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            return Ok(_worldRepository.GetAllTrips());
        }

        [HttpPost("")]
        public IActionResult Post([FromBody]TripViewModel theTrip)
        {
            if (ModelState.IsValid)
            {
                return Created($"api/trips/{theTrip.Name}", theTrip);
            }

            return BadRequest(ModelState);
        }
    }
}
