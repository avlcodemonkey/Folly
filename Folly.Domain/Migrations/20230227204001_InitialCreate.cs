using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Folly.Domain.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CountryCode = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    IsDefault = table.Column<bool>(type: "INTEGER", nullable: false),
                    LanguageCode = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedUserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ActionName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ControllerName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedUserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsDefault = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedUserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LanguageId = table.Column<int>(type: "INTEGER", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Status = table.Column<bool>(type: "INTEGER", nullable: true),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedUserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PermissionId = table.Column<int>(type: "INTEGER", nullable: false),
                    RoleId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedUserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedUserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Language",
                columns: new[] { "Id", "CountryCode", "CreatedDate", "CreatedUserId", "IsDefault", "LanguageCode", "Name", "UpdatedDate", "UpdatedUserId" },
                values: new object[,]
                {
                    { 1, "us", new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, true, "en", "English", new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null },
                    { 2, "mx", new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, false, "es", "Spanish", new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null }
                });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "ActionName", "ControllerName", "CreatedDate", "CreatedUserId", "UpdatedDate", "UpdatedUserId" },
                values: new object[,]
                {
                    { 1, "Index", "Dashboard", new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null },
                    { 2, "UpdateAccount", "Account", new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null },
                    { 3, "Logout", "Account", new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null },
                    { 4, "Dashboard", "Profiler", new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null },
                    { 5, "Index", "Role", new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null },
                    { 6, "Edit", "Role", new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null },
                    { 7, "Delete", "Role", new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null },
                    { 8, "Index", "User", new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null },
                    { 9, "Create", "User", new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null },
                    { 10, "Edit", "User", new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null },
                    { 11, "Delete", "User", new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null },
                    { 12, "RefreshPermissions", "Role", new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedDate", "CreatedUserId", "IsDefault", "Name", "UpdatedDate", "UpdatedUserId" },
                values: new object[] { 1, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, true, "Administrator", new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CreatedDate", "CreatedUserId", "Email", "FirstName", "LanguageId", "LastName", "Status", "UpdatedDate", "UpdatedUserId", "UserName" },
                values: new object[] { 1, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, "cpittman@gmail.com", "Chris", 1, "Pittman", true, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, "auth0|5fea4d6e81637b00685cec34" });

            migrationBuilder.InsertData(
                table: "RolePermission",
                columns: new[] { "Id", "CreatedDate", "CreatedUserId", "PermissionId", "RoleId", "UpdatedDate", "UpdatedUserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, 1, 1, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null },
                    { 2, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, 2, 1, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null },
                    { 3, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, 3, 1, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null },
                    { 4, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, 4, 1, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null },
                    { 5, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, 5, 1, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null },
                    { 6, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, 6, 1, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null },
                    { 7, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, 7, 1, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null },
                    { 8, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, 8, 1, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null },
                    { 9, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, 9, 1, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null },
                    { 10, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, 10, 1, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null },
                    { 11, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, 11, 1, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null },
                    { 12, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, 12, 1, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null }
                });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "Id", "CreatedDate", "CreatedUserId", "RoleId", "UpdatedDate", "UpdatedUserId", "UserId" },
                values: new object[] { 1, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, 1, new DateTime(2023, 2, 27, 20, 40, 1, 595, DateTimeKind.Utc).AddTicks(8890), null, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionId",
                table: "RolePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_RoleId",
                table: "RolePermission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropTable(
                name: "RolePermission");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
