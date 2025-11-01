using System.Collections.Generic;
using API.Domain.IManager;
using API.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AircraftController : ControllerBase
    {
        private readonly IAircraftManager _aircraftManager;
        private readonly IMapper _mapper;

        public AircraftController(IAircraftManager aircraftManager)
        {
            _aircraftManager = aircraftManager;
        }

        // GET api/values
        [HttpGet]
        [Route("{icao}")]
        public ActionResult<IEnumerable<AircraftDM>> Get(string icao)
        {
            return Ok(_aircraftManager.GetAircraftByICAO(icao));
        }
    }
}