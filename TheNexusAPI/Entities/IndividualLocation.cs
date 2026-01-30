using System.ComponentModel.DataAnnotations;

namespace TheNexusAPI.Entities
{
    public class IndividualLocation
    {
        [Key]
        public int IndividualLocationId { get; set; }
        public int IndividualId { get; set; }
        public int LocationId {get; set; }
    }
}
