using Microsoft.EntityFrameworkCore;
using TheNexusAPI.Data;
using TheNexusAPI.Entities;

namespace TheNexusAPI.Services
{
    public class IndividualPhoneNumberService
    {
        private readonly DataContext _dataContext;
        private readonly ChangeLogService _changeLog;
        private readonly ErrorLogService _errorLogService;

        public IndividualPhoneNumberService(DataContext dataContext)
        {
            _dataContext = dataContext;
            _changeLog = new ChangeLogService(_dataContext);
            _errorLogService = new ErrorLogService(_dataContext);
        }

        public List<IndividualPhoneNumber> UpdateIndividualPhoneNumber(IndividualPhoneNumber updatedIndividualPhoneNumber)
        {
            IndividualPhoneNumber? foundIndividualPhoneNumber = GetIndividualPhoneNumberByIndividualPhoneNumberId(updatedIndividualPhoneNumber.IndividualPhoneNumberId);
            // For holding foundPhoneNumber initial value and during changelog check making sure what changed
            IndividualPhoneNumber compareFoundPhoneNumber = foundIndividualPhoneNumber != null ? new IndividualPhoneNumber
            {
                IndividualPhoneNumberId = foundIndividualPhoneNumber.IndividualPhoneNumberId,
                IndividualId = foundIndividualPhoneNumber.IndividualId,
                PhoneNumberId = foundIndividualPhoneNumber.PhoneNumberId,
            } : new IndividualPhoneNumber();
            if (foundIndividualPhoneNumber != null)
            {
                foundIndividualPhoneNumber.IndividualPhoneNumberId = updatedIndividualPhoneNumber.IndividualPhoneNumberId;
                foundIndividualPhoneNumber.IndividualId = updatedIndividualPhoneNumber.IndividualId;
                foundIndividualPhoneNumber.PhoneNumberId = updatedIndividualPhoneNumber.PhoneNumberId;

                try
                {
                    _dataContext.IndividualPhoneNumber.Update(foundIndividualPhoneNumber ?? new IndividualPhoneNumber());
                    _dataContext.SaveChanges();
                    // If updates succeed, log changes
                    _changeLog.ConvertChangesForLogging(compareFoundPhoneNumber, updatedIndividualPhoneNumber);
                }
                catch (DbUpdateException ex)
                {
                    // Handle exceptions related to database updates
                    _errorLogService.GenericAddToErrorLog(ex);
                }
            }
            return _dataContext.IndividualPhoneNumber.ToList();
        }

        public IndividualPhoneNumber GetIndividualPhoneNumberByIndividualPhoneNumberId(int individualPhoneNumberId)
        {
            return _dataContext.IndividualPhoneNumber.FirstOrDefault(item => item.IndividualPhoneNumberId == individualPhoneNumberId) ?? new IndividualPhoneNumber();
        }

        public List<IndividualPhoneNumber> GetIndividualPhoneNumbersByIndividualId(int individualId)
        {
            return _dataContext.IndividualPhoneNumber.Where(il => il.IndividualId == individualId).ToList();
        }

        // Gets the IndividualPhoneNumbers by both individualId and phoneNumberId
        public IndividualPhoneNumber GetIndividualPhoneNumberByIndividualAndPhoneNumberId(int individualId, int phoneNumberId)
        {
            return _dataContext.IndividualPhoneNumber.Where(il => il.IndividualId == individualId && il.PhoneNumberId == phoneNumberId).FirstOrDefault() ?? new IndividualPhoneNumber();
        }

        //Adds an IndividualPhoneNumber to link an Individual to a PhoneNumber
        public IndividualPhoneNumber AddIndividualPhoneNumber(IndividualPhoneNumber newIndividualPhoneNumber)
        {
            IndividualPhoneNumber foundIndividualPhoneNumber = GetIndividualPhoneNumberByIndividualAndPhoneNumberId(newIndividualPhoneNumber.IndividualId, newIndividualPhoneNumber.PhoneNumberId);
            if (foundIndividualPhoneNumber != null)
            {
                _dataContext.IndividualPhoneNumber.Add(newIndividualPhoneNumber);
                _dataContext.SaveChanges();
                return newIndividualPhoneNumber;
            }

            return new IndividualPhoneNumber();
        }
    }
}
