﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using VirtoCommerce.QuoteModule.Data.Repositories;

#nullable disable

namespace VirtoCommerce.QuoteModule.Data.PostgreSql.Migrations
{
    [DbContext(typeof(QuoteDbContext))]
    [Migration("20240830073235_RelaxQuoteItemRestrictions")]
    partial class RelaxQuoteItemRestrictions
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("VirtoCommerce.QuoteModule.Data.Model.AddressEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("AddressType")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("CountryCode")
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.Property<string>("CountryName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("Line1")
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.Property<string>("Line2")
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.Property<string>("Organization")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("OuterId")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("Phone")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("PostalCode")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("QuoteRequestId")
                        .IsRequired()
                        .HasColumnType("character varying(128)");

                    b.Property<string>("RegionId")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("RegionName")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.HasKey("Id");

                    b.HasIndex("QuoteRequestId");

                    b.ToTable("QuoteAddress", (string)null);
                });

            modelBuilder.Entity("VirtoCommerce.QuoteModule.Data.Model.AttachmentEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("MimeType")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<string>("QuoteRequestId")
                        .IsRequired()
                        .HasColumnType("character varying(128)");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(2083)
                        .HasColumnType("character varying(2083)");

                    b.HasKey("Id");

                    b.HasIndex("QuoteRequestId");

                    b.ToTable("QuoteAttachment", (string)null);
                });

            modelBuilder.Entity("VirtoCommerce.QuoteModule.Data.Model.QuoteDynamicPropertyObjectValueEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<bool?>("BooleanValue")
                        .HasColumnType("boolean");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DateTimeValue")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal?>("DecimalValue")
                        .HasColumnType("decimal(18,5)");

                    b.Property<string>("DictionaryItemId")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<int?>("IntegerValue")
                        .HasColumnType("integer");

                    b.Property<string>("Locale")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("LongTextValue")
                        .HasColumnType("text");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ObjectId")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ObjectType")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PropertyId")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("PropertyName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("ShortTextValue")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("ValueType")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.HasKey("Id");

                    b.HasIndex("ObjectId");

                    b.HasIndex("ObjectType", "ObjectId")
                        .HasDatabaseName("IX_QuoteDynamicPropertyObjectValue_ObjectType_ObjectId");

                    b.ToTable("QuoteDynamicPropertyObjectValue", (string)null);
                });

            modelBuilder.Entity("VirtoCommerce.QuoteModule.Data.Model.QuoteItemEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("CatalogId")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("CategoryId")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("Comment")
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(1028)
                        .HasColumnType("character varying(1028)");

                    b.Property<decimal>("ListPrice")
                        .HasColumnType("Money");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<string>("ProductId")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<string>("QuoteRequestId")
                        .IsRequired()
                        .HasColumnType("character varying(128)");

                    b.Property<decimal>("SalePrice")
                        .HasColumnType("Money");

                    b.Property<string>("Sku")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("TaxType")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.HasKey("Id");

                    b.HasIndex("QuoteRequestId");

                    b.ToTable("QuoteItem", (string)null);
                });

            modelBuilder.Entity("VirtoCommerce.QuoteModule.Data.Model.QuoteRequestEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("CancelReason")
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.Property<DateTime?>("CancelledDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ChannelId")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<string>("Coupon")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.Property<string>("CustomerId")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("CustomerName")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("EmployeeId")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("EmployeeName")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<bool>("EnableNotification")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("ExpirationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("InnerComment")
                        .HasColumnType("text");

                    b.Property<bool>("IsAnonymous")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsCancelled")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsSubmitted")
                        .HasColumnType("boolean");

                    b.Property<string>("LanguageCode")
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)");

                    b.Property<decimal>("ManualRelDiscountAmount")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)");

                    b.Property<decimal>("ManualShippingTotal")
                        .HasColumnType("Money");

                    b.Property<decimal>("ManualSubTotal")
                        .HasColumnType("Money");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("OrganizationId")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("OrganizationName")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTime?>("ReminderDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ShipmentMethodCode")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("ShipmentMethodOption")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("Status")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("StoreId")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("StoreName")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Tag")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.HasKey("Id");

                    b.ToTable("QuoteRequest", (string)null);
                });

            modelBuilder.Entity("VirtoCommerce.QuoteModule.Data.Model.TierPriceEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<decimal>("Price")
                        .HasColumnType("Money");

                    b.Property<long>("Quantity")
                        .HasColumnType("bigint");

                    b.Property<string>("QuoteItemId")
                        .IsRequired()
                        .HasColumnType("character varying(128)");

                    b.HasKey("Id");

                    b.HasIndex("QuoteItemId");

                    b.ToTable("QuoteTierPrice", (string)null);
                });

            modelBuilder.Entity("VirtoCommerce.QuoteModule.Data.Model.AddressEntity", b =>
                {
                    b.HasOne("VirtoCommerce.QuoteModule.Data.Model.QuoteRequestEntity", "QuoteRequest")
                        .WithMany("Addresses")
                        .HasForeignKey("QuoteRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("QuoteRequest");
                });

            modelBuilder.Entity("VirtoCommerce.QuoteModule.Data.Model.AttachmentEntity", b =>
                {
                    b.HasOne("VirtoCommerce.QuoteModule.Data.Model.QuoteRequestEntity", "QuoteRequest")
                        .WithMany("Attachments")
                        .HasForeignKey("QuoteRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("QuoteRequest");
                });

            modelBuilder.Entity("VirtoCommerce.QuoteModule.Data.Model.QuoteDynamicPropertyObjectValueEntity", b =>
                {
                    b.HasOne("VirtoCommerce.QuoteModule.Data.Model.QuoteRequestEntity", "QuoteRequest")
                        .WithMany("DynamicPropertyObjectValues")
                        .HasForeignKey("ObjectId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("QuoteRequest");
                });

            modelBuilder.Entity("VirtoCommerce.QuoteModule.Data.Model.QuoteItemEntity", b =>
                {
                    b.HasOne("VirtoCommerce.QuoteModule.Data.Model.QuoteRequestEntity", "QuoteRequest")
                        .WithMany("Items")
                        .HasForeignKey("QuoteRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("QuoteRequest");
                });

            modelBuilder.Entity("VirtoCommerce.QuoteModule.Data.Model.TierPriceEntity", b =>
                {
                    b.HasOne("VirtoCommerce.QuoteModule.Data.Model.QuoteItemEntity", "QuoteItem")
                        .WithMany("ProposalPrices")
                        .HasForeignKey("QuoteItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("QuoteItem");
                });

            modelBuilder.Entity("VirtoCommerce.QuoteModule.Data.Model.QuoteItemEntity", b =>
                {
                    b.Navigation("ProposalPrices");
                });

            modelBuilder.Entity("VirtoCommerce.QuoteModule.Data.Model.QuoteRequestEntity", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("Attachments");

                    b.Navigation("DynamicPropertyObjectValues");

                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
