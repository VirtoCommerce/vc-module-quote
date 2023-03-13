using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtoCommerce.QuoteModule.Data.SqlServer.Migrations
{
    public partial class InitialQuotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuoteRequest",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 128, nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 64, nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 64, nullable: true),
                    Number = table.Column<string>(maxLength: 64, nullable: false),
                    StoreId = table.Column<string>(maxLength: 64, nullable: false),
                    StoreName = table.Column<string>(maxLength: 255, nullable: true),
                    ChannelId = table.Column<string>(maxLength: 64, nullable: true),
                    OrganizationId = table.Column<string>(maxLength: 64, nullable: true),
                    OrganizationName = table.Column<string>(maxLength: 255, nullable: true),
                    IsAnonymous = table.Column<bool>(nullable: false),
                    CustomerId = table.Column<string>(maxLength: 64, nullable: true),
                    CustomerName = table.Column<string>(maxLength: 255, nullable: true),
                    EmployeeId = table.Column<string>(maxLength: 64, nullable: true),
                    EmployeeName = table.Column<string>(maxLength: 255, nullable: true),
                    ExpirationDate = table.Column<DateTime>(nullable: true),
                    ReminderDate = table.Column<DateTime>(nullable: true),
                    EnableNotification = table.Column<bool>(nullable: false),
                    IsLocked = table.Column<bool>(nullable: false),
                    Status = table.Column<string>(maxLength: 64, nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    InnerComment = table.Column<string>(nullable: true),
                    Tag = table.Column<string>(maxLength: 128, nullable: true),
                    IsSubmitted = table.Column<bool>(nullable: false),
                    Currency = table.Column<string>(maxLength: 3, nullable: false),
                    LanguageCode = table.Column<string>(maxLength: 5, nullable: true),
                    Coupon = table.Column<string>(maxLength: 64, nullable: true),
                    ShipmentMethodCode = table.Column<string>(maxLength: 64, nullable: true),
                    ShipmentMethodOption = table.Column<string>(maxLength: 64, nullable: true),
                    IsCancelled = table.Column<bool>(nullable: false),
                    CancelledDate = table.Column<DateTime>(nullable: true),
                    CancelReason = table.Column<string>(maxLength: 2048, nullable: true),
                    ManualSubTotal = table.Column<decimal>(type: "Money", nullable: false),
                    ManualRelDiscountAmount = table.Column<decimal>(nullable: false),
                    ManualShippingTotal = table.Column<decimal>(type: "Money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuoteRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuoteAddress",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 128, nullable: false),
                    AddressType = table.Column<string>(maxLength: 32, nullable: true),
                    Organization = table.Column<string>(maxLength: 64, nullable: true),
                    CountryCode = table.Column<string>(maxLength: 3, nullable: true),
                    CountryName = table.Column<string>(maxLength: 64, nullable: false),
                    City = table.Column<string>(maxLength: 128, nullable: false),
                    PostalCode = table.Column<string>(maxLength: 64, nullable: true),
                    Line1 = table.Column<string>(maxLength: 2048, nullable: true),
                    Line2 = table.Column<string>(maxLength: 2048, nullable: true),
                    RegionId = table.Column<string>(maxLength: 128, nullable: true),
                    RegionName = table.Column<string>(maxLength: 128, nullable: true),
                    FirstName = table.Column<string>(maxLength: 64, nullable: false),
                    LastName = table.Column<string>(maxLength: 64, nullable: false),
                    Phone = table.Column<string>(maxLength: 64, nullable: true),
                    Email = table.Column<string>(maxLength: 254, nullable: true),
                    QuoteRequestId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuoteAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuoteAddress_QuoteRequest_QuoteRequestId",
                        column: x => x.QuoteRequestId,
                        principalTable: "QuoteRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuoteAttachment",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 128, nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 64, nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 64, nullable: true),
                    Url = table.Column<string>(maxLength: 2083, nullable: false),
                    Name = table.Column<string>(maxLength: 1024, nullable: true),
                    MimeType = table.Column<string>(maxLength: 128, nullable: true),
                    Size = table.Column<long>(nullable: false),
                    QuoteRequestId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuoteAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuoteAttachment_QuoteRequest_QuoteRequestId",
                        column: x => x.QuoteRequestId,
                        principalTable: "QuoteRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuoteItem",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 128, nullable: false),
                    Currency = table.Column<string>(maxLength: 3, nullable: false),
                    ListPrice = table.Column<decimal>(type: "Money", nullable: false),
                    SalePrice = table.Column<decimal>(type: "Money", nullable: false),
                    ProductId = table.Column<string>(maxLength: 64, nullable: false),
                    Sku = table.Column<string>(maxLength: 64, nullable: false),
                    CatalogId = table.Column<string>(maxLength: 64, nullable: false),
                    CategoryId = table.Column<string>(maxLength: 64, nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    Comment = table.Column<string>(maxLength: 2048, nullable: true),
                    ImageUrl = table.Column<string>(maxLength: 1028, nullable: true),
                    TaxType = table.Column<string>(maxLength: 64, nullable: true),
                    QuoteRequestId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuoteItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuoteItem_QuoteRequest_QuoteRequestId",
                        column: x => x.QuoteRequestId,
                        principalTable: "QuoteRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuoteTierPrice",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 128, nullable: false),
                    Price = table.Column<decimal>(type: "Money", nullable: false),
                    Quantity = table.Column<long>(nullable: false),
                    QuoteItemId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuoteTierPrice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuoteTierPrice_QuoteItem_QuoteItemId",
                        column: x => x.QuoteItemId,
                        principalTable: "QuoteItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuoteAddress_QuoteRequestId",
                table: "QuoteAddress",
                column: "QuoteRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_QuoteAttachment_QuoteRequestId",
                table: "QuoteAttachment",
                column: "QuoteRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_QuoteItem_QuoteRequestId",
                table: "QuoteItem",
                column: "QuoteRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_QuoteTierPrice_QuoteItemId",
                table: "QuoteTierPrice",
                column: "QuoteItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuoteAddress");

            migrationBuilder.DropTable(
                name: "QuoteAttachment");

            migrationBuilder.DropTable(
                name: "QuoteTierPrice");

            migrationBuilder.DropTable(
                name: "QuoteItem");

            migrationBuilder.DropTable(
                name: "QuoteRequest");
        }
    }
}
