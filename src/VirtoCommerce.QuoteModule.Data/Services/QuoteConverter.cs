using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.CoreModule.Core.Tax;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.QuoteModule.Core.Extensions;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;
using CartAddress = VirtoCommerce.CartModule.Core.Model.Address;
using CartLineItem = VirtoCommerce.CartModule.Core.Model.LineItem;
using QuoteAddress = VirtoCommerce.QuoteModule.Core.Models.Address;
using QuoteSettings = VirtoCommerce.QuoteModule.Core.ModuleConstants.Settings.General;
using QuoteShipmentMethod = VirtoCommerce.QuoteModule.Core.Models.ShipmentMethod;

namespace VirtoCommerce.QuoteModule.Data.Services;

public class QuoteConverter : IQuoteConverter
{
    private readonly ISettingsManager _settingsManager;

    public QuoteConverter(ISettingsManager settingsManager)
    {
        _settingsManager = settingsManager;
    }

    public virtual async Task<QuoteRequest> ConvertFromCart(ShoppingCart cart)
    {
        var result = AbstractTypeFactory<QuoteRequest>.TryCreateInstance();

        result.Comment = cart.Comment;
        result.Coupon = cart.Coupon;
        result.Currency = cart.Currency;
        result.CustomerId = cart.CustomerId;
        result.CustomerName = cart.CustomerName;
        result.LanguageCode = cart.LanguageCode;
        result.OrganizationId = cart.OrganizationId;
        result.Status = await GetInitialQuoteStatus();
        result.StoreId = cart.StoreId;

        result.Items = cart.Items?.Convert(FromCartItems);
        result.TaxDetails = cart.TaxDetails?.Convert(FromCartTaxDetails);
        result.DynamicProperties = cart.DynamicProperties?.Convert(ToDynamicProperties);

        result.Addresses = FromCartAddresses(cart);
        result.ShipmentMethod = cart.Shipments?.FirstOrDefault()?.Convert(FromCartShipment);

        return result;
    }

    public ShoppingCart ConvertToCart(QuoteRequest quote)
    {
        var result = AbstractTypeFactory<ShoppingCart>.TryCreateInstance();

        result.Comment = quote.Comment;
        result.Coupon = quote.Coupon;
        result.Currency = quote.Currency;
        result.CustomerId = quote.CustomerId;
        result.CustomerName = quote.CustomerName;
        result.LanguageCode = quote.LanguageCode;
        result.OrganizationId = quote.OrganizationId;
        result.StoreId = quote.StoreId;

        result.Items = quote.Items?.Convert(ToCartItems);
        result.TaxDetails = quote.TaxDetails?.Convert(ToCartTaxDetails);
        result.DynamicProperties = quote.DynamicProperties?.Convert(ToDynamicProperties);

        result.Addresses = quote.Addresses?.Convert(ToCartAddresses);
        result.Shipments = quote.ShipmentMethod?.Convert(x => ToCartShipments(x, result));
        result.Payments = ToCartPayments(quote, result.Addresses);

        return result;
    }


    protected virtual Task<string> GetInitialQuoteStatus()
    {
        return _settingsManager.GetValueByDescriptorAsync<string>(QuoteSettings.DefaultStatus);
    }

    protected virtual IList<QuoteItem> FromCartItems(ICollection<CartLineItem> items)
    {
        return items
            .Where(x => !x.IsRejected)
            .Select(FromCartItem)
            .ToList();
    }

    protected virtual QuoteItem FromCartItem(CartLineItem item)
    {
        var result = AbstractTypeFactory<QuoteItem>.TryCreateInstance();

        result.CatalogId = item.CatalogId;
        result.CategoryId = item.CategoryId;
        result.Comment = item.Note;
        result.Currency = item.Currency;
        result.ImageUrl = item.ImageUrl;
        result.ListPrice = item.ListPrice;
        result.Name = item.Name;
        result.ProductId = item.ProductId;
        result.SalePrice = item.SalePrice;
        result.Sku = item.Sku;
        result.TaxType = item.TaxType;

        var tierPrice = AbstractTypeFactory<TierPrice>.TryCreateInstance();
        tierPrice.Price = item.SalePrice;
        tierPrice.Quantity = item.Quantity;

        result.ProposalPrices = new[] { tierPrice };

        return result;
    }

    protected virtual IList<QuoteAddress> FromCartAddresses(ShoppingCart cart)
    {
        return FromCartAddresses(cart.Addresses)
            .Concat(FromCartAddresses(cart.Shipments?.Select(x => x.DeliveryAddress), AddressType.Shipping))
            .Concat(FromCartAddresses(cart.Payments?.Select(x => x.BillingAddress), AddressType.Billing))
            .Distinct()
            .ToList();
    }

    protected virtual IEnumerable<QuoteAddress> FromCartAddresses(IEnumerable<CartAddress> addresses, AddressType? type = null)
    {
        if (addresses != null)
        {
            foreach (var address in addresses.Where(x => x != null))
            {
                yield return FromCartAddress(address, type);
            }
        }
    }

