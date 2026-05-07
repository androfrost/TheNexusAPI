using System.ComponentModel.DataAnnotations;

namespace TheNexusAPI.Entities
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public int GroupTypeId { get; set; } = 0;
        public string GroupDescription { get; set; } = string.Empty;
    }
}
