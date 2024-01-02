using System.ComponentModel.DataAnnotations;
using Folly.Models;
using Folly.Resources;
using Folly.Services;

namespace Folly.Validators;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class IsUniqueUserName : ValidationAttribute {
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext) {
        var service = validationContext.GetService(typeof(IUserService)) as IUserService;

        if (validationContext.ObjectInstance is User user && service!.GetAllUsersAsync().Result.Any(x => x.UserName == user.UserName && x.Id != user.Id)) {
            return new ValidationResult(Users.ErrorDuplicateUserName, new[] { nameof(user.UserName) });
        }

        return ValidationResult.Success!;
    }
}
