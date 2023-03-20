using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtoCommerce.QuoteModule.Data.Model;

namespace VirtoCommerce.QuoteModule.Data.MySql
{
    public class TierPriceEntityConfiguration : IEntityTypeConfiguration<TierPriceEntity>
    {
        public void Configure(EntityTypeBuilder<TierPriceEntity> builder)
        {
            builder.Property(x => x.Price).HasColumnType("decimal").HasPrecision(18, 4);
        }
    }
}
