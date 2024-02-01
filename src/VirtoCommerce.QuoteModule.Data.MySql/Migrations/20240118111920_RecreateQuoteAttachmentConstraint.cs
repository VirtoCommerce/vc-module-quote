using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.QuoteModule.Data.MySql.Migrations
{
    /// <inheritdoc />
    public partial class RecreateQuoteAttachmentConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM QuoteAttachment WHERE QuoteRequestId IS NULL;");

            migrationBuilder.DropForeignKey(
                name: "FK_QuoteAttachment_QuoteRequest_QuoteRequestId",
                table: "QuoteAttachment");

            migrationBuilder.UpdateData(
                table: "QuoteAttachment",
                keyColumn: "QuoteRequestId",
                keyValue: null,
                column: "QuoteRequestId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "QuoteRequestId",
                table: "QuoteAttachment",
                type: "varchar(128)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

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
                type: "varchar(128)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(128)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_QuoteAttachment_QuoteRequest_QuoteRequestId",
                table: "QuoteAttachment",
                column: "QuoteRequestId",
                principalTable: "QuoteRequest",
                principalColumn: "Id");
        }
    }
}
