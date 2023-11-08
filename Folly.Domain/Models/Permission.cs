using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Folly.Domain.Models;

[Table("Permission")]
public class Permission : AuditableEntity {
    public Permission() => RolePermissions = new List<RolePermission>();

    [StringLength(100)]
    [Required]
    public string ActionName { get; set; } = null!;

    [StringLength(100)]
    [Required]
    public string ControllerName { get; set; } = null!;

    [ForeignKey(nameof(RolePermission.PermissionId))]
    public IEnumerable<RolePermission> RolePermissions { get; set; }
}
