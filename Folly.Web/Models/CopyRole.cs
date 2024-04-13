using System.ComponentModel.DataAnnotations;
using Folly.Resources;

namespace Folly.Models;

public sealed record CopyRole : IAuditedModel {
    [Required(ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorRequired))]
    public int Id { get; init; }

    [Display(ResourceType = typeof(Roles), Name = nameof(Roles.NewName))]
    [Required(ErrorMessageResourceType = typeof(Roles), ErrorMessageResourceName = nameof(Roles.ErrorNameRequired))]
    [StringLength(100, ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorMaxLength))]
    public string Prompt { get; init; } = "";
}
