using System.ComponentModel.DataAnnotations;

namespace TheNexusAPI.Entities.Dto
{
    public class IndividualLocationsDto
    {
        [Key]
        public int IndividualLocationId { get; set; }
        public int IndividualId { get; set; }
        public int LocationId { get; set; }
        public List<Location> Location { get; set; } = [];
    }
}
