using System.ComponentModel.DataAnnotations;

namespace TheNexusAPI.Entities
{
    public class PhoneNumber
    {
        [Key]
        public int PhoneNumberId { get; set; }
        public string PhoneNumberValue { get; set; } = string.Empty;
        public int PhoneNumberTypeId { get; set; }
    }
}
