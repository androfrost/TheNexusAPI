using Microsoft.EntityFrameworkCore;
using TheNexusAPI.Data;
using TheNexusAPI.Entities;
using TheNexusAPI.Enums;

namespace TheNexusAPI.Services
{
    public class IndividualLocationService
    {
        private readonly DataContext _dataContext;
        private readonly ChangeLogService _changeLog;
        private readonly ErrorLogService _errorLogService;

        public IndividualLocationService(DataContext dataContext)
        {
            _dataContext = dataContext;
            _changeLog = new ChangeLogService(_dataContext);
            _errorLogService = new ErrorLogService(_dataContext);
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

                try
                {
                    _dataContext.IndividualLocation.Update(foundIndividualLocation ?? new IndividualLocation());
                    _dataContext.SaveChanges();
                    // If updates succeed, log changes
                    _changeLog.ConvertChangesForLogging(compareFoundLocation, updatedIndividualLocation, 0, (int)ChangeType.Update);
                }
                catch (DbUpdateException ex)
                {
                    // Handle exceptions related to database updates
                    _errorLogService.GenericAddToErrorLog(ex);
                }
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

        // Gets the IndividualLocations by both individualId and locationId
        public IndividualLocation GetIndividualLocationByIndividualAndLocationId(int individualId, int locationId)
        {
            return _dataContext.IndividualLocation.Where(il => il.IndividualId == individualId && il.LocationId == locationId).FirstOrDefault() ?? new IndividualLocation();
        }

        //Adds an IndividualLocation to link an Individual to a Location
        public IndividualLocation AddIndividualLocation(IndividualLocation newIndividualLocation)
        {
            IndividualLocation foundIndividualLocation = GetIndividualLocationByIndividualAndLocationId(newIndividualLocation.IndividualId, newIndividualLocation.LocationId);
            if (foundIndividualLocation != null)
            {
                _dataContext.IndividualLocation.Add(newIndividualLocation);
                _dataContext.SaveChanges();
                // If add succeeds, log changes
                _changeLog.ConvertChangesForLogging(new IndividualLocation(), newIndividualLocation, 0, (int)ChangeType.Create);

                return newIndividualLocation;
            }

            return new IndividualLocation();
        }
    }
}
