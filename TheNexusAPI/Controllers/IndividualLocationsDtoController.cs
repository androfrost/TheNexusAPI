using Microsoft.AspNetCore.Mvc;
using TheNexusAPI.Data;
using TheNexusAPI.Entities.Dto;
using TheNexusAPI.Services;

namespace TheNexusAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndividualLocationsDtoController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IndividualLocationsDtoService _individualLocationsDtoService;

        public IndividualLocationsDtoController(DataContext dataContext)
        {
            _dataContext = dataContext;
            _individualLocationsDtoService = new IndividualLocationsDtoService(_dataContext);
        }

        // Get all individuals and their locations.
        [HttpGet]
        public List<IndividualLocationsDto> GetIndividualLocationsDto()
        {
            return _individualLocationsDtoService.GetIndividualLocationsDto();
        }

        // Get an individual based on their individualId and all their locations.
        [HttpGet("{individualId}")]
        public List<IndividualLocationsDto> GetIndividualLocationsByIndividualIdDto(int individualId)
        {
            return _individualLocationsDtoService.GetIndividualLocationsByIndividualIdDto(individualId);
        }

    }
}
