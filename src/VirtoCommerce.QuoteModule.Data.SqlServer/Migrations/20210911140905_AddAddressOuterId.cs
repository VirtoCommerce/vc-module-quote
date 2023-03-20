using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtoCommerce.QuoteModule.Data.SqlServer.Migrations
{
    public partial class AddAddressOuterId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OuterId",
                table: "QuoteAddress",
                maxLength: 128,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OuterId",
                table: "QuoteAddress");
        }
    }
}
