using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    LastName = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DIsh",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DIsh", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Tables",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tables", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Worker",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    LastName = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<int>(type: "int", nullable: true),
                    CityName = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    StreetName = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Worker", x => x.id);
                    table.ForeignKey(
                        name: "FK_Worker_Client",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DishCategory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    DishId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishCategory", x => x.id);
                    table.ForeignKey(
                        name: "FK_DishCategory_Category",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DishCategory_DIsh",
                        column: x => x.DishId,
                        principalTable: "DIsh",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    TableId = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    Sum = table.Column<decimal>(type: "money", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.id);
                    table.ForeignKey(
                        name: "FK_Order_Client",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Order_Tables",
                        column: x => x.TableId,
                        principalTable: "Tables",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "DishOrder",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    DishId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishOrder", x => x.id);
                    table.ForeignKey(
                        name: "FK_DishOrder_DIsh",
                        column: x => x.DishId,
                        principalTable: "DIsh",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DishOrder_Order",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DishCategory_CategoryId",
                table: "DishCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DishCategory_DishId",
                table: "DishCategory",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_DishOrder_DishId",
                table: "DishOrder",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_DishOrder_OrderId",
                table: "DishOrder",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ClientId",
                table: "Order",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_TableId",
                table: "Order",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_Worker_ClientId",
                table: "Worker",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DishCategory");

            migrationBuilder.DropTable(
                name: "DishOrder");

            migrationBuilder.DropTable(
                name: "Worker");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "DIsh");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Tables");
        }
    }
}
