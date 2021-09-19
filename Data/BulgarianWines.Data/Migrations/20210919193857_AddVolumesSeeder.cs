namespace BulgarianWines.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddVolumesSeeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Harvest",
                table: "Wines",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Origin",
                table: "Wines",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Variety",
                table: "Wines",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VolumeId",
                table: "Wines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Volumes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Volumes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wines_VolumeId",
                table: "Wines",
                column: "VolumeId");

            migrationBuilder.CreateIndex(
                name: "IX_Volumes_IsDeleted",
                table: "Volumes",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_Wines_Volumes_VolumeId",
                table: "Wines",
                column: "VolumeId",
                principalTable: "Volumes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wines_Volumes_VolumeId",
                table: "Wines");

            migrationBuilder.DropTable(
                name: "Volumes");

            migrationBuilder.DropIndex(
                name: "IX_Wines_VolumeId",
                table: "Wines");

            migrationBuilder.DropColumn(
                name: "Harvest",
                table: "Wines");

            migrationBuilder.DropColumn(
                name: "Origin",
                table: "Wines");

            migrationBuilder.DropColumn(
                name: "Variety",
                table: "Wines");

            migrationBuilder.DropColumn(
                name: "VolumeId",
                table: "Wines");
        }
    }
}
