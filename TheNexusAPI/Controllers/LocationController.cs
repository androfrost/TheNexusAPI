using Microsoft.AspNetCore.Mvc;
using TheNexusAPI.Data;
using TheNexusAPI.Entities;
using TheNexusAPI.Entities.Dto;
using TheNexusAPI.Services;

namespace TheNexusAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {

        private readonly DataContext _dataContext;
        private readonly LocationService _locationService;

        public LocationController(DataContext dataContext)
        {
            _dataContext = dataContext;
            _locationService = new LocationService(_dataContext);

        }

        #region Get
        // Get all Locations
        [HttpGet]
        public List<Location> GetLocations()
        {
            return _locationService.GetLocations();
        }

        [HttpGet("locationId")]
        public Location GetLocationByLocationId(int locationId)
        {
            return _locationService.GetLocationByLocationId(locationId);
        }

        [HttpGet("individualsLocations/{individualId}")]
        public List<Location> GetLocationsByIndividualId(int individualId)
        {
            return _locationService.GetLocationsByIndividualId(individualId);
        }
            
        #endregion

        #region Update

        //Update a specific location with new data based on passed location
        [HttpPut]
        public List<Location> UpdateLocation(Location location)
        {
            return _locationService.UpdateLocation(location);
        }

        #endregion

        #region Add

        //Add the given location if they do not already exist
        [HttpPost]
        public Location AddLocation(Location location)
        {
            return _locationService.AddLocation(location);
        }

        [HttpPost("IndividualLocation")]
        public IndividualLocationDto AddIndividualToALocation(IndividualLocationDto individualLocationDto)
        {
            return _locationService.AddIndividualToALocation(individualLocationDto);
        }

        #endregion
    }
}
