using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mealify.Migrations
{
    public partial class CompanyModelsFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Companies_ParentCompany",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "ParentCompany",
                table: "Companies",
                newName: "ParentCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Companies_ParentCompany",
                table: "Companies",
                newName: "IX_Companies_ParentCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Companies_ParentCompanyId",
                table: "Companies",
                column: "ParentCompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Companies_ParentCompanyId",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "ParentCompanyId",
                table: "Companies",
                newName: "ParentCompany");

            migrationBuilder.RenameIndex(
                name: "IX_Companies_ParentCompanyId",
                table: "Companies",
                newName: "IX_Companies_ParentCompany");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Companies_ParentCompany",
                table: "Companies",
                column: "ParentCompany",
                principalTable: "Companies",
                principalColumn: "Id");
        }
    }
}
