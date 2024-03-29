// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using VirtoCommerce.QuoteModule.Data.Repositories;

namespace VirtoCommerce.QuoteModule.Data.SqlServer.Migrations
{
    [DbContext(typeof(QuoteDbContext))]
    [Migration("20000000000000_UpdateQuoteV2")]
    partial class UpdateQuoteV2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("VirtoCommerce.QuoteModule.Data.Model.AddressEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("AddressType")
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("CountryCode")
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.Property<string>("CountryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(254)")
                        .HasMaxLength(254);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("Line1")
                        .HasColumnType("nvarchar(2048)")
                        .HasMaxLength(2048);

                    b.Property<string>("Line2")
                        .HasColumnType("nvarchar(2048)")
                        .HasMaxLength(2048);

                    b.Property<string>("Organization")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("QuoteRequestId")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("RegionId")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("RegionName")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.HasIndex("QuoteRequestId");

                    b.ToTable("QuoteAddress");
                });

            modelBuilder.Entity("VirtoCommerce.QuoteModule.Data.Model.AttachmentEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("MimeType")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(1024)")
                        .HasMaxLength(1024);

                    b.Property<string>("QuoteRequestId")
                        .HasColumnType("nvarchar(128)");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(2083)")
                        .HasMaxLength(2083);

                    b.HasKey("Id");

                    b.HasIndex("QuoteRequestId");

                    b.ToTable("QuoteAttachment");
                });

            modelBuilder.Entity("VirtoCommerce.QuoteModule.Data.Model.QuoteItemEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("CatalogId")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("CategoryId")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(2048)")
                        .HasMaxLength(2048);

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(1028)")
                        .HasMaxLength(1028);

                    b.Property<decimal>("ListPrice")
                        .HasColumnType("Money");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("QuoteRequestId")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)");

                    b.Property<decimal>("SalePrice")
                        .HasColumnType("Money");

                    b.Property<string>("Sku")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("TaxType")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("QuoteRequestId");

                    b.ToTable("QuoteItem");
                });

            modelBuilder.Entity("VirtoCommerce.QuoteModule.Data.Model.QuoteRequestEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("CancelReason")
                        .HasColumnType("nvarchar(2048)")
                        .HasMaxLength(2048);

                    b.Property<DateTime?>("CancelledDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ChannelId")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Coupon")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.Property<string>("CustomerId")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("CustomerName")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("EmployeeId")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("EmployeeName")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<bool>("EnableNotification")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("InnerComment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAnonymous")
                        .HasColumnType("bit");

                    b.Property<bool>("IsCancelled")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSubmitted")
                        .HasColumnType("bit");

                    b.Property<string>("LanguageCode")
                        .HasColumnType("nvarchar(5)")
                        .HasMaxLength(5);

                    b.Property<decimal>("ManualRelDiscountAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ManualShippingTotal")
                        .HasColumnType("Money");

                    b.Property<decimal>("ManualSubTotal")
                        .HasColumnType("Money");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("OrganizationId")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("OrganizationName")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<DateTime?>("ReminderDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ShipmentMethodCode")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("ShipmentMethodOption")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("StoreId")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("StoreName")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("Tag")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.ToTable("QuoteRequest");
                });

            modelBuilder.Entity("VirtoCommerce.QuoteModule.Data.Model.TierPriceEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<decimal>("Price")
                        .HasColumnType("Money");

                    b.Property<long>("Quantity")
                        .HasColumnType("bigint");

                    b.Property<string>("QuoteItemId")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.HasIndex("QuoteItemId");

                    b.ToTable("QuoteTierPrice");
                });

            modelBuilder.Entity("VirtoCommerce.QuoteModule.Data.Model.AddressEntity", b =>
                {
                    b.HasOne("VirtoCommerce.QuoteModule.Data.Model.QuoteRequestEntity", "QuoteRequest")
                        .WithMany("Addresses")
                        .HasForeignKey("QuoteRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VirtoCommerce.QuoteModule.Data.Model.AttachmentEntity", b =>
                {
                    b.HasOne("VirtoCommerce.QuoteModule.Data.Model.QuoteRequestEntity", "QuoteRequest")
                        .WithMany("Attachments")
                        .HasForeignKey("QuoteRequestId");
                });

            modelBuilder.Entity("VirtoCommerce.QuoteModule.Data.Model.QuoteItemEntity", b =>
                {
                    b.HasOne("VirtoCommerce.QuoteModule.Data.Model.QuoteRequestEntity", "QuoteRequest")
                        .WithMany("Items")
                        .HasForeignKey("QuoteRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VirtoCommerce.QuoteModule.Data.Model.TierPriceEntity", b =>
                {
                    b.HasOne("VirtoCommerce.QuoteModule.Data.Model.QuoteItemEntity", "QuoteItem")
                        .WithMany("ProposalPrices")
                        .HasForeignKey("QuoteItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
