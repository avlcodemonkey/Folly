using System.Reflection;
using Folly.Domain.Attributes;
using Folly.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Folly.Domain.Extensions;

public static class ModelBuilderExtensions {
    /// <summary>
    /// Apply custom DefaultValue and DefaultValueSql attributes.
    /// </summary>
    public static ModelBuilder ApplyDefaults(this ModelBuilder modelBuilder) {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes()) {
            foreach (var property in entityType.GetProperties()) {
                var info = property.PropertyInfo ?? property.FieldInfo as MemberInfo;
                if (info == null) {
                    continue;
                }

                var attributes = Attribute.GetCustomAttributes(info);
                if (attributes?.Any() == true) {
                    if (attributes.FirstOrDefault(x => x is DefaultValueSqlAttribute) is DefaultValueSqlAttribute defaultValueSqlAttr) {
                        property.SetDefaultValueSql(defaultValueSqlAttr.Sql);
                    }
                    if (attributes.FirstOrDefault(x => x is DefaultValueAttribute) is DefaultValueAttribute defaultValueAttr) {
                        property.SetDefaultValue(defaultValueAttr.DefaultValue);
                    }
                }
            }
        }

        return modelBuilder;
    }


    /// <summary>
    /// Seed data to get a clean db up and running.
    /// </summary>
    public static ModelBuilder Seed(this ModelBuilder modelBuilder) {
        var minDate = DateTime.MinValue;

        modelBuilder.Entity<Language>().HasData(
            new Language { Id = 1, Name = "English", LanguageCode = "en", CountryCode = "us", IsDefault = true, CreatedDate = minDate, UpdatedDate = minDate },
            new Language { Id = 2, Name = "Spanish", LanguageCode = "es", CountryCode = "mx", IsDefault = false, CreatedDate = minDate, UpdatedDate = minDate }
        );

        modelBuilder.Entity<Permission>().HasData(
            new Permission { Id = 1, ControllerName = "Dashboard", ActionName = "Index", CreatedDate = minDate, UpdatedDate = minDate },
            new Permission { Id = 2, ControllerName = "Account", ActionName = "UpdateAccount", CreatedDate = minDate, UpdatedDate = minDate },
            new Permission { Id = 3, ControllerName = "Account", ActionName = "Logout", CreatedDate = minDate, UpdatedDate = minDate },
            new Permission { Id = 4, ControllerName = "Role", ActionName = "Index", CreatedDate = minDate, UpdatedDate = minDate },
            new Permission { Id = 5, ControllerName = "Role", ActionName = "Edit", CreatedDate = minDate, UpdatedDate = minDate },
            new Permission { Id = 6, ControllerName = "Role", ActionName = "Delete", CreatedDate = minDate, UpdatedDate = minDate },
            new Permission { Id = 7, ControllerName = "User", ActionName = "Index", CreatedDate = minDate, UpdatedDate = minDate },
            new Permission { Id = 8, ControllerName = "User", ActionName = "Create", CreatedDate = minDate, UpdatedDate = minDate },
            new Permission { Id = 9, ControllerName = "User", ActionName = "Edit", CreatedDate = minDate, UpdatedDate = minDate },
            new Permission { Id = 10, ControllerName = "User", ActionName = "Delete", CreatedDate = minDate, UpdatedDate = minDate },
            new Permission { Id = 11, ControllerName = "Role", ActionName = "RefreshPermissions", CreatedDate = minDate, UpdatedDate = minDate }
        );

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Administrator", IsDefault = true, CreatedDate = minDate, UpdatedDate = minDate }
        );
        modelBuilder.Entity<RolePermission>().HasData(
            new RolePermission { Id = 1, PermissionId = 1, RoleId = 1, CreatedDate = minDate, UpdatedDate = minDate },
            new RolePermission { Id = 2, PermissionId = 2, RoleId = 1, CreatedDate = minDate, UpdatedDate = minDate },
            new RolePermission { Id = 3, PermissionId = 3, RoleId = 1, CreatedDate = minDate, UpdatedDate = minDate },
            new RolePermission { Id = 4, PermissionId = 4, RoleId = 1, CreatedDate = minDate, UpdatedDate = minDate },
            new RolePermission { Id = 5, PermissionId = 5, RoleId = 1, CreatedDate = minDate, UpdatedDate = minDate },
            new RolePermission { Id = 6, PermissionId = 6, RoleId = 1, CreatedDate = minDate, UpdatedDate = minDate },
            new RolePermission { Id = 7, PermissionId = 7, RoleId = 1, CreatedDate = minDate, UpdatedDate = minDate },
            new RolePermission { Id = 8, PermissionId = 8, RoleId = 1, CreatedDate = minDate, UpdatedDate = minDate },
            new RolePermission { Id = 9, PermissionId = 9, RoleId = 1, CreatedDate = minDate, UpdatedDate = minDate },
            new RolePermission { Id = 10, PermissionId = 10, RoleId = 1, CreatedDate = minDate, UpdatedDate = minDate },
            new RolePermission { Id = 11, PermissionId = 11, RoleId = 1, CreatedDate = minDate, UpdatedDate = minDate }
        );

        // hardcode to my user for now.  @todo will need a different solution eventually
        modelBuilder.Entity<User>().HasData(
            new User {
                Id = 1, UserName = "auth0|5fea4d6e81637b00685cec34", FirstName = "Chris", LastName = "Pittman",
                LanguageId = 1, Email = "cpittman@gmail.com", Status = true, CreatedDate = minDate, UpdatedDate = minDate
            }
        );
        modelBuilder.Entity<UserRole>().HasData(
            new UserRole { Id = 1, RoleId = 1, UserId = 1, CreatedDate = minDate, UpdatedDate = minDate }
        );

        return modelBuilder;
    }
}
