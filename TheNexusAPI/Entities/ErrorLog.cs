using System.ComponentModel.DataAnnotations;

namespace TheNexusAPI.Entities
{
    public class ErrorLog
    {
        [Key]
        public int ErrorLogId { get; set; }
        public string Source { get; set; } = string.Empty;
        public string TargetSite { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime ErrorDateTime { get; set; }
        public int UserId { get; set; }

    }
}
