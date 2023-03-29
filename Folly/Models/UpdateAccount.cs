using System.ComponentModel.DataAnnotations;
using Folly.Resources;

namespace Folly.Models;

public sealed record UpdateAccount : BaseModel {
    public UpdateAccount() { }

    public UpdateAccount(User user) {
        Email = user.Email;
        FirstName = user.FirstName;
        LastName = user.LastName ?? string.Empty;
        LanguageId = user.LanguageId;
    }

    [Display(ResourceType = typeof(Users), Name = nameof(Users.Email))]
    [Required(ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorRequired))]
    [StringLength(100, ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorMaxLength))]
    [EmailAddress(ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorEmailAddress))]
    [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorEmailAddressFormat))]
    public string Email { get; init; } = string.Empty;

    [Display(ResourceType = typeof(Users), Name = nameof(Users.FirstName))]
    [Required(ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorRequired))]
    [StringLength(100, ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorMaxLength))]
    public string FirstName { get; init; } = string.Empty;

    [Display(ResourceType = typeof(Users), Name = nameof(Users.Language))]
    [Required(ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorRequired))]
    public int LanguageId { get; init; }

    [Display(ResourceType = typeof(Users), Name = nameof(Users.LastName))]
    [Required(ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorRequired))]
    [StringLength(100, ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorMaxLength))]
    public string LastName { get; init; } = string.Empty;
}
