using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtoCommerce.QuoteModule.Data.Model;

namespace VirtoCommerce.QuoteModule.Data.MySql
{
    public class QuoteRequestEntityConfiguration : IEntityTypeConfiguration<QuoteRequestEntity>
    {
        public void Configure(EntityTypeBuilder<QuoteRequestEntity> builder)
        {
            builder.Property(x => x.ManualSubTotal).HasColumnType("decimal").HasPrecision(18, 4);
            builder.Property(x => x.ManualShippingTotal).HasColumnType("decimal").HasPrecision(18, 4);
        }
    }
}
