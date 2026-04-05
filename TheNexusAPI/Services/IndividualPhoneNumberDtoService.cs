using Microsoft.EntityFrameworkCore;
using TheNexusAPI.Data;
using TheNexusAPI.Entities;
using TheNexusAPI.Entities.Dto;

namespace TheNexusAPI.Services
{
    public class IndividualPhoneNumbersDtoService
    {
        private readonly DataContext _dataContext;
        private readonly ChangeLogService _changeLog;
        private readonly PhoneNumberService _phoneNumberService;

        public IndividualPhoneNumbersDtoService(DataContext dataContext)
        {
            _dataContext = dataContext;
            _changeLog = new ChangeLogService(_dataContext);
            _phoneNumberService = new PhoneNumberService(_dataContext);
        }

        public List<IndividualPhoneNumbersDto> GetIndividualPhoneNumbersDto()
        {
            List<IndividualPhoneNumbersDto> individualPhoneNumbersDto = (from il in _dataContext.IndividualPhoneNumber
                                                                   join l in _dataContext.PhoneNumber
                                                                   on il.PhoneNumberId equals l.PhoneNumberId
                                                                   select new IndividualPhoneNumbersDto
                                                                   {
                                                                       IndividualPhoneNumberId = il.IndividualPhoneNumberId,
                                                                       IndividualId = il.IndividualId,
                                                                       PhoneNumberId = il.PhoneNumberId,
                                                                   }).ToList();
            foreach (IndividualPhoneNumbersDto individualPhoneNumberDto in individualPhoneNumbersDto)
                individualPhoneNumberDto.PhoneNumber = _phoneNumberService.GetPhoneNumbersByIndividualId(individualPhoneNumberDto.IndividualId);

            return individualPhoneNumbersDto;
        }

        public List<IndividualPhoneNumbersDto> GetIndividualPhoneNumbersByIndividualIdDto(int individualId)
        {
            List<IndividualPhoneNumbersDto> individualPhoneNumbersDto = (from il in _dataContext.IndividualPhoneNumber
                                                                   join l in _dataContext.PhoneNumber
                                                                   on il.PhoneNumberId equals l.PhoneNumberId
                                                                   where il.PhoneNumberId == individualId
                                                                   select new IndividualPhoneNumbersDto
                                                                   {
                                                                       IndividualPhoneNumberId = il.IndividualPhoneNumberId,
                                                                       IndividualId = il.IndividualId,
                                                                       PhoneNumberId = il.PhoneNumberId,
                                                                   }).ToList();
            foreach (IndividualPhoneNumbersDto individualPhoneNumberDto in individualPhoneNumbersDto)
                individualPhoneNumberDto.PhoneNumber = _phoneNumberService.GetPhoneNumbersByIndividualId(individualPhoneNumberDto.IndividualId);

            return individualPhoneNumbersDto;
        }
    }
}
