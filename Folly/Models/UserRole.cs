using System.ComponentModel.DataAnnotations;
using Folly.Resources;

namespace Folly.Models;

public record UserRole : BaseModel
{
    [Required(ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorRequired))]
    public int RoleId { get; init; }

    [Required(ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorRequired))]
    public int UserId { get; init; }
}
