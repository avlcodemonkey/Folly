using System.ComponentModel.DataAnnotations;
using Folly.Resources;
using Folly.Validators;

namespace Folly.Models;

public sealed record User : VersionedModel {
    [Display(ResourceType = typeof(Users), Name = nameof(Users.Email))]
    [Required(ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorRequired))]
    [StringLength(100, ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorMaxLength))]
    [EmailAddress(ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorEmailAddress))]
    [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorEmailAddressFormat))]
    public string Email { get; init; } = "";

    [Display(ResourceType = typeof(Users), Name = nameof(Users.FirstName))]
    [Required(ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorRequired))]
    [StringLength(100, ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorMaxLength))]
    public string FirstName { get; init; } = "";

    [Display(ResourceType = typeof(Users), Name = nameof(Users.Language))]
    [Required(ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorRequired))]
    public int LanguageId { get; init; }

    [Display(ResourceType = typeof(Users), Name = nameof(Users.LastName))]
    [StringLength(100, ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorMaxLength))]
    public string? LastName { get; init; }

    [Display(ResourceType = typeof(Users), Name = nameof(Users.Roles))]
    public IEnumerable<int>? RoleIds { get; init; }

    [Display(ResourceType = typeof(Users), Name = nameof(Users.UserName))]
    [Required(ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorRequired))]
    [StringLength(100, ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorMaxLength))]
    [IsUniqueUserName]
    public string UserName { get; init; } = "";
}
