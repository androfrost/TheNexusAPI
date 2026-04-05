using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheNexusAPI.Data;
using TheNexusAPI.Entities;
using TheNexusAPI.Services;

namespace TheNexusAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndividualPhoneNumberController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IndividualPhoneNumberService _individualPhoneNumberService;
        private readonly ErrorLogService _errorLogService;

        public IndividualPhoneNumberController(DataContext dataContext)
        {
            _dataContext = dataContext;
            _individualPhoneNumberService = new IndividualPhoneNumberService(_dataContext);
            _errorLogService = new ErrorLogService(_dataContext);
        }

        #region Get
        // Get all Individual Types
        [HttpGet]
        public List<IndividualPhoneNumber> GetIndividualPhoneNumbers()
        {
            return _dataContext.IndividualPhoneNumber.ToList();
        }

        // Get all records with given IndividualId and PhoneNumberId
        [HttpGet("{ individualId }/{ phoneNumberId }")]
        public List<IndividualPhoneNumber> GetIndividualPhoneNumberByIndividualIdAndPhoneNumberId(int individualId, int phoneNumberId)
        {
            return _dataContext.IndividualPhoneNumber.Where(il => il.IndividualId == individualId && il.PhoneNumberId == phoneNumberId).ToList();
        }

        // Get all IndividualPhoneNumber records with given IndividualId
        [HttpGet("individual")]
        public List<IndividualPhoneNumber> GetActiveIndividualPhoneNumbersByIndividualId(int individualId)
        {
            return _dataContext.IndividualPhoneNumber.Where(il => il.IndividualId == individualId).ToList();
        }

        // Get all IndividualPhoneNumber records with given PhoneNumberId
        [HttpGet("phoneNumber")]
        public List<IndividualPhoneNumber> GetActiveIndividualPhoneNumberByPhoneNumberId(int phoneNumberId)
        {
            return _dataContext.IndividualPhoneNumber.Where(il => il.PhoneNumberId == phoneNumberId).ToList();
        }

        // Get the record with the IndividualPhoneNumberId
        [HttpGet("individualPhoneNumberId")]
        public IndividualPhoneNumber GetIndividualPhoneNumberByIndividualPhoneNumberId(int individualPhoneNumberId)
        {
            return _individualPhoneNumberService.GetIndividualPhoneNumberByIndividualPhoneNumberId(individualPhoneNumberId);
        }

        // Get the record with the IndividualId and PhoneNumberId
        [HttpGet("individualAndPhoneNumber")]
        public IndividualPhoneNumber GetIndividualPhoneNumberByIndividualAndPhoneNumberId(int individualId, int phoneNumberId)
        {
            return _individualPhoneNumberService.GetIndividualPhoneNumberByIndividualAndPhoneNumberId(individualId, phoneNumberId);
        }

        #endregion

        #region Update

        //Update a specific individual type with new data based on passed individual type
        [HttpPut]
        public List<IndividualPhoneNumber> UpdateIndividualPhoneNumber(IndividualPhoneNumber individualPhoneNumber)
        {
            return _individualPhoneNumberService.UpdateIndividualPhoneNumber(individualPhoneNumber);
        }

        #endregion

        #region Add

        //Add the given individualPhoneNumber if they do not already exist
        [HttpPost]
        public IndividualPhoneNumber AddIndividualPhoneNumber(IndividualPhoneNumber individualPhoneNumber)
        {
            return _individualPhoneNumberService.AddIndividualPhoneNumber(individualPhoneNumber);
        }

        #endregion

        #region Delete

        // Delete the given individualPhoneNumber if found
        [HttpDelete]
        public void DeleteIndividualPhoneNumber(int individualPhoneNumberId)
        {
            _dataContext.IndividualPhoneNumber.Remove(GetIndividualPhoneNumberByIndividualPhoneNumberId(individualPhoneNumberId));
            _dataContext.SaveChanges();
        }

        // Deletes a phoneNumber based on both the individualId and the phoneNumberId 
        [HttpDelete("IndividualPhoneNumberDelete/{individualId}/{phoneNumberId}")]
        public void DeleteIndividualPhoneNumberByIndividualAndPhoneNumberId(int individualId, int phoneNumberId)
        {
            try
            {
                _dataContext.IndividualPhoneNumber.Remove(GetIndividualPhoneNumberByIndividualAndPhoneNumberId(individualId, phoneNumberId));
                _dataContext.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                // Handle exceptions related to database updates
                _errorLogService.GenericAddToErrorLog(ex);
            }
        }

        #endregion
    }
}
