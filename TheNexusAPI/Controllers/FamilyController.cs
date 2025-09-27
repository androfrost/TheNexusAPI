using Microsoft.AspNetCore.Mvc;
using TheNexusAPI.Data;
using TheNexusAPI.Entities;
using TheNexusAPI.Services;

namespace TheNexusAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly FamilyService _familyService;

        private List<Family> families = new List<Family>() { };

        public FamilyController(DataContext dataContext)
        {
            _dataContext = dataContext;
            _familyService = new FamilyService(_dataContext);

            ////////////////////////
            // Default data to be removed once databases are implemented
            families.Add(new Family { FamilyId = 1, FamilyName = "Wade", FamilyDescription = "Wade"});
            families.Add(new Family { FamilyId = 2, FamilyName = "Disney", FamilyDescription = "Disney"});
            ////////////////////////
        }

        #region Get

        // Get all Families
        [HttpGet]
        public List<Family> GetFamilies()
        {
            return families;
        }

        // Get one specific Family based on the primary key FamilyId
        [HttpGet("{familyId}")]
        public Family? GetFamilyByFamilyId(int familyId)
        {
            return _familyService.GetFamilyByFamilyId(familyId, families);
        }

        // Get all Families who are within one family by the FamilyId
        [HttpGet("family/{familyId}")]
        public List<Family> GetFamiliesByFamilyId(int familyId)
        {
            return families.FindAll(item => item.FamilyId == familyId);  // ?? new List<Family>();
        }

        #endregion

        #region Update

        // Update a specific family with new data based on passed family
        [HttpPut]
        public List<Family> UpdateFamily(Family family)
        {
            return _familyService.UpdateFamily(family, families);
        }

        #endregion

        #region Add

        // Add the given family if they do not already exist
        [HttpPost]
        public Family AddFamily(Family family)
        {
            return _familyService.AddFamily(family);
        }

        #endregion
    }
}
