using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TheNexusAPI.Data;
using TheNexusAPI.Entities;
using TheNexusAPI.Entities.Dto;

namespace TheNexusAPI.Services
{
    public class PhoneNumberService
    {
        private readonly DataContext _dataContext;
        private readonly ChangeLogService _changeLog;
        private readonly IndividualService _individualService;
        private readonly IndividualPhoneNumberService _individualPhoneNumberService;
        private readonly ErrorLogService _errorLogService;

        public PhoneNumberService(DataContext dataContext)
        {
            _dataContext = dataContext;
            _changeLog = new ChangeLogService(_dataContext);
            _individualPhoneNumberService = new IndividualPhoneNumberService(_dataContext);
            _individualService = new IndividualService(_dataContext);
            _errorLogService = new ErrorLogService(_dataContext);
        }

        public List<PhoneNumber> UpdatePhoneNumber(PhoneNumber updatedPhoneNumber)
        {
            PhoneNumber? foundPhoneNumber = GetPhoneNumberByPhoneNumberId(updatedPhoneNumber.PhoneNumberId);
            // For holding foundPhoneNumber initial value and during changelog check making sure what changed
            PhoneNumber compareFoundPhoneNumber = foundPhoneNumber != null ? new PhoneNumber
            {
                PhoneNumberId = foundPhoneNumber.PhoneNumberId,
                PhoneNumberValue = foundPhoneNumber.PhoneNumberValue,
                PhoneNumberTypeId = foundPhoneNumber.PhoneNumberTypeId
            } : new PhoneNumber();
            if (foundPhoneNumber != null)
            {
                foundPhoneNumber.PhoneNumberId = updatedPhoneNumber.PhoneNumberId;
                foundPhoneNumber.PhoneNumberValue = updatedPhoneNumber.PhoneNumberValue;
                foundPhoneNumber.PhoneNumberTypeId = updatedPhoneNumber.PhoneNumberTypeId;

                try
                {
                    _dataContext.PhoneNumber.Update(foundPhoneNumber ?? new PhoneNumber());
                    _dataContext.SaveChanges();
                    // If updates succeed, log changes
                    _changeLog.ConvertChangesForLogging(compareFoundPhoneNumber, updatedPhoneNumber);
                }
                catch (DbUpdateException ex)
                {
                    // Handle exceptions related to database updates
                    _errorLogService.GenericAddToErrorLog(ex);
                }
            }
            return _dataContext.PhoneNumber.ToList();
        }

        public List<PhoneNumber> GetPhoneNumbers()
        {
            return _dataContext.PhoneNumber.ToList();
        }

        public PhoneNumber GetPhoneNumberByPhoneNumberId(int phoneNumberId)
        {
            return _dataContext.PhoneNumber.FirstOrDefault(item => item.PhoneNumberId == phoneNumberId) ?? new PhoneNumber();
        }

        public List<PhoneNumber> GetPhoneNumbersByIndividualId(int individualId)
        {
            List<IndividualPhoneNumber> individualPhoneNumber = _individualPhoneNumberService.GetIndividualPhoneNumbersByIndividualId(individualId);
            List<PhoneNumber> allPhoneNumbers = GetPhoneNumbers();
            List<PhoneNumber> phoneNumbers = allPhoneNumbers.Join(
                individualPhoneNumber,                 // inner sequence
                phoneNumber => phoneNumber.PhoneNumberId,    // outer key selector
                individualPhoneNumber => individualPhoneNumber.PhoneNumberId,  // inner key selector
            (phoneNumber, individualPhoneNumber) => new PhoneNumber    // result selector
            {
                PhoneNumberId = phoneNumber.PhoneNumberId,
                PhoneNumberValue = phoneNumber.PhoneNumberValue,
                PhoneNumberTypeId = phoneNumber.PhoneNumberTypeId,
            }).ToList();

            return phoneNumbers;
        }

        public List<PhoneNumbersWithAssignedIndividualDto> GetIndividualsPhoneNumberWithAssignedIndividual(int individualId)
        {
            List<PhoneNumber> allPhoneNumbers = GetPhoneNumbers();
            List<PhoneNumbersWithAssignedIndividualDto> phoneNumbersWithAssignedIndividualDtos = allPhoneNumbers.Select(phoneNumber => new PhoneNumbersWithAssignedIndividualDto
            {
                PhoneNumberId = phoneNumber.PhoneNumberId,
                PhoneNumberValue = phoneNumber.PhoneNumberValue,
                PhoneNumberTypeId = phoneNumber.PhoneNumberTypeId,
                IsAssigned = _individualPhoneNumberService.GetIndividualPhoneNumberByIndividualAndPhoneNumberId(individualId, phoneNumber.PhoneNumberId).IndividualPhoneNumberId > 0 ? true : false
            }).ToList();

            return phoneNumbersWithAssignedIndividualDtos;
        }

        public PhoneNumber AddPhoneNumber(PhoneNumber newPhoneNumber)
        {
            if (IsDuplicatePhoneNumber(newPhoneNumber))
            {
                Console.WriteLine("PhoneNumber Already Exists!");
                //return new PhoneNumber();

                //PhoneNumber foundPhoneNumber = GetPhoneNumberByPhoneNumberId(newPhoneNumber.PhoneNumberId) ?? new PhoneNumber();
                //if (foundPhoneNumber.PhoneNumberId == 0)
                //{
            }
            else
            {
                _dataContext.PhoneNumber.Add(newPhoneNumber);
                _dataContext.SaveChanges();

                return newPhoneNumber;
            }

            return new PhoneNumber();
        }

        public IndividualPhoneNumbersDto AddIndividualToAPhoneNumber(IndividualPhoneNumbersDto individualPhoneNumbersDto)
        {
            List<PhoneNumber> phoneNumber = individualPhoneNumbersDto.PhoneNumber;

            for (int loc = 0; loc < phoneNumber.Count; loc++)
            {

                var addedPhoneNumber = AddPhoneNumber(phoneNumber[loc]);
                if (addedPhoneNumber.PhoneNumberId == 0)
                {
                    Console.WriteLine("PhoneNumber Already Exists!");
                    return new IndividualPhoneNumbersDto();
                }
                _individualService.GetIndividualByIndividualId(individualPhoneNumbersDto.IndividualId);
                IndividualPhoneNumber individualPhoneNumber = new IndividualPhoneNumber
                {
                    IndividualId = individualPhoneNumbersDto.IndividualId,
                    PhoneNumberId = individualPhoneNumbersDto.PhoneNumber[loc].PhoneNumberId
                };
                _individualPhoneNumberService.AddIndividualPhoneNumber(individualPhoneNumber);
            }

            return individualPhoneNumbersDto;
        }

        public bool IsDuplicatePhoneNumber(PhoneNumber testPhoneNumber)
        {
            bool isDuplicate = false;
            isDuplicate = !(_dataContext.PhoneNumber
                .Where(pn => pn.PhoneNumberValue == testPhoneNumber.PhoneNumberValue)
                .IsNullOrEmpty());
            return isDuplicate;
        }
    }
}
