using System.Reflection;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.QuoteModule.Data.Model;

namespace VirtoCommerce.QuoteModule.Data.Repositories
{
    public class QuoteDbContext : DbContextBase
    {
#pragma warning disable S109
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
            modelBuilder.Entity<AttachmentEntity>().HasOne(x => x.QuoteRequest).WithMany(x => x.Attachments)
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
                .HasDatabaseName("IX_QuoteDynamicPropertyObjectValue_ObjectType_ObjectId");
            #endregion

            #region QuoteConfigurationItem

            modelBuilder.Entity<ConfigurationItemEntity>().ToTable("QuoteConfigurationItem").HasKey(x => x.Id);
            modelBuilder.Entity<ConfigurationItemEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
            modelBuilder.Entity<ConfigurationItemEntity>().HasOne(x => x.QuoteItem).WithMany(x => x.ConfigurationItems)
                        .HasForeignKey(x => x.QuoteItemId).IsRequired().OnDelete(DeleteBehavior.ClientCascade);

            #endregion

            #region QuoteConfigurationItemFile

            modelBuilder.Entity<ConfigurationItemFileEntity>().ToTable("QuoteConfigurationItemFile").HasKey(x => x.Id);
            modelBuilder.Entity<ConfigurationItemFileEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
            modelBuilder.Entity<ConfigurationItemFileEntity>().HasOne(x => x.ConfigurationItem).WithMany(x => x.Files)
                .HasForeignKey(x => x.ConfigurationItemId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            #endregion

            base.OnModelCreating(modelBuilder);


            // Allows configuration for an entity type for different database types.
            // Applies configuration from all <see cref="IEntityTypeConfiguration{TEntity}" in VirtoCommerce.QuoteModule.Data.XXX project. /> 
            switch (this.Database.ProviderName)
            {
                case "Pomelo.EntityFrameworkCore.MySql":
                    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.QuoteModule.Data.MySql"));
                    break;
                case "Npgsql.EntityFrameworkCore.PostgreSQL":
                    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.QuoteModule.Data.PostgreSql"));
                    break;
                case "Microsoft.EntityFrameworkCore.SqlServer":
                    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.QuoteModule.Data.SqlServer"));
                    break;
            }
        }
    }
#pragma warning restore S109
}
