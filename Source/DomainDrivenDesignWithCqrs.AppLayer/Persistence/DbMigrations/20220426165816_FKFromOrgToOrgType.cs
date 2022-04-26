using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomainDrivenDesignWithCqrs.AppLayer.Persistence.DbMigrations
{
    public partial class FKFromOrgToOrgType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Organisation_TypeId",
                table: "Organisation",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organisation_OrganisationType_TypeId",
                table: "Organisation",
                column: "TypeId",
                principalTable: "OrganisationType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organisation_OrganisationType_TypeId",
                table: "Organisation");

            migrationBuilder.DropIndex(
                name: "IX_Organisation_TypeId",
                table: "Organisation");
        }
    }
}
