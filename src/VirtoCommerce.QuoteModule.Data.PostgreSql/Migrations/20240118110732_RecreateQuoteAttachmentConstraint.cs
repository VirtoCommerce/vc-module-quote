using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.QuoteModule.Data.PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class RecreateQuoteAttachmentConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM ""QuoteAttachment"" WHERE ""QuoteRequestId"" IS NULL");

            migrationBuilder.DropForeignKey(
                name: "FK_QuoteAttachment_QuoteRequest_QuoteRequestId",
                table: "QuoteAttachment");

            migrationBuilder.AlterColumn<string>(
                name: "QuoteRequestId",
                table: "QuoteAttachment",
                type: "character varying(128)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_QuoteAttachment_QuoteRequest_QuoteRequestId",
                table: "QuoteAttachment",
                column: "QuoteRequestId",
                principalTable: "QuoteRequest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuoteAttachment_QuoteRequest_QuoteRequestId",
                table: "QuoteAttachment");

            migrationBuilder.AlterColumn<string>(
                name: "QuoteRequestId",
                table: "QuoteAttachment",
                type: "character varying(128)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)");

            migrationBuilder.AddForeignKey(
                name: "FK_QuoteAttachment_QuoteRequest_QuoteRequestId",
                table: "QuoteAttachment",
                column: "QuoteRequestId",
                principalTable: "QuoteRequest",
                principalColumn: "Id");
        }
    }
}
