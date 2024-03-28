using System.ComponentModel.DataAnnotations;
using Folly.Resources;
using Folly.Validators;

namespace Folly.Models;

public sealed record Role : VersionedModel {
    [Display(ResourceType = typeof(Roles), Name = nameof(Roles.Name))]
    [Required(ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorRequired))]
    [StringLength(100, ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorMaxLength))]
    [IsUniqueRoleName]
    public string Name { get; init; } = "";

    [Display(ResourceType = typeof(Roles), Name = nameof(Roles.IsDefault))]
    [IsOnlyDefaultRole]
    public bool IsDefault { get; init; } = false;

    [Display(ResourceType = typeof(Roles), Name = nameof(Roles.Permissions))]
    public IEnumerable<int>? PermissionIds { get; init; }
}
