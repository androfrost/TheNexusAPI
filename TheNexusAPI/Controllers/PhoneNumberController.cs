using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheNexusAPI.Data;
using TheNexusAPI.Entities;
using TheNexusAPI.Entities.Dto;
using TheNexusAPI.Services;

namespace TheNexusAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhoneNumberController : ControllerBase
    {

        private readonly DataContext _dataContext;
        private readonly PhoneNumberService _phoneNumberService;

        public PhoneNumberController(DataContext dataContext)
        {
            _dataContext = dataContext;
            _phoneNumberService = new PhoneNumberService(_dataContext);

        }

        #region Get
        // Get all PhoneNumbers
        [HttpGet]
        public List<PhoneNumber> GetPhoneNumbers()
        {
            return _phoneNumberService.GetPhoneNumbers();
        }

        [HttpGet("phoneNumberId")]
        public PhoneNumber GetPhoneNumberByPhoneNumberId(int phoneNumberId)
        {
            return _phoneNumberService.GetPhoneNumberByPhoneNumberId(phoneNumberId);
        }

        [HttpGet("individualsPhoneNumbers/{individualId}")]
        public List<PhoneNumber> GetPhoneNumbersByIndividualId(int individualId)
        {
            return _phoneNumberService.GetPhoneNumbersByIndividualId(individualId);
        }

        //Gets IndividualPhoneNumbers and the Assigned Flag into a Dto so they can be displayed if the chosen individual is assigned
        [HttpGet("GetIndividualsPhoneNumberWithAssignedIndividual/{individualId}")]
        public List<PhoneNumbersWithAssignedIndividualDto> GetIndividualsPhoneNumberWithAssignedIndividual(int individualId)
        {
            return _phoneNumberService.GetIndividualsPhoneNumberWithAssignedIndividual(individualId);
        }

        #endregion

        #region Update

        //Update a specific phoneNumber with new data based on passed phoneNumber
        [HttpPut]
        public List<PhoneNumber> UpdatePhoneNumber(PhoneNumber phoneNumber)
        {
            return _phoneNumberService.UpdatePhoneNumber(phoneNumber);
        }

        #endregion

        #region Add

        //Add the given phoneNumber if they do not already exist
        [HttpPost]
        public PhoneNumber AddPhoneNumber(PhoneNumber phoneNumber)
        {
            return _phoneNumberService.AddPhoneNumber(phoneNumber);
        }

        [HttpPost("IndividualPhoneNumber")]
        public IndividualPhoneNumbersDto AddIndividualToAPhoneNumber(IndividualPhoneNumbersDto individualPhoneNumberDto)
        {
            return _phoneNumberService.AddIndividualToAPhoneNumber(individualPhoneNumberDto);
        }

        #endregion
    }
}
