using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TheNexusAPI.Data;
using TheNexusAPI.Entities;
using TheNexusAPI.Entities.Dto;

namespace TheNexusAPI.Services
{
    public class LocationService
    {
        private readonly DataContext _dataContext;
        private readonly ChangeLogService _changeLog;
        private readonly IndividualService _individualService;
        private readonly IndividualLocationService _individualLocationService;

        public LocationService(DataContext dataContext)
        {
            _dataContext = dataContext;
            _changeLog = new ChangeLogService(_dataContext);
            _individualLocationService = new IndividualLocationService(_dataContext);
            _individualService = new IndividualService(_dataContext);
        }

        public List<Location> UpdateLocation(Location updatedLocation)
        {
            Location? foundLocation = GetLocationByLocationId(updatedLocation.LocationId);
            // For holding foundLocation initial value and during changelog check making sure what changed
            Location compareFoundLocation = foundLocation != null ? new Location
            {
                LocationId = foundLocation.LocationId,
                LocationName = foundLocation.LocationName,
                Address = foundLocation.Address,
                City = foundLocation.City,
                State = foundLocation.State,
                Zip = foundLocation.Zip
            } : new Location();
            if (foundLocation != null)
            {
                foundLocation.LocationId = updatedLocation.LocationId;
                foundLocation.LocationName = updatedLocation.LocationName;
                foundLocation.Address = updatedLocation.Address;
                foundLocation.City = updatedLocation.City;
                foundLocation.State = updatedLocation.State;
                foundLocation.Zip = updatedLocation.Zip;
            }

            try
            {
                _dataContext.Location.Update(foundLocation ?? new Location());
                _dataContext.SaveChanges();
                // If updates succeed, log changes
                _changeLog.ConvertChangesForLogging(compareFoundLocation, updatedLocation);
            }
            catch (DbUpdateException ex)
            {
                // Handle exceptions related to database updates
                Console.WriteLine($"An error occurred while updating the location: {ex.Message}");
            }
            return _dataContext.Location.ToList();
        }

        public List<Location> GetLocations()
        {
            return _dataContext.Location.ToList();
        }

        public Location GetLocationByLocationId(int locationId)
        {
            return _dataContext.Location.FirstOrDefault(item => item.LocationId == locationId) ?? new Location();
        }

        public List<Location> GetLocationsByIndividualId(int individualId)
        {
            List<IndividualLocation> individualLocation = _individualLocationService.GetIndividualLocationsByIndividualId(individualId);
            List<Location> allLocations = GetLocations();
            List<Location> locations = allLocations.Join(
                individualLocation,                 // inner sequence
                location => location.LocationId,    // outer key selector
                individualLocation => individualLocation.LocationId,  // inner key selector
            (location, individualLocation) => new Location    // result selector
            {
                LocationId = location.LocationId,
                LocationName = location.LocationName,
                Address = location.Address,
                City = location.City,
                State = location.State,
                Zip = location.Zip
            }).ToList();

            return locations;
        }

        public List<LocationsWithAssignedIndividualDto> GetIndividualsLocationWithAssignedIndividual(int individualId)
        {
            List<Location> allLocations = GetLocations();
            List<LocationsWithAssignedIndividualDto> locationsWithAssignedIndividualDtos = allLocations.Select(location => new LocationsWithAssignedIndividualDto
            {
                LocationId = location.LocationId,
                LocationName = location.LocationName,
                Address = location.Address,
                City = location.City,
                State = location.State,
                Zip = location.Zip,
                IsAssigned = _individualLocationService.GetIndividualLocationByIndividualAndLocationId(individualId, location.LocationId).IndividualLocationId > 0 ? true : false
            }).ToList();

            return locationsWithAssignedIndividualDtos;
        }

        public Location AddLocation(Location newLocation)
        {
            if (IsDuplicateLocation(newLocation))
            {
                Console.WriteLine("Location Already Exists!");
                //return new Location();

                //Location foundLocation = GetLocationByLocationId(newLocation.LocationId) ?? new Location();
                //if (foundLocation.LocationId == 0)
                //{
            }
            else
            {
                _dataContext.Location.Add(newLocation);
                _dataContext.SaveChanges();

                return newLocation;
            }

            return new Location();
        }

        public IndividualLocationsDto AddIndividualToALocation(IndividualLocationsDto individualLocationsDto)
        {
            List<Location> location = individualLocationsDto.Location;

            for (int loc = 0; loc < location.Count; loc++)
            {

                var addedLocation = AddLocation(location[loc]);
                if (addedLocation.LocationId == 0)
                {
                    Console.WriteLine("Location Already Exists!");
                    return new IndividualLocationsDto();
                }
                _individualService.GetIndividualByIndividualId(individualLocationsDto.IndividualId);
                IndividualLocation individualLocation = new IndividualLocation
                {
                    IndividualId = individualLocationsDto.IndividualId,
                    LocationId = individualLocationsDto.Location[loc].LocationId
                };
                _individualLocationService.AddIndividualLocation(individualLocation);
            }

            return individualLocationsDto;
        }

        public bool IsDuplicateLocation(Location testLocation)
        {
            bool isDuplicate = false;
            isDuplicate = !(_dataContext.Location
                .Where(loc => loc.Address == testLocation.Address &&
                    loc.City == testLocation.City &&
                    loc.State == testLocation.State &&
                    loc.Zip == testLocation.Zip)
                .IsNullOrEmpty());
            return isDuplicate;
        }
    }   
}
