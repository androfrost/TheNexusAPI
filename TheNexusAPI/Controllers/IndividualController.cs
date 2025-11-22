using Microsoft.AspNetCore.Mvc;
using TheNexusAPI.Data;
using TheNexusAPI.Entities;
using TheNexusAPI.Services;

namespace TheNexusAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndividualController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IndividualService _individualService;

        public IndividualController(DataContext dataContext) { 
            _dataContext = dataContext;
            _individualService = new IndividualService(_dataContext);
        }

        #region Get

        // Get all Individuals
        [HttpGet]
        public List<Individual> GetIndividuals()
        {
            return _dataContext.Individual.ToList();
        }


        // Get one specific Individual based on the primary key IndividualId
        [HttpGet("{individualId}")]
        public Individual? GetIndividualByIndividualId(int individualId)
        {
            return _individualService.GetIndividualByIndividualId(individualId);
        }

        // Get all Individuals who are within one family by the FamilyId
        [HttpGet("family/{familyId}")]
        public List<Individual> GetIndividualsByFamilyId(int familyId)
        {
            return _dataContext.Individual.Where(item => item.FamilyId == familyId).ToList();  // ?? new List<Individual>();
        }

        // Get all Individuals with a specific status
        [HttpGet("status/{statusId}")]
        public List<Individual> GetIndividualsByStatusId(int statusId)
        {
            return _individualService.GetIndividualByStatusId(statusId) ?? new List<Individual>();
        }

        #endregion

        #region Update

        // Update a specific individual with new data based on passed individual
        [HttpPut]
        public List<Individual> UpdateIndividual(Individual individual)
        {
            return _individualService.UpdateIndividual(individual);
        }

        #endregion

        #region Add

        // Add the given individual if they do not already exist
        [HttpPost]
        public Individual AddIndividual(Individual individual)
        {
            return _individualService.AddIndividual(individual);
        }
        
        #endregion
    }
}
