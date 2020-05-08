using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VirtoCommerce.QuoteModule.Data.Repositories
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<QuoteDbContext>
    {
        public QuoteDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<QuoteDbContext>();

            builder.UseSqlServer("Data Source=(local);Initial Catalog=VirtoCommerce;Persist Security Info=True;User ID=virto;Password=virto;MultipleActiveResultSets=True;Connect Timeout=30");

            return new QuoteDbContext(builder.Options);
        }
    }
}
