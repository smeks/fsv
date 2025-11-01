using System.Collections.Generic;
using System.Linq;
using API.Data.Mappers;
using API.Data.Models;
using API.Domain.Exceptions;
using API.Domain.IRepository;
using API.Domain.Models;
using AutoMapper;
using MongoDB.Driver;

namespace API.Data
{
    public class JobRepository : IJobRepository
    {
        private readonly IMongoCollection<Job> _collection;
        private readonly IMapper _mapper;

        public JobRepository()
        {
            _mapper = new MapperConfiguration(cfg => { cfg.AddProfile<JobRepositoryMappingProfile>(); }).CreateMapper();


            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("economy");
            _collection = database.GetCollection<Job>("jobs");
        }

        public List<JobDM> GetJobsByICAO(string icao)
        {
            var jobs = _collection.AsQueryable().Where(x => x.FromIcao == icao && x.IsAssigned == false).ToList();
            return _mapper.Map<List<JobDM>>(jobs);
        }

        public JobDM GetJobById(string id)
        {
            Job job = _collection.AsQueryable().SingleOrDefault(x => x.Id == id);

            if(job == null)
                throw new RepositoryNotFoundException($"Job {id} not found");

            return _mapper.Map<JobDM>(job);
        }

        public JobDM UpdateJob(JobDM job)
        {
            var entity = _mapper.Map<Job>(job);

            var updatedJob = _collection.FindOneAndReplace(x => x.Id == job.Id, entity);

            if (updatedJob == null)
                throw new RepositoryNotFoundException($"Could not find and update job {job.Id}");

            return GetJobById(job.Id);
        }
    }
}
