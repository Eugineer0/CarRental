using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalDal.Migrations
{
    public partial class RefactoredDomainTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "OrderCarServices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrderCarServices");
        }
    }
}
