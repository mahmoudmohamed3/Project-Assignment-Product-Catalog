namespace Product_Catalog.Core.Models
{
    public class AuditLog
    {
        public int AuditLogId { get; set; }

        [Required]
        [MaxLength(100)]
        public string EntityName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string EntityId { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Action { get; set; } = string.Empty;

        [MaxLength(100)]
        public string ChangedBy { get; set; } = string.Empty;

        public DateTime ChangedAt { get; set; } 

        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
    }
    
}
