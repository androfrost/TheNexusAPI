using System.ComponentModel.DataAnnotations;
using TheNexusAPI.Entities.Dto;

namespace TheNexusAPI.Entities
{
    public class IndividualDto
    {
        [Key]
        public int IndividualId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int FamilyId { get; set; } = 0;
        public IndividualLocationDto individualLocationDto { get; set; } = new IndividualLocationDto();
        public int PhoneNumberId { get; set; } = 0;
        public string IndividualDescription { get; set; } = string.Empty;
        public int IndividualTypeId { get; set; } = 0;
        public int SexId { get; set; } = 0;
        public DateTime DateOfBirth { get; set; } = new DateTime();
        public int StatusId { get; set; } = 0;
    }
}
