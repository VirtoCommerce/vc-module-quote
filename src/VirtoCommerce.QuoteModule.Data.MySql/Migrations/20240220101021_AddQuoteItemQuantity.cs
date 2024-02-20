using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.QuoteModule.Data.MySql.Migrations
{
    /// <inheritdoc />
    public partial class AddQuoteItemQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "QuoteItem",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "QuoteItem");
        }
    }
}
