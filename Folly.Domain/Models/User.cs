using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Folly.Domain.Models;

[Table("User")]
public class User : AuditableEntity {
    public User() => UserRoles = new HashSet<UserRole>();

    [StringLength(100)]
    [Required]
    public string Email { get; set; } = null!;

    [StringLength(100)]
    [Required]
    public string FirstName { get; set; } = null!;

    [Key]
    public int Id { get; set; }

    [Required]
    public int LanguageId { get; set; }

    [StringLength(100)]
    public string? LastName { get; set; }

    public bool? Status { get; set; }

    [StringLength(100)]
    [Required]
    public string UserName { get; set; } = null!;

    [ForeignKey(nameof(UserRole.UserId))]
    public IEnumerable<UserRole> UserRoles { get; set; }
}
