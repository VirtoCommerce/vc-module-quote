using Microsoft.EntityFrameworkCore;

namespace VirtoCommerce.QuoteModule.Data.MySql
{
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Configures the context to use PostgreSql.
        /// </summary>
        public static DbContextOptionsBuilder UseMySqlDatabase(this DbContextOptionsBuilder builder, string connectionString) =>
            builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), db => db
                .MigrationsAssembly(typeof(MySqlDbContextFactory).Assembly.GetName().Name));
    }
}
