using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Folly.Domain.Models;

[Table("Role")]
public class Role : AuditableEntity {
    public Role() {
        RolePermissions = new List<RolePermission>();
        UserRoles = new List<UserRole>();
    }

    public bool IsDefault { get; set; }

    [StringLength(100)]
    [Required]
    public string Name { get; set; } = null!;

    [ForeignKey(nameof(RolePermission.RoleId))]
    public List<RolePermission> RolePermissions { get; set; }

    [ForeignKey(nameof(UserRole.RoleId))]
    public List<UserRole> UserRoles { get; set; }
}
