using Microsoft.EntityFrameworkCore;
using TheNexusAPI.Data;
using TheNexusAPI.Entities;

namespace TheNexusAPI.Services
{
    public class IndividualLocationService
    {
        private readonly DataContext _dataContext;
        private readonly ChangeLogService _changeLog;

        public IndividualLocationService(DataContext dataContext)
        {
            _dataContext = dataContext;
            _changeLog = new ChangeLogService(_dataContext);
        }

        public List<IndividualLocation> UpdateIndividualLocation(IndividualLocation updatedIndividualLocation)
        {
            IndividualLocation? foundIndividualLocation = GetIndividualLocationByIndividualLocationId(updatedIndividualLocation.IndividualLocationId);
            // For holding foundLocation initial value and during changelog check making sure what changed
            IndividualLocation compareFoundLocation = foundIndividualLocation != null ? new IndividualLocation
            {
                IndividualLocationId = foundIndividualLocation.IndividualLocationId,
                IndividualId = foundIndividualLocation.IndividualId,
                LocationId = foundIndividualLocation.LocationId,
            } : new IndividualLocation();
            if (foundIndividualLocation != null)
            {
                foundIndividualLocation.IndividualLocationId = updatedIndividualLocation.IndividualLocationId;
                foundIndividualLocation.IndividualId = updatedIndividualLocation.IndividualId;
                foundIndividualLocation.LocationId = updatedIndividualLocation.LocationId;
            }

            try
            {
                _dataContext.IndividualLocation.Update(foundIndividualLocation ?? new IndividualLocation());
                _dataContext.SaveChanges();
                // If updates succeed, log changes
                _changeLog.ConvertChangesForLogging(compareFoundLocation, updatedIndividualLocation);
            }
            catch (DbUpdateException ex)
            {
                // Handle exceptions related to database updates
                Console.WriteLine($"An error occurred while updating the location: {ex.Message}");
            }
            return _dataContext.IndividualLocation.ToList();
        }

        public IndividualLocation GetIndividualLocationByIndividualLocationId(int individualLocationId)
        {
            return _dataContext.IndividualLocation.FirstOrDefault(item => item.IndividualLocationId == individualLocationId) ?? new IndividualLocation();
        }

        public List<IndividualLocation> GetIndividualLocationsByIndividualId(int individualId)
        {
            return _dataContext.IndividualLocation.Where(il => il.IndividualId == individualId).ToList();
        }

        public IndividualLocation AddIndividualLocation(IndividualLocation newIndividualLocation)
        {
            _dataContext.IndividualLocation.Add(newIndividualLocation);
            _dataContext.SaveChanges();
            
            return newIndividualLocation;
        }
    }
}
