using TheNexusAPI.Data;
using TheNexusAPI.Entities;

namespace TheNexusAPI.Services
{
    public class IndividualService
    {
        DataContext _dataContext;

        public IndividualService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Individual? GetIndividualByIndividualId(int individualId, List<Individual> individual)
        {
            return individual.FirstOrDefault(item => item.IndividualId == individualId);// ?? new Individual();
        }

        public List<Individual> UpdateIndividual(Individual updatedIndividual, List<Individual> individuals)
        {
            Individual? foundIndividual = GetIndividualByIndividualId(updatedIndividual.IndividualId, individuals);
            if (foundIndividual != null)
            {
                foundIndividual.FamilyId = updatedIndividual.FamilyId;
                foundIndividual.FirstName = updatedIndividual.FirstName;
                foundIndividual.LastName = updatedIndividual.LastName;
                foundIndividual.LocationId = updatedIndividual.LocationId;
                foundIndividual.DateOfBirth = updatedIndividual.DateOfBirth;
                foundIndividual.StatusId = updatedIndividual.StatusId;
                foundIndividual.PhoneNumberId = updatedIndividual.StatusId;
                foundIndividual.SexId = updatedIndividual.SexId;
                foundIndividual.Description = updatedIndividual.Description;
                foundIndividual.IndividualTypeId = updatedIndividual.IndividualTypeId;
            }

            return individuals;
        }

        public List<Individual> AddIndividual(Individual newIndividual, List<Individual> individuals)
        {
            Individual foundIndividual = GetIndividualByIndividualId(newIndividual.IndividualId, individuals);
            if (foundIndividual.IndividualId == 0 && newIndividual.IndividualId != 0)
            {
                individuals.Add(newIndividual);
            }
            return individuals;
        }
    }   
}
