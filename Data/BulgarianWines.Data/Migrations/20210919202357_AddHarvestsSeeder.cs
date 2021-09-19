﻿namespace BulgarianWines.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddHarvestsSeeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Harvest",
                table: "Wines");

            migrationBuilder.AddColumn<int>(
                name: "HarvestId",
                table: "Wines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Harvests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Harvests", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wines_HarvestId",
                table: "Wines",
                column: "HarvestId");

            migrationBuilder.CreateIndex(
                name: "IX_Harvests_IsDeleted",
                table: "Harvests",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_Wines_Harvests_HarvestId",
                table: "Wines",
                column: "HarvestId",
                principalTable: "Harvests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wines_Harvests_HarvestId",
                table: "Wines");

            migrationBuilder.DropTable(
                name: "Harvests");

            migrationBuilder.DropIndex(
                name: "IX_Wines_HarvestId",
                table: "Wines");

            migrationBuilder.DropColumn(
                name: "HarvestId",
                table: "Wines");

            migrationBuilder.AddColumn<string>(
                name: "Harvest",
                table: "Wines",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
