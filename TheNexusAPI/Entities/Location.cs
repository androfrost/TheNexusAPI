using System.ComponentModel.DataAnnotations;

namespace TheNexusAPI.Entities
{
    public class Location
    {
        [Key]
        public int LocationId { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;
    }
}
