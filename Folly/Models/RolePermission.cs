using System;
using System.ComponentModel.DataAnnotations;
using Folly.Resources;

namespace Folly.Models;

public record RolePermission : BaseModel, IEquatable<RolePermission>
{
    [Required(ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorRequired))]
    public int PermissionId { get; init; }

    [Required(ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorRequired))]
    public int RoleId { get; init; }
}
