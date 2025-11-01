using System.Collections.Generic;
using API.Domain.Models;

namespace API.Domain.IManager
{
    public interface IJobManager
    {
        List<JobDM> GetJobsByIcao(string icao);
    }
}