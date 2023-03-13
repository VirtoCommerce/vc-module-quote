using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using VirtoCommerce.QuoteModule.Data.Repositories;

namespace VirtoCommerce.QuoteModule.Data.PostgreSql
{
    public class PostgreSqlDbContextFactory : IDesignTimeDbContextFactory<QuoteDbContext>
    {
        public QuoteDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<QuoteDbContext>();
            var connectionString = args.Any() ? args[0] : "User ID = postgres; Password = password; Host = localhost; Port = 5432; Database = virtocommerce3;";

            builder.UseNpgsql(
                connectionString,
                db => db.MigrationsAssembly(typeof(PostgreSqlDbContextFactory).Assembly.GetName().Name));

            return new QuoteDbContext(builder.Options);
        }
    }
}
