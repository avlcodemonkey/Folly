using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Folly.Domain.Attributes;

namespace Folly.Domain.Models;

[Table("Role")]
public class Role : IAuditedEntity, IVersionedEntity {
    [Key]
    public int Id { get; set; }

    public bool IsDefault { get; set; }

    [StringLength(100)]
    [Required]
    public string Name { get; set; } = null!;

    [ForeignKey(nameof(RolePermission.RoleId))]
    public List<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    [ForeignKey(nameof(UserRole.RoleId))]
    public List<UserRole> UserRoles { get; set; } = new List<UserRole>();

    // sqlite specific, will need to change if backing database is changed
    [DefaultValueSql("(current_timestamp)")]
    public DateTime CreatedDate { get; set; }

    // sqlite specific, will need to change if backing database is changed
    [DefaultValueSql("(current_timestamp)")]
    public DateTime UpdatedDate { get; set; }

    [NotMapped]
    public int TemporaryId { get; set; }

    [Timestamp]
    public int RowVersion { get; set; }
}
