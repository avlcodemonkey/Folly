using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Folly.Models;
using Folly.Resources;
using Folly.Services;

namespace Folly.Validators;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class IsOnlyDefaultRole : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var service = (IRoleService) validationContext.GetService(typeof(IRoleService));
        if (validationContext.ObjectInstance is Role role && role.IsDefault)
            if (service.GetAllRoles().Result.Any(x => x.IsDefault && x.Id != role.Id))
                return new ValidationResult(Roles.ErrorDuplicateDefault, new[] { nameof(role.IsDefault) });
        return null;
    }
}
