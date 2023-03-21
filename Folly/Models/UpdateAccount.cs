using System.ComponentModel.DataAnnotations;
using Folly.Resources;

namespace Folly.Models;

public record UpdateAccount : BaseModel
{
    public UpdateAccount() { }

    public UpdateAccount(User user)
    {
        Email = user.Email;
        FirstName = user.FirstName;
        LastName = user.LastName;
        LanguageId = user.LanguageId;
    }

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
    [Required(ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorRequired))]
    [StringLength(100, ErrorMessageResourceType = typeof(Core), ErrorMessageResourceName = nameof(Core.ErrorMaxLength))]
    public string LastName { get; init; } = "";
}
