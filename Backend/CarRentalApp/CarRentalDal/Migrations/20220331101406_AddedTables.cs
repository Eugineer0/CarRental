using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalDal.Migrations
{
    public partial class AddedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarServices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(64)", nullable: false),
                    SeatPlaces = table.Column<byte>(type: "tinyint", nullable: false),
                    AverageConsumption = table.Column<double>(type: "float", nullable: false),
                    GearboxType = table.Column<byte>(type: "tinyint", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    Length = table.Column<int>(type: "int", nullable: false),
                    Power = table.Column<int>(type: "int", nullable: false),
                    PricePerMinute = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PricePerHour = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PricePerDay = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RentalCenters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(64)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(64)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(128)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalCenters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarServicePrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CarServiceId = table.Column<int>(type: "int", nullable: false),
                    CarTypeId = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nchar(7)", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    RentalCenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cars_CarTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "CarTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cars_RentalCenters_RentalCenterId",
                        column: x => x.RentalCenterId,
                        principalTable: "RentalCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OverallPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartRent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinishRent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CarId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Users_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderCarServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarServiceId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderCarServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderCarServices_CarServices_CarServiceId",
                        column: x => x.CarServiceId,
                        principalTable: "CarServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderCarServices_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_RentalCenterId",
                table: "Cars",
                column: "RentalCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_TypeId",
                table: "Cars",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CarServicePrices_CarServiceId",
                table: "CarServicePrices",
                column: "CarServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_CarServicePrices_CarTypeId",
                table: "CarServicePrices",
                column: "CarTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderCarServices_CarServiceId",
                table: "OrderCarServices",
                column: "CarServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderCarServices_OrderId",
                table: "OrderCarServices",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CarId",
                table: "Orders",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ClientId",
                table: "Orders",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarServicePrices");

            migrationBuilder.DropTable(
                name: "OrderCarServices");

            migrationBuilder.DropTable(
                name: "CarServices");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "CarTypes");

            migrationBuilder.DropTable(
                name: "RentalCenters");
        }
    }
}