using System.Security.Claims;
using API.Models;
using API.Domain.IManager;
using API.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PlayerFlightController : ControllerBase
    {
        private readonly IPlayerFlightManager _playerFlightManager;

        public PlayerFlightController(IPlayerFlightManager playerFlightManager)
        {
            _playerFlightManager = playerFlightManager;
        }

        /// <summary>
        /// Gets a player flight aggregate
        /// </summary>
        /// <param name="playerId">the player id</param>
        /// <param name="lat">current player latitude</param>
        /// <param name="lon">current player longitude</param>
        /// <param name="model">current player aircraft model</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<PlayerFlightDM> Get([FromQuery]double lat, [FromQuery]double lon, [FromQuery]string model)
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;

            if (userName == null)
                return Unauthorized();

            return Ok(_playerFlightManager.GetPlayerFlight(userName, lat,lon,model));
        }

        [HttpPut]
        [Route("start")]
        public ActionResult<PlayerDM> StartFlight(StartFlightDC startFlight)
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;

            if (userName == null)
                return Unauthorized();

            return Ok(_playerFlightManager.StartFlight(userName, startFlight.Latitude, startFlight.Longitude, startFlight.Model));
        }

        [HttpPut]
        [Route("end")]
        public ActionResult<PlayerDM> EndFlight(EndFlightDC endFlight)
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;

            if (userName == null)
                return Unauthorized();

            return Ok(_playerFlightManager.EndFlight(userName, endFlight.Latitude, endFlight.Longitude, endFlight.Model));
        }

    }
}
