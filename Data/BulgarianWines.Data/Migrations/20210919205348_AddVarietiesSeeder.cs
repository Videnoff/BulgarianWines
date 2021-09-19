using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BulgarianWines.Data.Migrations
{
    public partial class AddVarietiesSeeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Variety",
                table: "Wines");

            migrationBuilder.AddColumn<int>(
                name: "VarietyId",
                table: "Wines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Varieties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Varieties", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wines_VarietyId",
                table: "Wines",
                column: "VarietyId");

            migrationBuilder.CreateIndex(
                name: "IX_Varieties_IsDeleted",
                table: "Varieties",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_Wines_Varieties_VarietyId",
                table: "Wines",
                column: "VarietyId",
                principalTable: "Varieties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wines_Varieties_VarietyId",
                table: "Wines");

            migrationBuilder.DropTable(
                name: "Varieties");

            migrationBuilder.DropIndex(
                name: "IX_Wines_VarietyId",
                table: "Wines");

            migrationBuilder.DropColumn(
                name: "VarietyId",
                table: "Wines");

            migrationBuilder.AddColumn<string>(
                name: "Variety",
                table: "Wines",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
