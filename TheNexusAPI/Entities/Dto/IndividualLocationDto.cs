using System.ComponentModel.DataAnnotations;

namespace TheNexusAPI.Entities.Dto
{
    public class IndividualLocationDto
    {
        [Key]
        public int IndividualLocationId { get; set; }
        public int IndividualId { get; set; }
        public int LocationId { get; set; }
        public Location[] Location { get; set; } = [];
    }
}
