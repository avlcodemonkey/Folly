using System.ComponentModel.DataAnnotations;
using Folly.Resources;

namespace Folly.Models;

public sealed record RolePermission : IAuditedModel, IEquatable<RolePermission> {
    [Required(ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorRequired))]
    public int Id { get; init; }

    [Required(ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorRequired))]
    public int PermissionId { get; init; }

    [Required(ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorRequired))]
    public int RoleId { get; init; }
}
