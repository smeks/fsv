using System.Collections.Generic;
using API.Domain.IManager;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobManager _jobManager;
        private readonly IMapper _mapper;

        public JobController(IJobManager jobManager)
        {
            _jobManager = jobManager;
        }

        // GET api/values
        [HttpGet]
        [Route("{icao}")]
        public ActionResult<IEnumerable<string>> Get(string icao)
        {
            return Ok(_jobManager.GetJobsByIcao(icao));
        }
    }
}