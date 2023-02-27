using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Folly.Domain.Models
{
    [Table("RolePermission")]
    public class RolePermission : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public Permission Permission { get; set; } = null!;

        [Required]
        public int PermissionId { get; set; }

        public Role Role { get; set; } = null!;

        [Required]
        public int RoleId { get; set; }
    }
}
