using System.ComponentModel.DataAnnotations;

namespace TheNexusAPI.Entities
{
    public class Individual
    {
        [Key]
        public int IndividualId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int FamilyId { get; set; } = 0;
        public int LocationId { get; set; } = 0;
        public int PhoneNumberId { get; set; } = 0;
        public string Description { get; set; } = string.Empty;
        public int IndividualTypeId { get; set; } = 0;
        public int SexId { get; set; } = 0;
        public DateTime DateOfBirth { get; set; } = new DateTime();
        public int StatusId { get; set; } = 0;
    }
}
