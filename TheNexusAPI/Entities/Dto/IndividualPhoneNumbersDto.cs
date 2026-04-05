using System.ComponentModel.DataAnnotations;

namespace TheNexusAPI.Entities.Dto
{
    public class IndividualPhoneNumbersDto
    {
        [Key]
        public int IndividualPhoneNumberId { get; set; }
        public int IndividualId { get; set; }
        public int PhoneNumberId { get; set; }
        public List<PhoneNumber> PhoneNumber { get; set; } = [];

    }
}
