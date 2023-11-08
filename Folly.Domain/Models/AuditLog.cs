using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Folly.Domain.Models;

[Table("AuditLog")]
public class AuditLog {
    [Key]
    public long Id { get; set; }

    [Required]
    public Guid BatchId { get; set; }

    [Required]
    public string Entity { get; set; } = null!;

    [Required]
    public long PrimaryKey { get; set; }

    [Required]
    public EntityState State { get; set; }

    [Required]
    public DateTime Date { get; set; }

    public long? UserId { get; set; }

    public string? OldValues { get; set; }

    public string? NewValues { get; set; }
}
