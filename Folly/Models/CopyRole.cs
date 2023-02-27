using System.ComponentModel.DataAnnotations;
using Folly.Resources;

namespace Folly.Models;

public record CopyRole : BaseModel
{
    [Required(ErrorMessageResourceType = typeof(Roles), ErrorMessageResourceName = nameof(Roles.ErrorNameRequired))]
    [StringLength(100, ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorMaxLength))]
    public string Prompt { get; init; }
}
