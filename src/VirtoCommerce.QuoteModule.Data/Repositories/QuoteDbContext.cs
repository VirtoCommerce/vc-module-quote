using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.QuoteModule.Data.Model;

namespace VirtoCommerce.QuoteModule.Data.Repositories
{
    public class QuoteDbContext : DbContextWithTriggers
    {
        public QuoteDbContext(DbContextOptions<QuoteDbContext> options)
            : base(options)
        {
        }

        protected QuoteDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region AddressEntity
            modelBuilder.Entity<AddressEntity>().ToTable("QuoteAddress").HasKey(x => x.Id);
            modelBuilder.Entity<AddressEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
            modelBuilder.Entity<AddressEntity>().HasOne(x => x.QuoteRequest).WithMany(x => x.Addresses)
                        .HasForeignKey(x => x.QuoteRequestId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region AttachmentEntity
            modelBuilder.Entity<AttachmentEntity>().ToTable("QuoteAttachment").HasKey(x => x.Id);
            modelBuilder.Entity<AttachmentEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
            modelBuilder.Entity<AddressEntity>().HasOne(x => x.QuoteRequest).WithMany(x => x.Addresses)
                        .HasForeignKey(x => x.QuoteRequestId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region QuoteItemEntity
            modelBuilder.Entity<QuoteItemEntity>().ToTable("QuoteItem").HasKey(x => x.Id);
            modelBuilder.Entity<QuoteItemEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
            modelBuilder.Entity<QuoteItemEntity>().HasOne(x => x.QuoteRequest).WithMany(x => x.Items)
                        .HasForeignKey(x => x.QuoteRequestId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region TierPriceEntity
            modelBuilder.Entity<TierPriceEntity>().ToTable("QuoteTierPrice").HasKey(x => x.Id);
            modelBuilder.Entity<TierPriceEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
            modelBuilder.Entity<TierPriceEntity>().HasOne(x => x.QuoteItem).WithMany(x => x.ProposalPrices)
                        .HasForeignKey(x => x.QuoteItemId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region QuoteRequestEntity
            modelBuilder.Entity<QuoteRequestEntity>().ToTable("QuoteRequest").HasKey(x => x.Id);
            modelBuilder.Entity<QuoteRequestEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
            modelBuilder.Entity<QuoteRequestEntity>().Property(x => x.ManualRelDiscountAmount).HasPrecision(18, 2);

            #endregion


            #region DynamicPropertyValues
            modelBuilder.Entity<QuoteDynamicPropertyObjectValueEntity>().ToTable("QuoteDynamicPropertyObjectValue").HasKey(x => x.Id);
            modelBuilder.Entity<QuoteDynamicPropertyObjectValueEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
            modelBuilder.Entity<QuoteDynamicPropertyObjectValueEntity>().Property(x => x.DecimalValue).HasColumnType("decimal(18,5)");
            modelBuilder.Entity<QuoteDynamicPropertyObjectValueEntity>().HasOne(p => p.QuoteRequest)
                .WithMany(s => s.DynamicPropertyObjectValues).HasForeignKey(k => k.ObjectId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<QuoteDynamicPropertyObjectValueEntity>().HasIndex(x => new { x.ObjectType, x.ObjectId })
                .IsUnique(false)
                .HasDatabaseName("IX_ObjectType_ObjectId");
            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
