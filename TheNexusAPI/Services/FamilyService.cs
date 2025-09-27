using TheNexusAPI.Data;
using TheNexusAPI.Entities;

namespace TheNexusAPI.Services
{
    public class FamilyService
    {
        private readonly DataContext _dataContext;
        private readonly FamilyService _familyService;

        public FamilyService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Family? GetFamilyByFamilyId(int familyId, List<Family> family)
        {
            return family.FirstOrDefault(item => item.FamilyId == familyId);// ?? new Family();
        }

        public Family? GetFamilyByFamilyId(int familyId)
        {
            return _dataContext.Family.FirstOrDefault(item => item.FamilyId == familyId);// ?? new Family();
        }

        public List<Family> UpdateFamily(Family updatedFamily, List<Family> families)
        {
            Family? foundFamily = GetFamilyByFamilyId(updatedFamily.FamilyId, families);
            if (foundFamily != null)
            {
                foundFamily.FamilyId = updatedFamily.FamilyId;
                foundFamily.FamilyName = updatedFamily.FamilyName;
                foundFamily.FamilyDescription = updatedFamily.FamilyDescription;

            }

            return families;
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
