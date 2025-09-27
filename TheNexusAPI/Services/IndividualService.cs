using Microsoft.EntityFrameworkCore;
using TheNexusAPI.Data;
using TheNexusAPI.Entities;

namespace TheNexusAPI.Services
{
    public class IndividualService
    {
        private readonly DataContext _dataContext;
        private readonly IndividualService _individualService;

        public IndividualService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Individual? GetIndividualByIndividualId(int individualId, List<Individual> individual)
        {
            return individual.FirstOrDefault(item => item.IndividualId == individualId);// ?? new Individual();
        }

        public Individual? GetIndividualByIndividualId(int individualId)
        {
            return _dataContext.Individual.FirstOrDefault(item => item.IndividualId == individualId);// ?? new Individual();
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
                foundIndividual.IndividualDescription = updatedIndividual.IndividualDescription;
                foundIndividual.IndividualTypeId = updatedIndividual.IndividualTypeId;
            }

            return individuals;
        }

        public Individual AddIndividual(Individual newIndividual)//, List<Individual> individuals)
        {

            Individual foundIndividual = GetIndividualByIndividualId(newIndividual.IndividualId) ?? new Individual();
            if (foundIndividual.IndividualId == 0)
            {
                _dataContext.Individual.Add(newIndividual);
                _dataContext.SaveChanges();

                return newIndividual;
            }

            return new Individual();
        }
    }   
}
