using System.ComponentModel.DataAnnotations;

namespace TheNexusAPI.Entities
{
    public class IndividualType
    {
        [Key]
        public int IndividualTypeId { get; set; }
        public string IndividualTypeName {  get; set; } = string.Empty;
        public bool Active { get; set; } = false;
    }
}
