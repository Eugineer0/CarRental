using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalDal.Migrations
{
    public partial class RenamedCarsColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarTypes_TypeId",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Cars",
                newName: "CarTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_TypeId",
                table: "Cars",
                newName: "IX_Cars_CarTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarTypes_CarTypeId",
                table: "Cars",
                column: "CarTypeId",
                principalTable: "CarTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarTypes_CarTypeId",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "CarTypeId",
                table: "Cars",
                newName: "TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_CarTypeId",
                table: "Cars",
                newName: "IX_Cars_TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarTypes_TypeId",
                table: "Cars",
                column: "TypeId",
                principalTable: "CarTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
