using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WEBAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialPrdductApiTaskDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TotalRecords = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sku = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TotalRecords = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreateBy", "CreateDate", "LastModifiedBy", "LastModifiedDate", "Name", "Title", "TotalRecords" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Active Wear - Men", null, 0 },
                    { 2, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Active Wear - Women", null, 0 },
                    { 3, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Mineral Water", null, 0 },
                    { 4, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Publications", null, 0 },
                    { 5, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Supplements", null, 0 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreateBy", "CreateDate", "Description", "IsAvailable", "LastModifiedBy", "LastModifiedDate", "Name", "Price", "Sku", "Title", "TotalRecords" },
                values: new object[,]
                {
                    { 1, 1, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7596), null, true, null, null, "Grunge Skater Jeans", 68m, "AWMGSJ", null, 0 },
                    { 2, 1, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7605), null, true, null, null, "Polo Shirt", 35m, "AWMPS", null, 0 },
                    { 3, 1, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7608), null, true, null, null, "Skater Graphic T-Shirt", 33m, "AWMSGT", null, 0 },
                    { 4, 1, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7610), null, true, null, null, "Slicker Jacket", 125m, "AWMSJ", null, 0 },
                    { 5, 1, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7612), null, true, null, null, "Thermal Fleece Jacket", 60m, "AWMTFJ", null, 0 },
                    { 6, 1, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7616), null, true, null, null, "Unisex Thermal Vest", 95m, "AWMUTV", null, 0 },
                    { 7, 1, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7619), null, true, null, null, "V-Neck Pullover", 65m, "AWMVNP", null, 0 },
                    { 8, 1, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7622), null, true, null, null, "V-Neck Sweater", 65m, "AWMVNS", null, 0 },
                    { 9, 1, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7624), null, true, null, null, "V-Neck T-Shirt", 17m, "AWMVNT", null, 0 },
                    { 10, 2, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7627), null, true, null, null, "Bamboo Thermal Ski Coat", 99m, "AWWBTSC", null, 0 },
                    { 11, 2, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, "Cross-Back Training Tank", 0m, "AWWCTT", null, 0 },
                    { 12, 2, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7632), null, true, null, null, "Grunge Skater Jeans", 68m, "AWWGSJ", null, 0 },
                    { 13, 2, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7634), null, true, null, null, "Slicker Jacket", 125m, "AWWSJ", null, 0 },
                    { 14, 2, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7637), null, true, null, null, "Stretchy Dance Pants", 55m, "AWWSDP", null, 0 },
                    { 15, 2, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7639), null, true, null, null, "Ultra-Soft Tank Top", 22m, "AWWUTT", null, 0 },
                    { 16, 2, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7641), null, true, null, null, "Unisex Thermal Vest", 95m, "AWWUTV", null, 0 },
                    { 17, 2, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7644), null, true, null, null, "V-Next T-Shirt", 17m, "AWWVNT", null, 0 },
                    { 18, 3, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7648), null, true, null, null, "Blueberry Mineral Water", 2.8m, "MWB", null, 0 },
                    { 19, 3, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7650), null, true, null, null, "Lemon-Lime Mineral Water", 2.8m, "MWLL", null, 0 },
                    { 20, 3, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7653), null, true, null, null, "Orange Mineral Water", 2.8m, "MWO", null, 0 },
                    { 21, 3, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7655), null, true, null, null, "Peach Mineral Water", 2.8m, "MWP", null, 0 },
                    { 22, 3, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7657), null, true, null, null, "Raspberry Mineral Water", 2.8m, "MWR", null, 0 },
                    { 23, 3, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7659), null, true, null, null, "Strawberry Mineral Water", 2.8m, "MWS", null, 0 },
                    { 24, 4, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7661), null, true, null, null, "In the Kitchen with H+ Sport", 24.99m, "PITK", null, 0 },
                    { 25, 5, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7664), null, true, null, null, "Calcium 400 IU (150 tablets)", 9.99m, "SC400", null, 0 },
                    { 26, 5, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7666), null, true, null, null, "Flaxseed Oil 100 mg (90 capsules)", 12.49m, "SFO100", null, 0 },
                    { 27, 5, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7669), null, true, null, null, "Iron 65 mg (150 caplets)", 13.99m, "SI65", null, 0 },
                    { 28, 5, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7671), null, true, null, null, "Magnesium 250 mg (100 tablets)", 12.49m, "SM250", null, 0 },
                    { 29, 5, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7673), null, true, null, null, "Multi-Vitamin (90 capsules)", 9.99m, "SMV", null, 0 },
                    { 30, 5, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7675), null, true, null, null, "Vitamin A 10,000 IU (125 caplets)", 11.99m, "SVA", null, 0 },
                    { 31, 5, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7678), null, true, null, null, "Vitamin B-Complex (100 caplets)", 12.99m, "SVB", null, 0 },
                    { 32, 5, null, new DateTime(2023, 12, 19, 8, 49, 30, 252, DateTimeKind.Utc).AddTicks(7680), null, true, null, null, "Vitamin C 1000 mg (100 tablets)", 9.99m, "SVC", null, 0 },
                    { 33, 5, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, null, "Vitamin D3 1000 IU (100 tablets)", 12.49m, "SVD3", null, 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
