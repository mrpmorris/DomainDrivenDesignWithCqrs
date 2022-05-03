using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomainDrivenDesignWithCqrs.AppLayer.Persistence.DbMigrations
{
    public partial class ColumnSizesAndChangeRowVersionToTimestamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RowVersion",
                table: "OrganisationType",
                newName: "Timestamp");

            migrationBuilder.RenameIndex(
                name: "UX_OrganisationType_Name",
                table: "OrganisationType",
                newName: "IX_OrganisationType_Name");

            migrationBuilder.RenameColumn(
                name: "RowVersion",
                table: "Organisation",
                newName: "Timestamp");

            migrationBuilder.RenameIndex(
                name: "UX_Organisation_Name",
                table: "Organisation",
                newName: "IX_Organisation_Name");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "OrganisationType",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Organisation",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "OrganisationType",
                newName: "RowVersion");

            migrationBuilder.RenameIndex(
                name: "IX_OrganisationType_Name",
                table: "OrganisationType",
                newName: "UX_OrganisationType_Name");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "Organisation",
                newName: "RowVersion");

            migrationBuilder.RenameIndex(
                name: "IX_Organisation_Name",
                table: "Organisation",
                newName: "UX_Organisation_Name");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "OrganisationType",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Organisation",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
