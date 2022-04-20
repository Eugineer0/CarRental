using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalDal.Migrations
{
    public partial class ChangedPricesRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarTypes_TypeId",
                table: "Cars");

            migrationBuilder.DropTable(
                name: "CarServicePrices");

            migrationBuilder.DropColumn(
                name: "PricePerDay",
                table: "CarTypes");

            migrationBuilder.DropColumn(
                name: "PricePerHour",
                table: "CarTypes");

            migrationBuilder.DropColumn(
                name: "PricePerMinute",
                table: "CarTypes");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Cars",
                newName: "CarTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_TypeId",
                table: "Cars",
                newName: "IX_Cars_CarTypeId");

            migrationBuilder.CreateTable(
                name: "CarTypeCarServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarTypeId = table.Column<int>(type: "int", nullable: false),
                    CarServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarTypeCarServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarTypeCarServices_CarServices_CarServiceId",
                        column: x => x.CarServiceId,
                        principalTable: "CarServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarTypeCarServices_CarTypes_CarTypeId",
                        column: x => x.CarTypeId,
                        principalTable: "CarTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarTypesPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarTypeId = table.Column<int>(type: "int", nullable: false),
                    RentalCenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PricePerMinute = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PricePerHour = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PricePerDay = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarTypesPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarTypesPrices_CarTypes_CarTypeId",
                        column: x => x.CarTypeId,
                        principalTable: "CarTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarTypesPrices_RentalCenters_RentalCenterId",
                        column: x => x.RentalCenterId,
                        principalTable: "RentalCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarTypeCarServicePrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RentalCenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CarTypeCarServiceId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarTypeCarServicePrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarTypeCarServicePrices_CarTypeCarServices_CarTypeCarServiceId",
                        column: x => x.CarTypeCarServiceId,
                        principalTable: "CarTypeCarServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarTypeCarServicePrices_RentalCenters_RentalCenterId",
                        column: x => x.RentalCenterId,
                        principalTable: "RentalCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarTypeCarServicePrices_CarTypeCarServiceId",
                table: "CarTypeCarServicePrices",
                column: "CarTypeCarServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_CarTypeCarServicePrices_RentalCenterId",
                table: "CarTypeCarServicePrices",
                column: "RentalCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_CarTypeCarServices_CarServiceId",
                table: "CarTypeCarServices",
                column: "CarServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_CarTypeCarServices_CarTypeId",
                table: "CarTypeCarServices",
                column: "CarTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CarTypesPrices_CarTypeId",
                table: "CarTypesPrices",
                column: "CarTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CarTypesPrices_RentalCenterId",
                table: "CarTypesPrices",
                column: "RentalCenterId");

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

            migrationBuilder.DropTable(
                name: "CarTypeCarServicePrices");

            migrationBuilder.DropTable(
                name: "CarTypesPrices");

            migrationBuilder.DropTable(
                name: "CarTypeCarServices");

            migrationBuilder.RenameColumn(
                name: "CarTypeId",
                table: "Cars",
                newName: "TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_CarTypeId",
                table: "Cars",
                newName: "IX_Cars_TypeId");

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerDay",
                table: "CarTypes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerHour",
                table: "CarTypes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerMinute",
                table: "CarTypes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "CarServicePrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarServiceId = table.Column<int>(type: "int", nullable: false),
                    CarTypeId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarServicePrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarServicePrices_CarServices_CarServiceId",
                        column: x => x.CarServiceId,
                        principalTable: "CarServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarServicePrices_CarTypes_CarTypeId",
                        column: x => x.CarTypeId,
                        principalTable: "CarTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarServicePrices_CarServiceId",
                table: "CarServicePrices",
                column: "CarServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_CarServicePrices_CarTypeId",
                table: "CarServicePrices",
                column: "CarTypeId");

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
