using System.ComponentModel.DataAnnotations;

namespace TheNexusAPI.Entities.Dto
{
    public class PhoneNumbersWithAssignedIndividualDto
    {
        [Key]
        public int PhoneNumberId { get; set; }
        public string PhoneNumberValue { get; set; } = string.Empty;
        public int PhoneNumberTypeId { get; set; }
        public bool IsAssigned { get; set; } = false;
    }
}
