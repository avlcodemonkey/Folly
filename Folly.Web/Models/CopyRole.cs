using System.ComponentModel.DataAnnotations;
using Folly.Resources;

namespace Folly.Models;

public sealed record CopyRole : BaseModel {
    [Display(ResourceType = typeof(Roles), Name = nameof(Roles.Name))]
    [Required(ErrorMessageResourceType = typeof(Roles), ErrorMessageResourceName = nameof(Roles.ErrorNameRequired))]
    [StringLength(100, ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorMaxLength))]
    public string? Prompt { get; init; }
}
