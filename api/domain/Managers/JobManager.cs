using System.Collections.Generic;
using API.Domain.IManager;
using API.Domain.IRepository;
using API.Domain.Models;

namespace API.Domain.Managers
{
    public class JobManager : IJobManager
    {
        private readonly IJobRepository _jobRepository;

        public JobManager(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public List<JobDM> GetJobsByIcao(string icao)
        {
            return _jobRepository.GetJobsByICAO(icao);
        }
    }
}
