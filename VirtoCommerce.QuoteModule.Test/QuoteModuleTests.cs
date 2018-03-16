using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Moq;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Quote.Model;
using VirtoCommerce.Domain.Shipping.Model;
using VirtoCommerce.Domain.Store.Model;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Domain.Tax.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.QuoteModule.Data.Converters;
using VirtoCommerce.QuoteModule.Data.Services;
using VirtoCommerce.QuoteModule.Web.Converters;
using Xunit;
using dataModel = VirtoCommerce.QuoteModule.Data.Model;
using domainModel = VirtoCommerce.Domain.Quote.Model;
using webModel = VirtoCommerce.QuoteModule.Web.Model;

namespace VirtoCommerce.QuoteModule.Test
{
    [Trait("Category", "CI")]
    public class QuoteModuleTests
    {
        [Fact]
        public void TierPrice_Types_Converting()
        {
            var tierPriceData = new dataModel.TierPriceEntity();
            var tierPriceDomain = new domainModel.TierPrice();
            var tierPriceWeb = new webModel.TierPrice();

            var from1 = tierPriceData.FromModel(tierPriceDomain);
            Assert.IsType<dataModel.TierPriceEntity>(from1);

            var to1 = tierPriceData.ToModel(tierPriceDomain);
            Assert.IsType<domainModel.TierPrice>(to1);

            var toWeb1 = TierPriceConverter.ToWebModel(tierPriceDomain);
            Assert.IsType<webModel.TierPrice>(toWeb1);

            tierPriceWeb = toWeb1;
            var toCore1 = TierPriceConverter.ToCoreModel(tierPriceWeb);
            Assert.IsType<domainModel.TierPrice>(toCore1);
        }

        [Fact]
        public void QuoteItem_Types_Converting()
        {
            var pkMap = new PrimaryKeyResolvingMap();
            //Testing transformation of QuoteItem types
            var quoteItemData = new dataModel.QuoteItemEntity();
            var quoteItemDomain = new domainModel.QuoteItem();
            var quoteItemWeb = new webModel.QuoteItem();

            var to2 = quoteItemData.ToModel(quoteItemDomain);
            Assert.IsType<domainModel.QuoteItem>(to2);

            var from2 = quoteItemData.FromModel(quoteItemDomain, pkMap);
            Assert.IsType<dataModel.QuoteItemEntity>(from2);

            var toWeb2 = Web.Converters.QuoteItemConverter.ToWebModel(quoteItemDomain);
            Assert.IsType<webModel.QuoteItem>(toWeb2);

            quoteItemWeb = toWeb2;
            var toCore2 = Web.Converters.QuoteItemConverter.ToCoreModel(quoteItemWeb);
            Assert.IsType<domainModel.QuoteItem>(toCore2);
        }

        [Fact]
        public void QuoteRequest_Types_Converting()
        {
            var pkMap = new PrimaryKeyResolvingMap();

            var quoteRequestData = new dataModel.QuoteRequestEntity();
            var quoteRequestDomain = new domainModel.QuoteRequest();
            var quoteRequestWeb = new webModel.QuoteRequest();

            var to3 = quoteRequestData.ToModel(quoteRequestDomain);
            Assert.IsType<domainModel.QuoteRequest>(to3);

            var from3 = quoteRequestData.FromModel(quoteRequestDomain, pkMap);
            Assert.IsType<dataModel.QuoteRequestEntity>(from3);

            var toWeb3 = Web.Converters.QuoteRequestConverter.ToWebModel(quoteRequestDomain);
            Assert.IsType<webModel.QuoteRequest>(toWeb3);

            quoteRequestWeb = toWeb3;
            var toCore3 = Web.Converters.QuoteRequestConverter.ToCoreModel(quoteRequestWeb);
            Assert.IsType<domainModel.QuoteRequest>(toCore3);
        }

