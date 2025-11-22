using System.ComponentModel.DataAnnotations;

namespace TheNexusAPI.Entities
{
    public class ChangeLog
    {
        [Key]
        public int ChangeLogId { get; set; }
        public string ChangedTable { get; set; } = string.Empty;
        public int ChangedTablePrimaryId { get; set; }
        public string OldValue { get; set; } = string.Empty;
        public string NewValue { get; set; } = string.Empty;
        public DateTime ChangeDateTime { get; set; }
        public int ChangeUserId { get; set; }

    }
}
