using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtoCommerce.QuoteModule.Data.SqlServer.Migrations
{
    public partial class UpdateQuoteV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '__MigrationHistory'))
        IF (EXISTS (SELECT * FROM __MigrationHistory WHERE ContextKey = 'VirtoCommerce.QuoteModule.Data.Migrations.Configuration'))
            BEGIN
                INSERT INTO [__EFMigrationsHistory] ([MigrationId],[ProductVersion]) VALUES ('20200506113606_InitialQuotes', '3.1.2')
            END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
