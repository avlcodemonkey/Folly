using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Folly.Domain.Attributes;

namespace Folly.Domain.Models;

[Table("UserRole")]
public class UserRole : IAuditedEntity {
    [Key]
    public int Id { get; set; }

    public Role Role { get; set; } = null!;

    [Required]
    public int RoleId { get; set; }

    public User User { get; set; } = null!;

    [Required]
    public int UserId { get; set; }

    // sqlite specific, will need to change if backing database is changed
    [DefaultValueSql("(current_timestamp)")]
    public DateTime CreatedDate { get; set; }

    // sqlite specific, will need to change if backing database is changed
    [DefaultValueSql("(current_timestamp)")]
    public DateTime UpdatedDate { get; set; }

    [NotMapped]
    public int TemporaryId { get; set; }
}
