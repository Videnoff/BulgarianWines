namespace BulgarianWines.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddPaymentStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WineOrders_Wines_WineId1",
                table: "WineOrders");

            migrationBuilder.DropIndex(
                name: "IX_WineOrders_WineId1",
                table: "WineOrders");

            migrationBuilder.DropColumn(
                name: "WineId1",
                table: "WineOrders");

            migrationBuilder.RenameColumn(
                name: "ZIPCode",
                table: "Cities",
                newName: "PostCode");

            migrationBuilder.AlterColumn<int>(
                name: "WineId",
                table: "WineOrders",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryPrice",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryType",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentStatus",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentType",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "UserFullName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WineOrders_WineId",
                table: "WineOrders",
                column: "WineId");

            migrationBuilder.AddForeignKey(
                name: "FK_WineOrders_Wines_WineId",
                table: "WineOrders",
                column: "WineId",
                principalTable: "Wines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WineOrders_Wines_WineId",
                table: "WineOrders");

            migrationBuilder.DropIndex(
                name: "IX_WineOrders_WineId",
                table: "WineOrders");

            migrationBuilder.DropColumn(
                name: "DeliveryPrice",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryType",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UserFullName",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "PostCode",
                table: "Cities",
                newName: "ZIPCode");

            migrationBuilder.AlterColumn<string>(
                name: "WineId",
                table: "WineOrders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "WineId1",
                table: "WineOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WineOrders_WineId1",
                table: "WineOrders",
                column: "WineId1");

            migrationBuilder.AddForeignKey(
                name: "FK_WineOrders_Wines_WineId1",
                table: "WineOrders",
                column: "WineId1",
                principalTable: "Wines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