        [Fact]
        public void Attachment_Types_Converting()
        {
            var pkMap = new PrimaryKeyResolvingMap();

            var attachmentData = new dataModel.AttachmentEntity();
            var attachmentDomain = new domainModel.QuoteAttachment();
            var attachmentWeb = new webModel.QuoteAttachment();

            var to4 = attachmentData.ToModel(attachmentDomain);
            Assert.IsType<domainModel.QuoteAttachment>(to4);

            var from4 = attachmentData.FromModel(attachmentDomain, pkMap);
            Assert.IsType<dataModel.AttachmentEntity>(from4);

            var toWeb4 = QuoteAttachmentConverter.ToWebModel(attachmentDomain);
            Assert.IsType<webModel.QuoteAttachment>(toWeb4);

            attachmentWeb = toWeb4;
            var toCore4 = QuoteAttachmentConverter.ToCoreModel(attachmentWeb);
            Assert.IsType<domainModel.QuoteAttachment>(toCore4);
        }

        [Fact]
        public void Address_Types_Converting()
        {
            var addressData = new dataModel.AddressEntity();
            var addressDomain = new Address();
            var addressWeb = new webModel.Address();

            var from5 = addressData.FromModel(addressDomain);
            Assert.IsType<dataModel.AddressEntity>(from5);

            var to5 = addressData.ToModel(addressDomain);
            Assert.IsType<Address>(to5);

            var toWeb5 = AddressConverter.ToWebModel(addressDomain);
            Assert.IsType<webModel.Address>(toWeb5);

            addressWeb = toWeb5;
            var toCore5 = AddressConverter.ToCoreModel(addressWeb);
            Assert.IsType<Address>(toCore5);
        }

        [Fact]
        public void QuoteRequest_ShipmentMethod_Converting()
        {
            var pkMap = new PrimaryKeyResolvingMap();
            var quoteRequestData = new dataModel.QuoteRequestEntity();
            quoteRequestData.ShipmentMethodCode = "123123";
            quoteRequestData.ShipmentMethodOption = "TestName";

            var quoteRequestDomain = new domainModel.QuoteRequest();
            quoteRequestDomain.ShipmentMethod = new domainModel.ShipmentMethod();

            var quoteRequestWeb = new webModel.QuoteRequest();
            quoteRequestWeb.ShipmentMethod = new webModel.ShipmentMethod();

            var to3 = quoteRequestData.ToModel(quoteRequestDomain);
            Assert.Equal("123123", to3.ShipmentMethod.ShipmentMethodCode);
            Assert.Equal("TestName", to3.ShipmentMethod.OptionName);

            var from3 = quoteRequestData.FromModel(quoteRequestDomain, pkMap);
            Assert.Equal("123123", from3.ShipmentMethodCode);
            Assert.Equal("TestName", from3.ShipmentMethodOption);

            var toWeb3 = Web.Converters.QuoteRequestConverter.ToWebModel(quoteRequestDomain);
            Assert.Equal("123123", toWeb3.ShipmentMethod.ShipmentMethodCode);
            Assert.Equal("TestName", toWeb3.ShipmentMethod.OptionName);

            quoteRequestWeb = toWeb3;
            var toCore3 = Web.Converters.QuoteRequestConverter.ToCoreModel(quoteRequestWeb);
            Assert.Equal("123123", toCore3.ShipmentMethod.ShipmentMethodCode);
            Assert.Equal("TestName", toCore3.ShipmentMethod.OptionName);
        }

        [Fact]
        public void InheritFromAuditableEntity()
        {
            var pkMap = new PrimaryKeyResolvingMap();

            //Testing transformation CreatedDate of QuoteRequest types
            var quoteRequestData = new dataModel.QuoteRequestEntity();
            quoteRequestData.CreatedDate = DateTime.Now.Date;

            var quoteRequestDomain = new domainModel.QuoteRequest();
            var quoteRequestWeb = new webModel.QuoteRequest();

            var to3 = quoteRequestData.ToModel(quoteRequestDomain);
            Assert.Equal(DateTime.Now.Date, to3.CreatedDate);

            var from3 = quoteRequestData.FromModel(quoteRequestDomain, pkMap);
            Assert.Equal(DateTime.Now.Date, from3.CreatedDate);

            var toWeb3 = Web.Converters.QuoteRequestConverter.ToWebModel(quoteRequestDomain);
            Assert.Equal(DateTime.Now.Date, toWeb3.CreatedDate);

            quoteRequestWeb = toWeb3;
            var toCore3 = Web.Converters.QuoteRequestConverter.ToCoreModel(quoteRequestWeb);
            Assert.Equal(DateTime.Now.Date, toCore3.CreatedDate);

            //Testing transformation CreatedDate of QuoteAttachment types
            var attachmentData = new dataModel.AttachmentEntity();
            attachmentData.CreatedDate = DateTime.Now.Date;

            var attachmentDomain = new domainModel.QuoteAttachment();
            var attachmentWeb = new webModel.QuoteAttachment();

            var to4 = attachmentData.ToModel(attachmentDomain);
            Assert.Equal(DateTime.Now.Date, to4.CreatedDate);

            var from4 = attachmentData.FromModel(attachmentDomain, pkMap);
            Assert.Equal(DateTime.Now.Date, from4.CreatedDate);

            var toWeb4 = QuoteAttachmentConverter.ToWebModel(attachmentDomain);
            Assert.Equal(DateTime.Now.Date, toWeb4.CreatedDate);

            attachmentWeb = toWeb4;
            var toCore4 = QuoteAttachmentConverter.ToCoreModel(attachmentWeb);
            Assert.Equal(DateTime.Now.Date, toCore4.CreatedDate);
        }

