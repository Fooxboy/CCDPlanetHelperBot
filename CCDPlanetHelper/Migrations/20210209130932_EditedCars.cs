using Microsoft.EntityFrameworkCore.Migrations;

namespace CCDPlanetHelper.Migrations
{
    public partial class EditedCars : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "PriceFromSalon",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "PriceUsed",
                table: "Cars",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "PriceSellUsed",
                table: "Cars",
                newName: "IsPublic");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Cars",
                newName: "PriceUsed");

            migrationBuilder.RenameColumn(
                name: "IsPublic",
                table: "Cars",
                newName: "PriceSellUsed");

            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "Cars",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PriceFromSalon",
                table: "Cars",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
