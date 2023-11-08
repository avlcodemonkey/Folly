using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Folly.Domain.Models;

[Table("RolePermission")]
public class RolePermission : AuditableEntity {
    public Permission Permission { get; set; } = null!;

    [Required]
    public int PermissionId { get; set; }

    public Role Role { get; set; } = null!;

    [Required]
    public int RoleId { get; set; }
}
