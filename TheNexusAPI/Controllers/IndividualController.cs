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

        private List<Individual> individuals = new List<Individual>() { };

        public IndividualController(DataContext dataContext) { 
            _dataContext = dataContext;
            _individualService = new IndividualService(dataContext);

            ////////////////////////
            // Default data to be removed once databases are implemented
            individuals.Add(new Individual { IndividualId = 1, FirstName = "Larry", LastName = "Wade", FamilyId = 1 });
            individuals.Add(new Individual { IndividualId = 2, FirstName = "Walt", LastName = "Disney", FamilyId = 2 });
            individuals.Add(new Individual { IndividualId = 3, FirstName = "Jonetta", LastName = "Wade", FamilyId = 1 });
            individuals.Add(new Individual { IndividualId = 4, FirstName = "Mickey", LastName = "Mouse", FamilyId = 2 });
            ////////////////////////
        }

        #region Get

        // Get all Individuals
        [HttpGet]
        public List<Individual> GetIndividuals()
        {
            return individuals;
        }

        // Get one specific Individual based on the primary key IndividualId
        [HttpGet("{individualId}")]
        public Individual? GetIndividualByIndividualId(int individualId)
        {
            return _individualService.GetIndividualByIndividualId(individualId, individuals);
        }

        // Get all Individuals who are within one family by the FamilyId
        [HttpGet("family/{familyId}")]
        public List<Individual> GetIndividualsByFamilyId(int familyId)
        {
            return individuals.FindAll(item => item.FamilyId == familyId);  // ?? new List<Individual>();
        }
        
        #endregion

        #region Update

        // Update a specific individual with new data based on passed individual
        [HttpPut]
        public List<Individual> UpdateIndividual(Individual individual)
        {
            return _individualService.UpdateIndividual(individual, individuals);
        }

        #endregion

        #region Add

        // Add the given individual if they do not already exist
        [HttpPost]
        public List<Individual> AddIndividual(Individual individual)
        {
            return _individualService.AddIndividual(individual, individuals);
        }
        
        #endregion
    }
}
