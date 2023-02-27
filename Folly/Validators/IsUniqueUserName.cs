using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Folly.Models;
using Folly.Resources;
using Folly.Services;

namespace Folly.Validators;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class IsUniqueUserName : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var service = (IUserService) validationContext.GetService(typeof(IUserService));
        if (validationContext.ObjectInstance is User user)
            if (service.GetAllUsers().Result.Any(x => x.UserName == user.UserName && x.Id != user.Id))
                return new ValidationResult(Users.ErrorDuplicateUserName, new[] { nameof(user.UserName) });
        return null;
    }
}
