using Microsoft.AspNetCore.Mvc;
using TheNexusAPI.Data;
using TheNexusAPI.Entities;
using TheNexusAPI.Services;

namespace TheNexusAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndividualLocationController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IndividualLocationService _individualLocationService;

        public IndividualLocationController(DataContext dataContext)
        {
            _dataContext = dataContext;
            _individualLocationService = new IndividualLocationService(_dataContext);

        }

        #region Get
        // Get all Individual Types
        [HttpGet]
        public List<IndividualLocation> GetIndividualLocations()
        {
            return _dataContext.IndividualLocation.ToList();
        }

        // Get all records with given IndividualId and LocationId
        [HttpGet("{ individualId }/{ locationId }")]
        public List<IndividualLocation> GetIndividualLocationByIndividualIdAndLocationId(int individualId, int locationId)
        {
            return _dataContext.IndividualLocation.Where(il => il.IndividualId == individualId && il.LocationId == locationId).ToList();
        }

        // Get all IndividualLocation records with given IndividualId
        [HttpGet("individual")]
        public List<IndividualLocation> GetActiveIndividualLocationsByIndividualId(int individualId)
        {
            return _dataContext.IndividualLocation.Where(il => il.IndividualId == individualId).ToList();
        }

        // Get all IndividualLocation records with given LocationId
        [HttpGet("location")]
        public List<IndividualLocation> GetActiveIndividualLocationByLocationId(int locationId)
        {
            return _dataContext.IndividualLocation.Where(il => il.LocationId == locationId).ToList();
        }

        // Get the record with the IndividualLocationId
        [HttpGet("individualLocationId")]
        public IndividualLocation GetIndividualLocationByIndividualLocationId(int individualLocationId)
        {
            return _individualLocationService.GetIndividualLocationByIndividualLocationId(individualLocationId);
        }

        // Get the record with the IndividualId and LocationId
        [HttpGet("individualAndLocation")]
        public IndividualLocation GetIndividualLocationByIndividualAndLocationId(int individualId, int locationId)
        {
            return _dataContext.IndividualLocation.Where(il => il.IndividualId == individualId && il.LocationId == locationId).FirstOrDefault() ?? new IndividualLocation();
        }

        #endregion

        #region Update

        //Update a specific individual type with new data based on passed individual type
        [HttpPut]
        public List<IndividualLocation> UpdateIndividualLocation(IndividualLocation individualLocation)
        {
            return _individualLocationService.UpdateIndividualLocation(individualLocation);
        }

        #endregion

        #region Add

        //Add the given individualLocation if they do not already exist
        [HttpPost]
        public IndividualLocation AddIndividualLocation(IndividualLocation individualLocation)
        {
            return _individualLocationService.AddIndividualLocation(individualLocation);
        }

        #endregion

        #region Delete

        //Delete the given individualLocation if found
        [HttpDelete]
        public void DeleteIndividualLocation(int individualLocationId)
        {
            _dataContext.IndividualLocation.Remove(GetIndividualLocationByIndividualLocationId(individualLocationId));
            _dataContext.SaveChanges();
        }

        [HttpDelete("Delete")]
        public void DeleteIndividualLocationByIndividualAndLocationId(int individualId, int locationId)
        {
            _dataContext.IndividualLocation.Remove(GetIndividualLocationByIndividualAndLocationId(individualId, locationId));
            _dataContext.SaveChanges();
        }

        #endregion
    }
}
