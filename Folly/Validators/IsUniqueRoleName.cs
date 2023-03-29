using System.ComponentModel.DataAnnotations;
using Folly.Models;
using Folly.Resources;
using Folly.Services;

namespace Folly.Validators;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class IsUniqueRoleName : ValidationAttribute {
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext) {
        var service = validationContext.GetService(typeof(IRoleService)) as IRoleService;
        if (validationContext.ObjectInstance is Role role && service!.GetAllRoles().Result.Any(x => x.Name == role.Name && x.Id != role.Id))
            return new ValidationResult(Roles.ErrorDuplicateName, new[] { nameof(role.Name) });
        return ValidationResult.Success!;
    }
}
