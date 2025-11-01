using System.Collections.Generic;
using API.Domain.Models;

namespace API.Domain.IRepository
{
    public interface IJobRepository
    {
        List<JobDM> GetJobsByICAO(string icao);
        JobDM GetJobById(string id);
        JobDM UpdateJob(JobDM job);
    }
}