        [Fact]
        public void TotalCalculator()
        {
            var quote = Quote;
            var cartFromQuote = quote.ToCartModel();
            var store = TestStore;
            var storeService = new Mock<IStoreService>();
            storeService.Setup(x => x.GetById(It.Is<string>(id => id.Equals(quote.StoreId)))).Returns(store);
            var retVal = new QuoteRequestTotals();
            retVal.ShippingTotal = quote.ManualShippingTotal;
            var items = quote.Items.Where(x => x.SelectedTierPrice != null);
            if (quote.Items != null)
            {
                retVal.OriginalSubTotalExlTax = items.Sum(x => x.SalePrice * x.SelectedTierPrice.Quantity);
                retVal.SubTotalExlTax = items.Sum(x => x.SelectedTierPrice.Price * x.SelectedTierPrice.Quantity);
                if (quote.ManualSubTotal > 0)
                {
                    retVal.DiscountTotal = retVal.SubTotalExlTax - quote.ManualSubTotal;
                    retVal.SubTotalExlTax = quote.ManualSubTotal;
                }
                else if (quote.ManualRelDiscountAmount > 0)
                {
                    retVal.DiscountTotal = retVal.SubTotalExlTax * quote.ManualRelDiscountAmount * 0.01m;
                }
            }
            var sut = new DefaultQuoteTotalsCalculator(storeService.Object);
            var result = sut.CalculateTotals(quote);
            Assert.Equal(result.GetType(), retVal.GetType());
            var a = Serialize(result);
            var b = Serialize(retVal);
            Assert.Equal(a, b);
        }

        private static QuoteRequest Quote
        {
            get
            {
                return new domainModel.QuoteRequest {
                    Id = "quot-1",
                    StoreId = "store-1",
                    Items = new List<QuoteItem>
                    {
                        new QuoteItem
                        {
                           SelectedTierPrice = new TierPrice
                           {
                             Price = 3285,
                             Quantity = 3
                           },
                           ProposalPrices = new List<TierPrice>
                           {
                               new TierPrice
                               {
                                 Price = 3295,
                                 Quantity = 1
                               },
                               new TierPrice
                               {
                                 Price = 3290,
                                 Quantity = 2
                               },
                               new TierPrice
                               {
                                 Price = 3285,
                                 Quantity = 3
                               }
                           },
                           ListPrice = 3295,
                           SalePrice = 3295                      
                        }                               
                    },
                    ManualShippingTotal = 15,
                    ShipmentMethod = new ShipmentMethod
                    {
                        ShipmentMethodCode = "test-code",
                        OptionName = "test-name",
                        Currency = "USD",
                        Price = 10
                    },
                    ManualSubTotal = 10,
                    ManualRelDiscountAmount = 0
                };
            }
        }

        private static Store TestStore
        {
            get {
                return new Store {
                    Id = "store-1",
                    ShippingMethods = new List<ShippingMethod> (){ },
                    TaxProviders = new List<TaxProvider> { }
                };
            }
        }

        public static string Serialize<T>(T obj)
        {
            if (obj == null)
                return string.Empty;

            try
            {
                var stringWriter = new StringWriter();
                var xmlWriter = XmlWriter.Create(stringWriter);
                var xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(xmlWriter, obj);
                var serializeXml = stringWriter.ToString();
                xmlWriter.Close();
                return serializeXml;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
