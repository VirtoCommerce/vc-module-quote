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
        public void TierPrice_Types_Converting_ReturnsTypes()
        {
            //Arrange
            var tierPriceData = new dataModel.TierPriceEntity();
            var tierPriceDomain = new domainModel.TierPrice();
            var tierPriceWeb = new webModel.TierPrice();

            //Act
            var from1 = tierPriceData.FromModel(tierPriceDomain);
            //Assert
            Assert.IsType<dataModel.TierPriceEntity>(from1);

            //Act
            var to1 = tierPriceData.ToModel(tierPriceDomain);
            //Assert
            Assert.IsType<domainModel.TierPrice>(to1);

            //Act
            var toWeb1 = TierPriceConverter.ToWebModel(tierPriceDomain);
            //Assert
            Assert.IsType<webModel.TierPrice>(toWeb1);

            //Act
            tierPriceWeb = toWeb1;
            var toCore1 = TierPriceConverter.ToCoreModel(tierPriceWeb);
            //Assert
            Assert.IsType<domainModel.TierPrice>(toCore1);
        }

        [Fact]
        public void QuoteItem_Types_Converting_ReturnsTypes()
        {
            //Arrange
            var pkMap = new PrimaryKeyResolvingMap();
            //Testing transformation of QuoteItem types
            var quoteItemData = new dataModel.QuoteItemEntity();
            var quoteItemDomain = new domainModel.QuoteItem();
            var quoteItemWeb = new webModel.QuoteItem();

            //Act
            var to2 = quoteItemData.ToModel(quoteItemDomain);
            //Assert
            Assert.IsType<domainModel.QuoteItem>(to2);

            //Act
            var from2 = quoteItemData.FromModel(quoteItemDomain, pkMap);
            //Assert
            Assert.IsType<dataModel.QuoteItemEntity>(from2);

            //Act
            var toWeb2 = Web.Converters.QuoteItemConverter.ToWebModel(quoteItemDomain);
            //Assert
            Assert.IsType<webModel.QuoteItem>(toWeb2);

            //Act
            quoteItemWeb = toWeb2;
            var toCore2 = Web.Converters.QuoteItemConverter.ToCoreModel(quoteItemWeb);
            //Assert
            Assert.IsType<domainModel.QuoteItem>(toCore2);
        }

        [Fact]
        public void QuoteRequest_Types_Converting_ReturnsTypes()
        {
            //Arrange
            var pkMap = new PrimaryKeyResolvingMap();

            var quoteRequestData = new dataModel.QuoteRequestEntity();
            var quoteRequestDomain = new domainModel.QuoteRequest();
            var quoteRequestWeb = new webModel.QuoteRequest();

            //Act
            var to3 = quoteRequestData.ToModel(quoteRequestDomain);
            //Assert
            Assert.IsType<domainModel.QuoteRequest>(to3);

            var from3 = quoteRequestData.FromModel(quoteRequestDomain, pkMap);
            //Assert
            Assert.IsType<dataModel.QuoteRequestEntity>(from3);

            //Act
            var toWeb3 = Web.Converters.QuoteRequestConverter.ToWebModel(quoteRequestDomain);
            //Assert
            Assert.IsType<webModel.QuoteRequest>(toWeb3);

            //Act
            quoteRequestWeb = toWeb3;
            var toCore3 = Web.Converters.QuoteRequestConverter.ToCoreModel(quoteRequestWeb);
            //Assert
            Assert.IsType<domainModel.QuoteRequest>(toCore3);
        }

        [Fact]
        public void Attachment_Types_Converting_ReturnsTypes()
        {
            //Arrange
            var pkMap = new PrimaryKeyResolvingMap();

            var attachmentData = new dataModel.AttachmentEntity();
            var attachmentDomain = new domainModel.QuoteAttachment();
            var attachmentWeb = new webModel.QuoteAttachment();

            //Act
            var to4 = attachmentData.ToModel(attachmentDomain);
            //Assert
            Assert.IsType<domainModel.QuoteAttachment>(to4);

            //Act
            var from4 = attachmentData.FromModel(attachmentDomain, pkMap);
            //Assert
            Assert.IsType<dataModel.AttachmentEntity>(from4);

            //Act
            var toWeb4 = QuoteAttachmentConverter.ToWebModel(attachmentDomain);
            //Assert
            Assert.IsType<webModel.QuoteAttachment>(toWeb4);

            //Act
            attachmentWeb = toWeb4;
            var toCore4 = QuoteAttachmentConverter.ToCoreModel(attachmentWeb);
            //Assert
            Assert.IsType<domainModel.QuoteAttachment>(toCore4);
        }

        [Fact]
        public void Address_Types_Converting_ReturnsTypes()
        {
            //Arrange
            var addressData = new dataModel.AddressEntity();
            var addressDomain = new Address();
            var addressWeb = new webModel.Address();

            //Act
            var from5 = addressData.FromModel(addressDomain);
            //Assert
            Assert.IsType<dataModel.AddressEntity>(from5);

            //Act
            var to5 = addressData.ToModel(addressDomain);
            //Assert
            Assert.IsType<Address>(to5);

            //Act
            var toWeb5 = AddressConverter.ToWebModel(addressDomain);
            //Assert
            Assert.IsType<webModel.Address>(toWeb5);

            //Act
            addressWeb = toWeb5;
            var toCore5 = AddressConverter.ToCoreModel(addressWeb);
            //Assert
            Assert.IsType<Address>(toCore5);
        }

        [Fact]
        public void QuoteRequestShipmentMethod_Converting_ReturnsShipmentMethod()
        {
            //Arrange
            var pkMap = new PrimaryKeyResolvingMap();
            var quoteRequestData = new dataModel.QuoteRequestEntity();
            quoteRequestData.ShipmentMethodCode = "123123";
            quoteRequestData.ShipmentMethodOption = "TestName";

            var quoteRequestDomain = new domainModel.QuoteRequest();
            quoteRequestDomain.ShipmentMethod = new domainModel.ShipmentMethod();

            var quoteRequestWeb = new webModel.QuoteRequest();
            quoteRequestWeb.ShipmentMethod = new webModel.ShipmentMethod();

            //Act
            var to3 = quoteRequestData.ToModel(quoteRequestDomain);
            //Assert
            Assert.Equal("123123", to3.ShipmentMethod.ShipmentMethodCode);
            Assert.Equal("TestName", to3.ShipmentMethod.OptionName);

            //Act
            var from3 = quoteRequestData.FromModel(quoteRequestDomain, pkMap);
            //Assert
            Assert.Equal("123123", from3.ShipmentMethodCode);
            Assert.Equal("TestName", from3.ShipmentMethodOption);

            //Act
            var toWeb3 = Web.Converters.QuoteRequestConverter.ToWebModel(quoteRequestDomain);
            //Assert
            Assert.Equal("123123", toWeb3.ShipmentMethod.ShipmentMethodCode);
            Assert.Equal("TestName", toWeb3.ShipmentMethod.OptionName);

            //Act
            quoteRequestWeb = toWeb3;
            var toCore3 = Web.Converters.QuoteRequestConverter.ToCoreModel(quoteRequestWeb);
            //Assert
            Assert.Equal("123123", toCore3.ShipmentMethod.ShipmentMethodCode);
            Assert.Equal("TestName", toCore3.ShipmentMethod.OptionName);
        }

        [Fact]
        public void InheritFromAuditableEntity_ReturnsCreatedDate()
        {
            //Arrange
            var pkMap = new PrimaryKeyResolvingMap();

            //Testing transformation CreatedDate of QuoteRequest types
            var quoteRequestData = new dataModel.QuoteRequestEntity();
            quoteRequestData.CreatedDate = DateTime.Now.Date;

            var quoteRequestDomain = new domainModel.QuoteRequest();
            var quoteRequestWeb = new webModel.QuoteRequest();

            //Act
            var to3 = quoteRequestData.ToModel(quoteRequestDomain);
            //Assert
            Assert.Equal(DateTime.Now.Date, to3.CreatedDate);

            //Act
            var from3 = quoteRequestData.FromModel(quoteRequestDomain, pkMap);
            //Assert
            Assert.Equal(DateTime.Now.Date, from3.CreatedDate);

            //Act
            var toWeb3 = Web.Converters.QuoteRequestConverter.ToWebModel(quoteRequestDomain);
            //Assert
            Assert.Equal(DateTime.Now.Date, toWeb3.CreatedDate);

            //Act
            quoteRequestWeb = toWeb3;
            var toCore3 = Web.Converters.QuoteRequestConverter.ToCoreModel(quoteRequestWeb);
            //Assert
            Assert.Equal(DateTime.Now.Date, toCore3.CreatedDate);

            //Testing transformation CreatedDate of QuoteAttachment types
            //Arrange
            var attachmentData = new dataModel.AttachmentEntity();
            attachmentData.CreatedDate = DateTime.Now.Date;

            var attachmentDomain = new domainModel.QuoteAttachment();
            var attachmentWeb = new webModel.QuoteAttachment();

            //Act
            var to4 = attachmentData.ToModel(attachmentDomain);
            //Assert
            Assert.Equal(DateTime.Now.Date, to4.CreatedDate);

            //Act
            var from4 = attachmentData.FromModel(attachmentDomain, pkMap);
            //Assert
            Assert.Equal(DateTime.Now.Date, from4.CreatedDate);

            //Act
            var toWeb4 = QuoteAttachmentConverter.ToWebModel(attachmentDomain);
            //Assert
            Assert.Equal(DateTime.Now.Date, toWeb4.CreatedDate);

            //Act
            attachmentWeb = toWeb4;
            var toCore4 = QuoteAttachmentConverter.ToCoreModel(attachmentWeb);
            //Assert
            Assert.Equal(DateTime.Now.Date, toCore4.CreatedDate);
        }

        [Fact]
        public void TotalCalculator_ReturnsQuoteRequestTotals()
        {
            //Arrange
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
            //Act
            var sut = new DefaultQuoteTotalsCalculator(storeService.Object);
            var result = sut.CalculateTotals(quote);
            //Assert
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
