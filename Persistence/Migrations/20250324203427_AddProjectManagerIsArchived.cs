using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectManagerIsArchived : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isArchived",
                table: "Programmers",
                newName: "IsArchived");

            migrationBuilder.AddColumn<bool>(
                name: "isArchived",
                table: "ProjectManagers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isArchived",
                table: "ProjectManagers");

            migrationBuilder.RenameColumn(
                name: "IsArchived",
                table: "Programmers",
                newName: "isArchived");
        }
    }
}
