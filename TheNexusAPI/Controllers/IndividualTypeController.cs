using Microsoft.AspNetCore.Mvc;
using TheNexusAPI.Data;
using TheNexusAPI.Entities;
using TheNexusAPI.Services;

namespace TheNexusAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndividualTypeController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IndividualTypeService _individualTypeService;

        public IndividualTypeController(DataContext dataContext)
        {
            _dataContext = dataContext;
            _individualTypeService = new IndividualTypeService(_dataContext);

        }

        #region Get
        // Get all Individual Types
        [HttpGet]
        public List<IndividualType> GetIndividualTypes()
        {
            return _dataContext.IndividualType.ToList();
        }

        // Get all active Individual Types
        [HttpGet("active")]
        public List<IndividualType> GetActiveIndividualTypes(bool active)
        {
            return _dataContext.IndividualType.Where(it => it.Active == active).ToList();
        }

        [HttpGet("individualTypeId")]
        public IndividualType GetIndividualTypeByIndividualTypeId(int individualTypeId)
        {
            return _individualTypeService.GetIndividualTypeByIndividualTypeId(individualTypeId);
        }

        #endregion

        #region Update

        //Update a specific individual type with new data based on passed individual type
        [HttpPut]
        public List<IndividualType> UpdateIndividualType(IndividualType individualType)
        {
            return _individualTypeService.UpdateIndividualType(individualType);
        }

        #endregion

        #region Add

        //Add the given individual type if they do not already exist
        [HttpPost]
        public IndividualType AddIndividualType(IndividualType individualType)
        {
            return _individualTypeService.AddIndividualType(individualType);
        }

        #endregion
    }
}
