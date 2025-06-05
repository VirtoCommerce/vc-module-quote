using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.QuoteModule.Data.PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class AddConfigurationItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsConfigured",
                table: "QuoteItem",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "QuoteConfigurationItem",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    QuoteItemId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ProductId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Name = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    Sku = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(1028)", maxLength: 1028, nullable: true),
                    CatalogId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    CategoryId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Type = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CustomText = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    ModifiedBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuoteConfigurationItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuoteConfigurationItem_QuoteItem_QuoteItemId",
                        column: x => x.QuoteItemId,
                        principalTable: "QuoteItem",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuoteConfigurationItemFile",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Url = table.Column<string>(type: "character varying(2083)", maxLength: 2083, nullable: false),
                    Name = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    ContentType = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    ConfigurationItemId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    ModifiedBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuoteConfigurationItemFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuoteConfigurationItemFile_QuoteConfigurationItem_Configura~",
                        column: x => x.ConfigurationItemId,
                        principalTable: "QuoteConfigurationItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuoteConfigurationItem_QuoteItemId",
                table: "QuoteConfigurationItem",
                column: "QuoteItemId");

            migrationBuilder.CreateIndex(
                name: "IX_QuoteConfigurationItemFile_ConfigurationItemId",
                table: "QuoteConfigurationItemFile",
                column: "ConfigurationItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuoteConfigurationItemFile");

            migrationBuilder.DropTable(
                name: "QuoteConfigurationItem");

            migrationBuilder.DropColumn(
                name: "IsConfigured",
                table: "QuoteItem");
        }
    }
}
