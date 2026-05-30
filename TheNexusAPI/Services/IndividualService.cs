using Microsoft.EntityFrameworkCore;
using TheNexusAPI.Data;
using TheNexusAPI.Entities;
using TheNexusAPI.Enums;

namespace TheNexusAPI.Services
{
    public class IndividualService
    {
        private readonly DataContext _dataContext;
        private readonly ChangeLogService _changeLog;
        private readonly ErrorLogService _errorLogService;

        public IndividualService(DataContext dataContext)
        {
            _dataContext = dataContext;
            _changeLog = new ChangeLogService(_dataContext);
            _errorLogService = new ErrorLogService(_dataContext);
        }

        public Individual? GetIndividualByIndividualId(int individualId, List<Individual> individual)
        {
            return individual.FirstOrDefault(item => item.IndividualId == individualId);// ?? new Individual();
        }

        public Individual? GetIndividualByIndividualId(int individualId)
        {
            return _dataContext.Individual.FirstOrDefault(item => item.IndividualId == individualId);// ?? new Individual();
        }

        public List<Individual>? GetIndividualByStatusId(int statusId)
        {
            return _dataContext.Individual.Where(item => item.StatusId == statusId).ToList();// ?? new Individual();
        }

        public List<Individual> UpdateIndividual(Individual updatedIndividual)
        {
            Individual? foundIndividual = GetIndividualByIndividualId(updatedIndividual.IndividualId);
            // For holding foundIndividual initial value and during changelog check making sure what changed
            Individual compareFoundIndividual = foundIndividual != null ? new Individual
            {
                IndividualId = foundIndividual.IndividualId,
                GroupId = foundIndividual.GroupId,
                FirstName = foundIndividual.FirstName,
                LastName = foundIndividual.LastName,
                LocationId = foundIndividual.LocationId,
                DateOfBirth = foundIndividual.DateOfBirth,
                StatusId = foundIndividual.StatusId,
                PhoneNumberId = foundIndividual.PhoneNumberId,
                SexId = foundIndividual.SexId,
                IndividualDescription = foundIndividual.IndividualDescription,
                IndividualTypeId = foundIndividual.IndividualTypeId
            } : new Individual();
            if (foundIndividual != null)
            {
                foundIndividual.GroupId = updatedIndividual.GroupId;
                foundIndividual.FirstName = updatedIndividual.FirstName;
                foundIndividual.LastName = updatedIndividual.LastName;
                foundIndividual.LocationId = updatedIndividual.LocationId;
                foundIndividual.DateOfBirth = updatedIndividual.DateOfBirth;
                foundIndividual.StatusId = updatedIndividual.StatusId;
                foundIndividual.PhoneNumberId = updatedIndividual.PhoneNumberId;
                foundIndividual.SexId = updatedIndividual.SexId;
                foundIndividual.IndividualDescription = updatedIndividual.IndividualDescription;
                foundIndividual.IndividualTypeId = updatedIndividual.IndividualTypeId;


                try
                {
                    _dataContext.Individual.Update(foundIndividual ?? new Individual());
                    _dataContext.SaveChanges();
                    // If updates succeed, log changes
                    _changeLog.ConvertChangesForLogging(compareFoundIndividual, updatedIndividual, 0, (int)ChangeType.Update);
                }
                catch (DbUpdateException ex)
                {
                    // Handle exceptions related to database updates
                    _errorLogService.GenericAddToErrorLog(ex);
                }
            }
            return _dataContext.Individual.ToList();
        }

        public Individual AddIndividual(Individual newIndividual)//, List<Individual> individuals)
        {

            Individual foundIndividual = GetIndividualByIndividualId(newIndividual.IndividualId) ?? new Individual();
            if (foundIndividual.IndividualId == 0)
            {
                _dataContext.Individual.Add(newIndividual);
                _dataContext.SaveChanges();
                // If add succeeds, log changes
                _changeLog.ConvertChangesForLogging(new Individual(), newIndividual, 0, (int)ChangeType.Create);

                return newIndividual;
            }

            return new Individual();
        }
    }   
}