    protected virtual QuoteAddress FromCartAddress(CartAddress address, AddressType? type)
    {
        var result = AbstractTypeFactory<QuoteAddress>.TryCreateInstance();

        result.Name = address.Name;
        result.OuterId = address.OuterId;
        result.AddressType = address.AddressType;
        result.CountryCode = address.CountryCode;
        result.CountryName = address.CountryName;
        result.PostalCode = address.PostalCode;
        result.RegionId = address.RegionId;
        result.RegionName = address.RegionName;
        result.City = address.City;
        result.Line1 = address.Line1;
        result.Line2 = address.Line2;
        result.Email = address.Email;
        result.Phone = address.Phone;
        result.FirstName = address.FirstName;
        result.LastName = address.LastName;
        result.Organization = address.Organization;


        if (type != null && !result.AddressType.HasFlag(type.Value))
        {
            result.AddressType = type.Value;
        }

        return result;
    }

    protected virtual ICollection<TaxDetail> FromCartTaxDetails(ICollection<TaxDetail> details)
    {
        return details;
    }

    protected virtual QuoteShipmentMethod FromCartShipment(Shipment shipment)
    {
        var result = AbstractTypeFactory<QuoteShipmentMethod>.TryCreateInstance();

        result.ShipmentMethodCode = shipment.ShipmentMethodCode;
        result.OptionName = shipment.ShipmentMethodOption;

        return result;
    }

    protected virtual IList<CartLineItem> ToCartItems(ICollection<QuoteItem> items)
    {
        return items
            .Select(ToCartItem)
            .ToList();
    }

    protected virtual CartLineItem ToCartItem(QuoteItem item)
    {
        var result = AbstractTypeFactory<CartLineItem>.TryCreateInstance();

        result.CatalogId = item.CatalogId;
        result.CategoryId = item.CategoryId;
        result.Currency = item.Currency;
        result.ImageUrl = item.ImageUrl;
        result.ListPrice = item.ListPrice;
        result.Name = item.Name;
        result.Note = item.Comment;
        result.ProductId = item.ProductId;
        result.SalePrice = item.SalePrice;
        result.Sku = item.Sku;
        result.TaxType = item.TaxType;

        if (item.SelectedTierPrice != null)
        {
            result.SalePrice = item.SelectedTierPrice.Price;
            result.Quantity = (int)item.SelectedTierPrice.Quantity;
        }

        if (result.ListPrice < result.SalePrice)
        {
            result.ListPrice = result.SalePrice;
        }

        result.DiscountAmount = result.ListPrice - result.SalePrice;
        result.IsReadOnly = true;

        // Workaround for the cart to order converter
        result.Id = Guid.NewGuid().ToString();

        return result;
    }

    protected virtual IList<CartAddress> ToCartAddresses(ICollection<QuoteAddress> addresses)
    {
        return addresses
            .Select(ToCartAddress)
            .Distinct()
            .ToList();
    }

    protected virtual CartAddress ToCartAddress(QuoteAddress address)
    {
        var result = AbstractTypeFactory<CartAddress>.TryCreateInstance();

        result.Name = address.Name;
        result.OuterId = address.OuterId;
        result.AddressType = address.AddressType;
        result.CountryCode = address.CountryCode;
        result.CountryName = address.CountryName;
        result.PostalCode = address.PostalCode;
        result.RegionId = address.RegionId;
        result.RegionName = address.RegionName;
        result.City = address.City;
        result.Line1 = address.Line1;
        result.Line2 = address.Line2;
        result.Email = address.Email;
        result.Phone = address.Phone;
        result.FirstName = address.FirstName;
        result.LastName = address.LastName;
        result.Organization = address.Organization;

        return result;
    }

    protected virtual ICollection<TaxDetail> ToCartTaxDetails(ICollection<TaxDetail> details)
    {
        return details;
    }

    protected virtual IList<Shipment> ToCartShipments(QuoteShipmentMethod shipmentMethod, ShoppingCart cart)
    {
        var shipment = AbstractTypeFactory<Shipment>.TryCreateInstance();

        shipment.ShipmentMethodCode = shipmentMethod.ShipmentMethodCode;
        shipment.ShipmentMethodOption = shipmentMethod.OptionName;
        shipment.Currency = cart.Currency;
        shipment.DeliveryAddress = cart.Addresses.FirstOrDefault(x => x.AddressType.HasFlag(AddressType.Shipping));

        return new List<Shipment> { shipment };
    }

    protected virtual IList<Payment> ToCartPayments(QuoteRequest quote, ICollection<CartAddress> addresses)
    {
        var payment = AbstractTypeFactory<Payment>.TryCreateInstance();

        payment.Currency = quote.Currency;
        payment.Amount = quote.Totals?.GrandTotalInclTax ?? 0m;
        payment.BillingAddress = addresses.FirstOrDefault(x => x.AddressType.HasFlag(AddressType.Billing));

        return new List<Payment> { payment };
    }

    protected virtual IList<DynamicObjectProperty> ToDynamicProperties(ICollection<DynamicObjectProperty> properties)
    {
        return properties
            .Select(ToDynamicProperty)
            .ToList();
    }

    protected virtual DynamicObjectProperty ToDynamicProperty(DynamicObjectProperty property)
    {
        var result = AbstractTypeFactory<DynamicObjectProperty>.TryCreateInstance();

        result.Name = property.Name;
        result.IsDictionary = property.IsDictionary;
        result.ValueType = property.ValueType;
        result.Values = property.Values;

        return result;
    }
}
