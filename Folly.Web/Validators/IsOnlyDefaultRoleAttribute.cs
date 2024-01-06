using System.ComponentModel.DataAnnotations;
using Folly.Models;
using Folly.Resources;
using Folly.Services;

namespace Folly.Validators;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class IsOnlyDefaultRoleAttribute : ValidationAttribute {
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
        var service = validationContext.GetService(typeof(IRoleService)) as IRoleService;

        if (validationContext.ObjectInstance is Role role && role.IsDefault && service!.GetAllRolesAsync().Result.Any(x => x.IsDefault && x.Id != role.Id)) {
            return new ValidationResult(Roles.ErrorDuplicateDefault, new[] { nameof(role.IsDefault) });
        }

        return ValidationResult.Success;
    }
}
