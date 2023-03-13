using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtoCommerce.QuoteModule.Data.Model;

namespace VirtoCommerce.QuoteModule.Data.MySql
{
    public class QuoteItemEntityConfiguration : IEntityTypeConfiguration<QuoteItemEntity>
    {
        public void Configure(EntityTypeBuilder<QuoteItemEntity> builder)
        {
            builder.Property(x => x.ListPrice).HasColumnType("decimal").HasPrecision(18, 4);
            builder.Property(x => x.SalePrice).HasColumnType("decimal").HasPrecision(18, 4);
        }
    }
}
