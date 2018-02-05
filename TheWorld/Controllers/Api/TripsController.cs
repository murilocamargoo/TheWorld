using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheWorld.Models;
using TheWorld.ViewModels;
using Microsoft.Extensions.Logging;
using TheWorld.Services;

namespace TheWorld.Controllers.Api
{
    [Route("api/trips")]
    [Authorize]
    public class TripsController : Controller
    {
        private IWorldRepository _worldRepository;
        private ILogger<TripsController> _logger;
        private GeoCoordsService _coordsService;

        public TripsController(IWorldRepository worldRepository,
                               ILogger<TripsController> logger,
                               GeoCoordsService coordsService)
        {
            _worldRepository = worldRepository;
            _logger = logger;
            _coordsService = coordsService;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            try
            {
                var results = _worldRepository.GetTripsByUserName(this.User.Identity.Name);

                return Ok(Mapper.Map<IEnumerable<TripViewModel>>(results));
            }
            catch (Exception exception)
            {
                _logger.LogError($"Failed to get All Trips: {exception}");

                return BadRequest("Error ocurred");
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]TripViewModel theTrip)
        {
            if (ModelState.IsValid)
            {
                var newTrip = Mapper.Map<Trip>(theTrip);

                newTrip.UserName = User.Identity.Name;

                _worldRepository.AddTrip(newTrip);

                if (await _worldRepository.SaveChangesAsync())
                {
                    return Created($"api/trips/{theTrip.Name}", Mapper.Map<TripViewModel>(newTrip));
                }              
            }

            return BadRequest("Failed to save the trip");
        }
    }
}
