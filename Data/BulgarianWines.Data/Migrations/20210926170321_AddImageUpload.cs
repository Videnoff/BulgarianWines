namespace BulgarianWines.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddImageUpload : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RemoteImageUrl",
                table: "Images",
                newName: "ImageUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Images",
                newName: "RemoteImageUrl");
        }
    }
}
