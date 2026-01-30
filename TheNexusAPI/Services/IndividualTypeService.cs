using Microsoft.EntityFrameworkCore;
using TheNexusAPI.Data;
using TheNexusAPI.Entities;

namespace TheNexusAPI.Services
{
    public class IndividualTypeService
    {
        private readonly DataContext _dataContext;
        private readonly ChangeLogService _changeLog;

        public IndividualTypeService(DataContext dataContext)
        {
            _dataContext = dataContext;
            _changeLog = new ChangeLogService(_dataContext);
        }

        public List<IndividualType> UpdateIndividualType(IndividualType updatedIndividualType)
        {
            IndividualType? foundIndividualType = GetIndividualTypeByIndividualTypeId(updatedIndividualType.IndividualTypeId);
            // For holding foundIndividualType initial value and during changelog check making sure what changed
            IndividualType compareFoundIndividual = foundIndividualType != null ? new IndividualType
            {
                IndividualTypeId = foundIndividualType.IndividualTypeId,
                IndividualTypeName = foundIndividualType.IndividualTypeName,
                Active = foundIndividualType.Active
            } : new IndividualType();
            if (foundIndividualType != null)
            {
                foundIndividualType.IndividualTypeId = updatedIndividualType.IndividualTypeId;
                foundIndividualType.IndividualTypeName = updatedIndividualType.IndividualTypeName;
                foundIndividualType.Active = updatedIndividualType.Active;
            }

            try
            {
                _dataContext.IndividualType.Update(foundIndividualType ?? new IndividualType());
                _dataContext.SaveChanges();
                // If updates succeed, log changes
                _changeLog.ConvertChangesForLogging(compareFoundIndividual, updatedIndividualType);
            }
            catch (DbUpdateException ex)
            {
                // Handle exceptions related to database updates
                Console.WriteLine($"An error occurred while updating the individual type: {ex.Message}");
            }
            return _dataContext.IndividualType.ToList();
        }

        public IndividualType GetIndividualTypeByIndividualTypeId(int individualTypeId)
        {
           return _dataContext.IndividualType.FirstOrDefault(item => item.IndividualTypeId == individualTypeId) ?? new IndividualType();
        }

        public IndividualType AddIndividualType(IndividualType newIndividualType)
        {

            IndividualType foundIndividualType = GetIndividualTypeByIndividualTypeId(newIndividualType.IndividualTypeId) ?? new IndividualType();
            if (foundIndividualType.IndividualTypeId == 0)
            {
                _dataContext.IndividualType.Add(newIndividualType);
                _dataContext.SaveChanges();

                return newIndividualType;
            }

            return new IndividualType();
        }
    }
}
