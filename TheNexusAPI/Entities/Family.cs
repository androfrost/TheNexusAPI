using System.ComponentModel.DataAnnotations;

namespace TheNexusAPI.Entities
{
    public class Family
    {
        [Key]
        public int FamilyId { get; set; }
        public string FamilyName { get; set; } = string.Empty;
        public string FamilyDescription { get; set; } = string.Empty;
    }
}
