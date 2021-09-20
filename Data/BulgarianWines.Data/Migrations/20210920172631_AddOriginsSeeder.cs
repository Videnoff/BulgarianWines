using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BulgarianWines.Data.Migrations
{
    public partial class AddOriginsSeeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Origin",
                table: "Wines");

            migrationBuilder.AddColumn<int>(
                name: "OriginId",
                table: "Wines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Origins",
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
                    table.PrimaryKey("PK_Origins", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wines_OriginId",
                table: "Wines",
                column: "OriginId");

            migrationBuilder.CreateIndex(
                name: "IX_Origins_IsDeleted",
                table: "Origins",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_Wines_Origins_OriginId",
                table: "Wines",
                column: "OriginId",
                principalTable: "Origins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wines_Origins_OriginId",
                table: "Wines");

            migrationBuilder.DropTable(
                name: "Origins");

            migrationBuilder.DropIndex(
                name: "IX_Wines_OriginId",
                table: "Wines");

            migrationBuilder.DropColumn(
                name: "OriginId",
                table: "Wines");

            migrationBuilder.AddColumn<string>(
                name: "Origin",
                table: "Wines",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
