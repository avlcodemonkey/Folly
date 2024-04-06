using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Folly.Domain.Migrations {
    /// <inheritdoc />
    public partial class AddRowVersion : Migration {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.AddColumn<int>(
                name: "RowVersion",
                table: "User",
                type: "INTEGER",
                rowVersion: true,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RowVersion",
                table: "Role",
                type: "INTEGER",
                rowVersion: true,
                nullable: false,
                defaultValue: 0);

            // sqlite specific, may need to change if backing database is changed
            migrationBuilder.Sql(@"
CREATE TRIGGER UserUpdate
AFTER UPDATE ON User
BEGIN
    UPDATE User
    SET RowVersion = RowVersion + 1
    WHERE rowid = NEW.rowid;
END;
            ");

            migrationBuilder.Sql(@"
CREATE TRIGGER RoleUpdate
AFTER UPDATE ON Role
BEGIN
    UPDATE Role
    SET RowVersion = RowVersion + 1
    WHERE rowid = NEW.rowid;
END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "User");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Role");

            migrationBuilder.Sql("DROP TRIGGER IF EXISTS UserUpdate;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS RoleUpdate;");
        }
    }
}
