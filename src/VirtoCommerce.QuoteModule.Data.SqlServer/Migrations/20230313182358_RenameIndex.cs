using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.QuoteModule.Data.SqlServer.Migrations
{
    public partial class RenameIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_ObjectType_ObjectId",
                table: "QuoteDynamicPropertyObjectValue",
                newName: "IX_QuoteDynamicPropertyObjectValue_ObjectType_ObjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_QuoteDynamicPropertyObjectValue_ObjectType_ObjectId",
                table: "QuoteDynamicPropertyObjectValue",
                newName: "IX_ObjectType_ObjectId");
        }
    }
}
