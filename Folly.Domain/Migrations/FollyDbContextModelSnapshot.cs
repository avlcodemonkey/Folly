﻿// <auto-generated />
using System;
using Folly.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Folly.Domain.Migrations
{
    [DbContext(typeof(FollyDbContext))]
    partial class FollyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.3");

            modelBuilder.Entity("Folly.Domain.Models.Language", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CountryCode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CreatedUserId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LanguageCode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("UpdatedUserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Language");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CountryCode = "us",
                            CreatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            IsDefault = true,
                            LanguageCode = "en",
                            Name = "English",
                            UpdatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890)
                        },
                        new
                        {
                            Id = 2,
                            CountryCode = "mx",
                            CreatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            IsDefault = false,
                            LanguageCode = "es",
                            Name = "Spanish",
                            UpdatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890)
                        });
                });

            modelBuilder.Entity("Folly.Domain.Models.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ActionName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("ControllerName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CreatedUserId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("UpdatedUserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Permission");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ActionName = "Index",
                            ControllerName = "Dashboard",
                            CreatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            UpdatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890)
                        },
                        new
                        {
                            Id = 2,
                            ActionName = "UpdateAccount",
                            ControllerName = "Account",
                            CreatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            UpdatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890)
                        },
                        new
                        {
                            Id = 3,
                            ActionName = "Logout",
                            ControllerName = "Account",
                            CreatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            UpdatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890)
                        },
                        new
                        {
                            Id = 4,
                            ActionName = "Dashboard",
                            ControllerName = "Profiler",
                            CreatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            UpdatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890)
                        },
                        new
                        {
                            Id = 5,
                            ActionName = "Index",
                            ControllerName = "Role",
                            CreatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            UpdatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890)
                        },
                        new
                        {
                            Id = 6,
                            ActionName = "Edit",
                            ControllerName = "Role",
                            CreatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            UpdatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890)
                        },
                        new
                        {
                            Id = 7,
                            ActionName = "Delete",
                            ControllerName = "Role",
                            CreatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            UpdatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890)
                        },
                        new
                        {
                            Id = 8,
                            ActionName = "Index",
                            ControllerName = "User",
                            CreatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            UpdatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890)
                        },
                        new
                        {
                            Id = 9,
                            ActionName = "Create",
                            ControllerName = "User",
                            CreatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            UpdatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890)
                        },
                        new
                        {
                            Id = 10,
                            ActionName = "Edit",
                            ControllerName = "User",
                            CreatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            UpdatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890)
                        },
                        new
                        {
                            Id = 11,
                            ActionName = "Delete",
                            ControllerName = "User",
                            CreatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            UpdatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890)
                        },
                        new
                        {
                            Id = 12,
                            ActionName = "RefreshPermissions",
                            ControllerName = "Role",
                            CreatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            UpdatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890)
                        });
                });

            modelBuilder.Entity("Folly.Domain.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CreatedUserId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("UpdatedUserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Role");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            IsDefault = true,
                            Name = "Administrator",
                            UpdatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890)
                        });
                });

            modelBuilder.Entity("Folly.Domain.Models.RolePermission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CreatedUserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PermissionId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoleId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("UpdatedUserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PermissionId");

                    b.HasIndex("RoleId");

                    b.ToTable("RolePermission");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            PermissionId = 1,
                            RoleId = 1,
                            UpdatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890)
                        },
                        new
                        {
                            Id = 2,
                            CreatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            PermissionId = 2,
                            RoleId = 1,
                            UpdatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890)
                        },
                        new
                        {
                            Id = 3,
                            CreatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            PermissionId = 3,
                            RoleId = 1,
                            UpdatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890)
                        },
                        new
                        {
                            Id = 4,
                            CreatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            PermissionId = 4,
                            RoleId = 1,
                            UpdatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890)
                        },
                        new
                        {
                            Id = 5,
                            CreatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            PermissionId = 5,
                            RoleId = 1,
                            UpdatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890)
                        },
                        new
                        {
                            Id = 6,
                            CreatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            PermissionId = 6,
                            RoleId = 1,
                            UpdatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890)
                        },
                        new
                        {
                            Id = 7,
                            CreatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            PermissionId = 7,
                            RoleId = 1,
                            UpdatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890)
                        },
                        new
                        {
                            Id = 8,
                            CreatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            PermissionId = 8,
                            RoleId = 1,
                            UpdatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890)
                        },
                        new
                        {
                            Id = 9,
                            CreatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            PermissionId = 9,
                            RoleId = 1,
                            UpdatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890)
                        },
                        new
                        {
                            Id = 10,
                            CreatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            PermissionId = 10,
                            RoleId = 1,
                            UpdatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890)
                        },
                        new
                        {
                            Id = 11,
                            CreatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            PermissionId = 11,
                            RoleId = 1,
                            UpdatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890)
                        },
                        new
                        {
                            Id = 12,
                            CreatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            PermissionId = 12,
                            RoleId = 1,
                            UpdatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890)
                        });
                });

            modelBuilder.Entity("Folly.Domain.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CreatedUserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int>("LanguageId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastName")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<bool?>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("UpdatedUserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            Email = "cpittman@gmail.com",
                            FirstName = "Chris",
                            LanguageId = 1,
                            LastName = "Pittman",
                            Status = true,
                            UpdatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            UserName = "auth0|5fea4d6e81637b00685cec34"
                        });
                });

            modelBuilder.Entity("Folly.Domain.Models.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CreatedUserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoleId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("UpdatedUserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRole");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            RoleId = 1,
                            UpdatedDate = new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890),
                            UserId = 1
                        });
                });

            modelBuilder.Entity("Folly.Domain.Models.RolePermission", b =>
                {
                    b.HasOne("Folly.Domain.Models.Permission", "Permission")
                        .WithMany("RolePermissions")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Folly.Domain.Models.Role", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Folly.Domain.Models.UserRole", b =>
                {
                    b.HasOne("Folly.Domain.Models.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Folly.Domain.Models.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Folly.Domain.Models.Permission", b =>
                {
                    b.Navigation("RolePermissions");
                });

            modelBuilder.Entity("Folly.Domain.Models.Role", b =>
                {
                    b.Navigation("RolePermissions");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Folly.Domain.Models.User", b =>
                {
                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
