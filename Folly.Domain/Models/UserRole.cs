using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Folly.Domain.Models;

[Table("UserRole")]
public class UserRole : AuditableEntity {
    public Role Role { get; set; } = null!;

    [Required]
    public int RoleId { get; set; }

    public User User { get; set; } = null!;

    [Required]
    public int UserId { get; set; }
}
