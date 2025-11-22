using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;
using TheNexusAPI.Data;
using TheNexusAPI.Entities;

namespace TheNexusAPI.Services
{
    public class FamilyService
    {
        private readonly DataContext _dataContext;
        private readonly FamilyService _familyService;
        private readonly ChangeLogService _changeLog;

        public FamilyService(DataContext dataContext)
        {
            _dataContext = dataContext;
            _changeLog = new ChangeLogService(_dataContext);
        }

        public Family? GetFamilyByFamilyId(int familyId, List<Family> family)
        {
            return family.FirstOrDefault(item => item.FamilyId == familyId);// ?? new Family();
        }

        public Family? GetFamilyByFamilyId(int familyId)
        {
            return _dataContext.Family.FirstOrDefault(item => item.FamilyId == familyId);// ?? new Family();
        }

        public List<Family> UpdateFamily(Family updatedFamily)
        {
            Family? foundFamily = GetFamilyByFamilyId(updatedFamily.FamilyId);
            // For holding foundFamily initial value and during changelog check making sure what changed
            Family compareFoundFamily = foundFamily != null ? new Family
            {
                FamilyId = foundFamily.FamilyId,
                FamilyName = foundFamily.FamilyName,
                FamilyDescription = foundFamily.FamilyDescription
            } : new Family();

            if (foundFamily != null)
            {
                foundFamily.FamilyId = updatedFamily.FamilyId;
                foundFamily.FamilyName = updatedFamily.FamilyName;
                foundFamily.FamilyDescription = updatedFamily.FamilyDescription;

            }

            try { 
                _dataContext.Family.Update(foundFamily ?? new Family());
                _dataContext.SaveChanges();
                // If updates succeed, log changes
                _changeLog.ConvertChangesForLogging(compareFoundFamily, updatedFamily);
            }
            catch(DbUpdateException ex)
            {
                // Handle exceptions related to database updates
                Console.WriteLine($"An error occurred while updating the family: {ex.Message}");
            }
            return _dataContext.Family.ToList();
        }

        public Family AddFamily(Family newFamily)//, List<Family> families)
        {

            Family foundFamily = GetFamilyByFamilyId(newFamily.FamilyId) ?? new Family();

            if (foundFamily.FamilyId == 0)
            {
                _dataContext.Family.Add(newFamily);
                _dataContext.SaveChanges();

                return newFamily;
            }

            return new Family();
        }
    }
}
