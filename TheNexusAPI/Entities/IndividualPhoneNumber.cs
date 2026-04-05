using System.ComponentModel.DataAnnotations;

namespace TheNexusAPI.Entities
{
    public class IndividualPhoneNumber
    {
        [Key]
        public int IndividualPhoneNumberId { get; set; }
        public int IndividualId { get; set; }
        public int PhoneNumberId { get; set; }
    }
}
