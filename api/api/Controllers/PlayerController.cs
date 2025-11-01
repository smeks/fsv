using System.Security.Claims;
using API.Domain.Exceptions;
using API.Domain.IManager;
using API.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerManager _playerManager;

        public PlayerController(IPlayerManager playerManager)
        {
            _playerManager = playerManager;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<PlayerDM> Get()
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;

            if (userName == null)
                return Unauthorized();

            return Ok(_playerManager.GetPlayer(userName));
        }

        // GET api/values
        [HttpPut]
        [Route("job/{jobId}")]
        public ActionResult<PlayerDM> AddJob(string jobId)
        {
            try
            {
                var userName = User.FindFirst(ClaimTypes.Name)?.Value;

                if (userName == null)
                    return Unauthorized();

                return Ok(_playerManager.AddJob(userName, jobId));
            }
            catch (DomainBadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DomainNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]
        [Route("job/{jobId}")]
        public ActionResult<PlayerDM> RemoveJob(string jobId)
        {
            try
            {
                var userName = User.FindFirst(ClaimTypes.Name)?.Value;

                if (userName == null)
                    return Unauthorized();

                return Ok(_playerManager.RemoveJob(userName, jobId));
            }
            catch (DomainBadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DomainNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut]
        [Route("rent/{aircraftId}")]
        public ActionResult<PlayerDM> Rent(string aircraftId)
        {
            try
            {
                var userName = User.FindFirst(ClaimTypes.Name)?.Value;

                if (userName == null)
                    return Unauthorized();

                return Ok(_playerManager.RentAircraft(userName, aircraftId));
            }
            catch (DomainBadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DomainNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]
        [Route("rent")]
        public ActionResult<PlayerDM> Release()
        {
            try
            {
                var userName = User.FindFirst(ClaimTypes.Name)?.Value;

                if (userName == null)
                    return Unauthorized();

                return Ok(_playerManager.ReleaseRentedAircraft(userName));
            }
            catch (DomainBadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DomainNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
