using Microsoft.EntityFrameworkCore.Migrations;

namespace BulgarianWines.Data.Migrations
{
    public partial class AddOrderProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserFirstName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserLastName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserFirstName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UserLastName",
                table: "Orders");
        }
    }
}
