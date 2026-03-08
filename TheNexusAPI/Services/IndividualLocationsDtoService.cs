using Microsoft.EntityFrameworkCore;
using TheNexusAPI.Data;
using TheNexusAPI.Entities;
using TheNexusAPI.Entities.Dto;

namespace TheNexusAPI.Services
{
    public class IndividualLocationsDtoService
    {
        private readonly DataContext _dataContext;
        private readonly ChangeLogService _changeLog;
        private readonly LocationService _locationService;

        public IndividualLocationsDtoService(DataContext dataContext)
        {
            _dataContext = dataContext;
            _changeLog = new ChangeLogService(_dataContext);
            _locationService = new LocationService(_dataContext);
        }

        public List<IndividualLocationsDto> GetIndividualLocationsDto()
        {
            List<IndividualLocationsDto> individualLocationsDto = (from il in _dataContext.IndividualLocation
                                                             join l in _dataContext.Location
                                                             on il.LocationId equals l.LocationId
                                                             select new IndividualLocationsDto
                                                             {
                                                                 IndividualLocationId = il.IndividualLocationId,
                                                                 IndividualId = il.IndividualId,
                                                                 LocationId = il.LocationId,
                                                             }).ToList();
            foreach (IndividualLocationsDto individualLocationDto in individualLocationsDto)
                individualLocationDto.Location = _locationService.GetLocationsByIndividualId(individualLocationDto.IndividualId);

            return individualLocationsDto;
        }

        public List<IndividualLocationsDto> GetIndividualLocationsByIndividualIdDto(int individualId)
        {
            List<IndividualLocationsDto> individualLocationsDto = (from il in _dataContext.IndividualLocation
                                                                   join l in _dataContext.Location
                                                                   on il.LocationId equals l.LocationId
                                                                   where il.LocationId == individualId
                                                                   select new IndividualLocationsDto
                                                                   {
                                                                       IndividualLocationId = il.IndividualLocationId,
                                                                       IndividualId = il.IndividualId,
                                                                       LocationId = il.LocationId,
                                                                   }).ToList();
            foreach (IndividualLocationsDto individualLocationDto in individualLocationsDto)
                individualLocationDto.Location = _locationService.GetLocationsByIndividualId(individualLocationDto.IndividualId);

            return individualLocationsDto;
        }
    }
}
