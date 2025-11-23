using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.CoreModule.Core.Currency;
using VirtoCommerce.CoreModule.Core.Tax;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.QuoteModule.Core.Extensions;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.ShippingModule.Core.Model.Search;
using VirtoCommerce.ShippingModule.Core.Services;
using VirtoCommerce.TaxModule.Core.Model;
using VirtoCommerce.TaxModule.Core.Model.Search;
using VirtoCommerce.TaxModule.Core.Services;
using CartAddress = VirtoCommerce.CartModule.Core.Model.Address;
using CartConfigurationItem = VirtoCommerce.CartModule.Core.Model.ConfigurationItem;
using CartConfigurationItemFile = VirtoCommerce.CartModule.Core.Model.ConfigurationItemFile;
using CartLineItem = VirtoCommerce.CartModule.Core.Model.LineItem;
using QuoteAddress = VirtoCommerce.QuoteModule.Core.Models.Address;
using QuoteShipmentMethod = VirtoCommerce.QuoteModule.Core.Models.ShipmentMethod;

namespace VirtoCommerce.QuoteModule.Data.Services;

public class QuoteConverter : IQuoteConverter
{
    private const int _precision = 4;
    private const decimal _accuracy = 0.01m;

    private readonly ISettingsManager _settingsManager;
    private readonly ITaxProviderSearchService _taxProviderSearchService;
    private readonly IShippingMethodsSearchService _shippingMethodsSearchService;
    private readonly ICurrencyService _currencyService;

    public QuoteConverter(ISettingsManager settingsManager,
        ITaxProviderSearchService taxProviderSearchService,
        IShippingMethodsSearchService shippingMethodsSearchService,
        ICurrencyService currencyService)
    {
        _settingsManager = settingsManager;
        _taxProviderSearchService = taxProviderSearchService;
        _shippingMethodsSearchService = shippingMethodsSearchService;
        _currencyService = currencyService;
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
        result.OrganizationName = cart.OrganizationName;
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

        result.CreatedBy = quote.CreatedBy;
        result.CreatedDate = quote.CreatedDate;
        result.ModifiedBy = quote.ModifiedBy;
        result.ModifiedDate = quote.ModifiedDate;
        result.ChannelId = quote.ChannelId;
        result.IsAnonymous = quote.IsAnonymous;
        result.Status = quote.Status;
        result.Comment = quote.Comment;
        result.Coupon = quote.Coupon;
        result.Currency = quote.Currency;
        result.CustomerId = quote.CustomerId;
        result.CustomerName = quote.CustomerName;
        result.LanguageCode = quote.LanguageCode;
        result.OrganizationId = quote.OrganizationId;
        result.OrganizationName = quote.OrganizationName;
        result.StoreId = quote.StoreId;

        result.Items = quote.Items?.Convert(x => ToCartItems(x, quote));
        result.TaxDetails = quote.TaxDetails?.Convert(ToCartTaxDetails);
        result.DynamicProperties = quote.DynamicProperties?.Convert(ToDynamicProperties);

        result.Addresses = quote.Addresses?.Convert(ToCartAddresses);
        result.Shipments = quote.ShipmentMethod?.Convert(x => ToCartShipments(x, result, quote)).GetAwaiter().GetResult();
        result.Payments = ToCartPayments(quote, result.Addresses);

        return result;
    }

    public virtual async Task<ShoppingCart> ConvertToCartWithTax(QuoteRequest quote)
    {
        var result = ConvertToCart(quote);

        EvaluateDiscount(quote, result);
        var taxRates = await EvaluateTaxesAsync(result);
        ApplyTaxRates(result, taxRates);

        result.DiscountTotal = 0m;
        result.DiscountTotalWithTax = 0m;
        result.FeeTotal = result.Fee;
        result.TaxTotal = 0m;

        var cartItemsWithoutGifts = result.Items?.Where(x => !x.IsGift);

        if (cartItemsWithoutGifts != null)
        {
            result.SubTotal = cartItemsWithoutGifts.Sum(x => x.ListPrice * x.Quantity);
            result.SubTotalWithTax = cartItemsWithoutGifts.Sum(x => x.ListPriceWithTax * x.Quantity);
            result.SubTotalDiscount = cartItemsWithoutGifts.Sum(x => x.DiscountTotal);
            result.SubTotalDiscountWithTax = cartItemsWithoutGifts.Sum(x => x.DiscountTotalWithTax);
            result.DiscountTotal += cartItemsWithoutGifts.Sum(x => x.DiscountTotal);
            result.DiscountTotalWithTax += cartItemsWithoutGifts.Sum(x => x.DiscountTotalWithTax);
            result.FeeTotal += cartItemsWithoutGifts.Sum(x => x.Fee);
            result.FeeTotalWithTax += cartItemsWithoutGifts.Sum(x => x.FeeWithTax);
            result.TaxTotal += cartItemsWithoutGifts.Sum(x => x.TaxTotal);
        }

        if (result.Shipments != null)
        {
            result.ShippingTotal = result.Shipments.Sum(x => x.Total);
            result.ShippingTotalWithTax = result.Shipments.Sum(x => x.TotalWithTax);
            result.ShippingSubTotal = result.Shipments.Sum(x => x.Price);
            result.ShippingSubTotalWithTax = result.Shipments.Sum(x => x.PriceWithTax);
            result.ShippingDiscountTotal = result.Shipments.Sum(x => x.DiscountAmount);
            result.ShippingDiscountTotalWithTax = result.Shipments.Sum(x => x.DiscountAmountWithTax);
            result.DiscountTotal += result.Shipments.Sum(x => x.DiscountAmount);
            result.DiscountTotalWithTax += result.Shipments.Sum(x => x.DiscountAmountWithTax);
            result.FeeTotal += result.Shipments.Sum(x => x.Fee);
            result.FeeTotalWithTax += result.Shipments.Sum(x => x.FeeWithTax);
            result.TaxTotal += result.Shipments.Sum(x => x.TaxTotal);
        }

        if (result.Payments != null)
        {
            result.PaymentTotal = result.Payments.Sum(x => x.Total);
            result.PaymentTotalWithTax = result.Payments.Sum(x => x.TotalWithTax);
            result.PaymentSubTotal = result.Payments.Sum(x => x.Price);
            result.PaymentSubTotalWithTax = result.Payments.Sum(x => x.PriceWithTax);
            result.PaymentDiscountTotal = result.Payments.Sum(x => x.DiscountAmount);
            result.PaymentDiscountTotalWithTax = result.Payments.Sum(x => x.DiscountAmountWithTax);
            result.DiscountTotal += result.Payments.Sum(x => x.DiscountAmount);
            result.DiscountTotalWithTax += result.Payments.Sum(x => x.DiscountAmountWithTax);
            result.TaxTotal += result.Payments.Sum(x => x.TaxTotal);
        }

        var taxFactor = 1 + result.TaxPercentRate;
        result.FeeWithTax = result.Fee * taxFactor;
        result.FeeTotalWithTax = result.FeeTotal * taxFactor;
        result.DiscountTotal += result.DiscountAmount;
        result.DiscountTotalWithTax += result.DiscountAmount * taxFactor;
        //Subtract from cart tax total self discount tax amount
        result.TaxTotal -= result.DiscountAmount * result.TaxPercentRate;

        //Need to round all cart totals
        var currencies = await _currencyService.GetAllCurrenciesAsync();
        var currency = currencies.First(c => c.Code == result.Currency);
        result.SubTotal = currency.RoundingPolicy.RoundMoney(result.SubTotal, currency);
        result.SubTotalWithTax = currency.RoundingPolicy.RoundMoney(result.SubTotalWithTax, currency);
        result.SubTotalDiscount = currency.RoundingPolicy.RoundMoney(result.SubTotalDiscount, currency);
        result.SubTotalDiscountWithTax = currency.RoundingPolicy.RoundMoney(result.SubTotalDiscountWithTax, currency);
        result.TaxTotal = currency.RoundingPolicy.RoundMoney(result.TaxTotal, currency);
        result.DiscountTotal = currency.RoundingPolicy.RoundMoney(result.DiscountTotal, currency);
        result.DiscountTotalWithTax = currency.RoundingPolicy.RoundMoney(result.DiscountTotalWithTax, currency);
        result.Fee = currency.RoundingPolicy.RoundMoney(result.Fee, currency);
        result.FeeWithTax = currency.RoundingPolicy.RoundMoney(result.FeeWithTax, currency);
        result.FeeTotal = currency.RoundingPolicy.RoundMoney(result.FeeTotal, currency);
        result.FeeTotalWithTax = currency.RoundingPolicy.RoundMoney(result.FeeTotalWithTax, currency);
        result.ShippingTotal = currency.RoundingPolicy.RoundMoney(result.ShippingTotal, currency);
        result.ShippingTotalWithTax = currency.RoundingPolicy.RoundMoney(result.ShippingTotalWithTax, currency);
        result.ShippingSubTotal = currency.RoundingPolicy.RoundMoney(result.ShippingSubTotal, currency);
        result.ShippingSubTotalWithTax = currency.RoundingPolicy.RoundMoney(result.ShippingSubTotalWithTax, currency);
        result.PaymentTotal = currency.RoundingPolicy.RoundMoney(result.PaymentTotal, currency);
        result.PaymentTotalWithTax = currency.RoundingPolicy.RoundMoney(result.PaymentTotalWithTax, currency);
        result.PaymentSubTotal = currency.RoundingPolicy.RoundMoney(result.PaymentSubTotal, currency);
        result.PaymentSubTotalWithTax = currency.RoundingPolicy.RoundMoney(result.PaymentSubTotalWithTax, currency);
        result.PaymentDiscountTotal = currency.RoundingPolicy.RoundMoney(result.PaymentDiscountTotal, currency);
        result.PaymentDiscountTotalWithTax = currency.RoundingPolicy.RoundMoney(result.PaymentDiscountTotalWithTax, currency);

        result.Total = result.SubTotal + result.ShippingSubTotal + result.TaxTotal + result.PaymentSubTotal + result.FeeTotal - result.DiscountTotal;

        return result;
    }

    #region to cart taxes

    protected virtual async Task<IEnumerable<TaxRate>> EvaluateTaxesAsync(ShoppingCart cart)
    {
        var result = Enumerable.Empty<TaxRate>();
        var taxProvider = await GetActiveTaxProviderAsync(cart);
        if (taxProvider != null)
        {
            var taxEvalContext = CreateTaxEvalContext(cart);
            result = taxProvider.CalculateRates(taxEvalContext);
        }
        return result;
    }

    protected virtual TaxEvaluationContext CreateTaxEvalContext(ShoppingCart cart)
    {
        var taxEvalContext = AbstractTypeFactory<TaxEvaluationContext>.TryCreateInstance();
        taxEvalContext.StoreId = cart.StoreId;
        taxEvalContext.Code = cart.Name;
        taxEvalContext.Type = "Cart";
        taxEvalContext.CustomerId = cart.CustomerId;
        //map customer after PT-5425

        foreach (var lineItem in cart.Items)
        {
            taxEvalContext.Lines.Add(new TaxLine()
            {
                //PT-5339: Add Currency to tax line
                Id = lineItem.Id,
                Code = lineItem.Sku,
                Name = lineItem.Name,
                TaxType = lineItem.TaxType,
                //Special case when product have 100% discount and need to calculate tax for old value
                Amount = lineItem.ExtendedPrice > 0 ? lineItem.ExtendedPrice : lineItem.SalePrice,
                Quantity = lineItem.Quantity,
                Price = lineItem.PlacedPrice,
                TypeName = "item"
            });
        }

        foreach (var shipment in cart.Shipments ?? Array.Empty<Shipment>())
        {
            var totalTaxLine = new TaxLine
            {
                //PT-5339: Add Currency to tax line
                Id = shipment.Id,
                Code = shipment.ShipmentMethodCode,
                Name = shipment.ShipmentMethodOption,
                TaxType = shipment.TaxType,
                //Special case when shipment have 100% discount and need to calculate tax for old value
                Amount = shipment.Total > 0 ? shipment.Total : shipment.Price,
                TypeName = "shipment"
            };
            taxEvalContext.Lines.Add(totalTaxLine);

            if (shipment.DeliveryAddress != null)
            {
                taxEvalContext.Address = CreateTaxAddress(shipment.DeliveryAddress);
            }
        }

        foreach (var payment in cart.Payments ?? Array.Empty<Payment>())
        {
            var totalTaxLine = new TaxLine
            {
                //PT-5339: Add Currency to tax line
                Id = payment.Id,
                Code = payment.PaymentGatewayCode,
                Name = payment.PaymentGatewayCode,
                TaxType = payment.TaxType,
                //Special case when shipment have 100% discount and need to calculate tax for old value
                Amount = payment.Total > 0 ? payment.Total : payment.Price,
                TypeName = "payment"
            };
            taxEvalContext.Lines.Add(totalTaxLine);
        }
        return taxEvalContext;
    }

    protected virtual TaxModule.Core.Model.Address CreateTaxAddress(CartAddress address)
    {
        var result = AbstractTypeFactory<TaxModule.Core.Model.Address>.TryCreateInstance();
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

    protected virtual async Task<TaxProvider> GetActiveTaxProviderAsync(ShoppingCart cart)
    {
        var storeTaxProviders = await _taxProviderSearchService.SearchAsync(new TaxProviderSearchCriteria
        {
            StoreIds = new[] { cart.StoreId }
        });

        return storeTaxProviders?.Results.FirstOrDefault(x => x.IsActive);
    }

    protected void ApplyTaxRates(ShoppingCart cart, IEnumerable<TaxRate> taxRates)
    {
        cart.TaxPercentRate = 0m;
        foreach (var lineItem in cart.Items ?? Enumerable.Empty<LineItem>())
        {
            ApplyTaxRates(lineItem, taxRates);

            //Get percent rate from line item
            if (cart.TaxPercentRate == 0)
            {
                cart.TaxPercentRate = lineItem.TaxPercentRate;
            }
        }
        foreach (var shipment in cart.Shipments ?? Enumerable.Empty<Shipment>())
        {
            ApplyTaxRates(shipment, taxRates);
        }
        foreach (var payment in cart.Payments ?? Enumerable.Empty<Payment>())
        {
            ApplyTaxRates(payment, taxRates);
        }
    }

    protected virtual void ApplyTaxRates(CartLineItem lineItem, IEnumerable<TaxRate> taxRates)
    {
        lineItem.TaxPercentRate = 0m;
        var lineItemTaxRate = taxRates.FirstOrDefault(x => x.Line.Id != null && x.Line.Id.EqualsIgnoreCase(lineItem.Id ?? ""))
            ?? taxRates.FirstOrDefault(x => x.Line.Code != null && x.Line.Code.EqualsIgnoreCase(lineItem.Sku ?? ""));

        if (lineItemTaxRate == null)
        {
            return;
        }

        if (lineItemTaxRate.PercentRate > 0)
        {
            lineItem.TaxPercentRate = lineItemTaxRate.PercentRate;
        }
        else
        {
            var amount = lineItem.ExtendedPrice > 0 ? lineItem.ExtendedPrice : lineItem.SalePrice;
            if (amount > 0)
            {
                lineItem.TaxPercentRate = Math.Round(lineItemTaxRate.Rate / amount, _precision, MidpointRounding.AwayFromZero);
            }
        }

        lineItem.TaxDetails = lineItemTaxRate.TaxDetails;

        var taxFactor = 1 + lineItem.TaxPercentRate;
        lineItem.ListPriceWithTax = lineItem.ListPrice * taxFactor;
        lineItem.SalePriceWithTax = lineItem.SalePrice * taxFactor;
        lineItem.PlacedPrice = lineItem.ListPrice - lineItem.DiscountAmount;
        lineItem.PlacedPriceWithTax = lineItem.PlacedPrice * taxFactor;
        lineItem.ExtendedPrice = lineItem.PlacedPrice * lineItem.Quantity;
        lineItem.DiscountAmountWithTax = lineItem.DiscountAmount * taxFactor;
        lineItem.DiscountTotal = lineItem.DiscountAmount * Math.Max(1, lineItem.Quantity);
        lineItem.FeeWithTax = lineItem.Fee * taxFactor;
        lineItem.PlacedPriceWithTax = lineItem.PlacedPrice * taxFactor;
        lineItem.ExtendedPriceWithTax = lineItem.PlacedPriceWithTax * lineItem.Quantity;
        lineItem.DiscountTotalWithTax = lineItem.DiscountAmountWithTax * Math.Max(1, lineItem.Quantity);
        lineItem.TaxTotal = (lineItem.ExtendedPrice + lineItem.Fee) * lineItem.TaxPercentRate;
    }

    protected virtual void ApplyTaxRates(Payment payment, IEnumerable<TaxRate> taxRates)
    {
        payment.TaxPercentRate = 0m;
        var paymentTaxRate = taxRates.FirstOrDefault(x => x.Line.Id != null && x.Line.Id.EqualsIgnoreCase(payment.Id ?? ""))
            ?? taxRates.FirstOrDefault(x => x.Line.Code != null && x.Line.Code.EqualsIgnoreCase(payment.PaymentGatewayCode));

        if (paymentTaxRate == null)
        {
            return;
        }

        if (paymentTaxRate.PercentRate > 0)
        {
            payment.TaxPercentRate = paymentTaxRate.PercentRate;
        }
        else
        {
            var amount = payment.Total > 0 ? payment.Total : payment.Price;
            if (amount > 0)
            {
                payment.TaxPercentRate = Math.Round(paymentTaxRate.Rate / amount, _precision, MidpointRounding.AwayFromZero);
            }
        }

        payment.TaxDetails = paymentTaxRate.TaxDetails;
        var taxFactor = 1 + payment.TaxPercentRate;
        payment.Total = payment.Price - payment.DiscountAmount;
        payment.TotalWithTax = payment.Total * taxFactor;
        payment.PriceWithTax = payment.Price * taxFactor;
        payment.DiscountAmountWithTax = payment.DiscountAmount * taxFactor;
        payment.TaxTotal = payment.Total * payment.TaxPercentRate;
    }

    protected virtual void ApplyTaxRates(Shipment shipment, IEnumerable<TaxRate> taxRates)
    {
        shipment.TaxPercentRate = 0m;
        var shipmentTaxRate = taxRates.FirstOrDefault(x => x.Line.Id != null && x.Line.Id.EqualsIgnoreCase(shipment.Id ?? ""))
            ?? taxRates.FirstOrDefault(x => x.Line.Code != null && x.Line.Code.EqualsIgnoreCase(shipment.ShipmentMethodCode));

        if (shipmentTaxRate == null || shipmentTaxRate.Rate <= 0)
        {
            return;
        }

        if (shipmentTaxRate.PercentRate > 0)
        {
            shipment.TaxPercentRate = shipmentTaxRate.PercentRate;
        }
        else
        {
            var amount = shipment.Total > 0 ? shipment.Total : shipment.Price;
            if (amount > 0)
            {
                shipment.TaxPercentRate = Math.Round(shipmentTaxRate.Rate / amount, _precision, MidpointRounding.AwayFromZero);
            }
        }

        shipment.TaxDetails = shipmentTaxRate.TaxDetails;

        var taxFactor = 1 + shipment.TaxPercentRate;
        shipment.PriceWithTax = shipment.Price * taxFactor;
        shipment.DiscountAmountWithTax = shipment.DiscountAmount * taxFactor;
        shipment.FeeWithTax = shipment.Fee * taxFactor;
        shipment.Total = shipment.Price + shipment.Fee - shipment.DiscountAmount;
        shipment.TotalWithTax = shipment.PriceWithTax + shipment.FeeWithTax - shipment.DiscountAmountWithTax;
        shipment.TaxTotal = shipment.Total * shipment.TaxPercentRate;
    }

    #endregion

    #region to cart discounts

    protected virtual void EvaluateDiscount(QuoteRequest quote, ShoppingCart cart)
    {
        if (quote.Items != null)
        {
            var subTotal = quote.Items.Sum(x => x.SelectedTierPrice.Price * x.SelectedTierPrice.Quantity);
            var discount = 0m;
            if (quote.ManualSubTotal > 0)
            {
                discount = subTotal - quote.ManualSubTotal;
            }
            else if (quote.ManualRelDiscountAmount > 0)
            {
                discount = subTotal * quote.ManualRelDiscountAmount * _accuracy;
            }

            if (discount > 0)
            {
                cart.DiscountAmount = discount;
            }
        }
    }

    #endregion

    #region quote converters

    protected virtual Task<string> GetInitialQuoteStatus()
    {
        return _settingsManager.GetDefaultQuoteStatusAsync();
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
        result.Quantity = item.Quantity;
        result.IsConfigured = item.IsConfigured;

        result.ConfigurationItems = item.ConfigurationItems?.Convert(FromCartConfigurationItems);

        var tierPrice = AbstractTypeFactory<TierPrice>.TryCreateInstance();
        tierPrice.Price = item.SalePrice;
        tierPrice.Quantity = item.Quantity;

        result.ProposalPrices = new[] { tierPrice };

        return result;
    }

    protected virtual IList<QuoteConfigurationItem> FromCartConfigurationItems(ICollection<CartConfigurationItem> items)
    {
        return items
            .Select(FromCartConfigurationItem)
            .ToList();
    }

    protected virtual QuoteConfigurationItem FromCartConfigurationItem(CartConfigurationItem cartConfigurationItem)
    {
        var result = AbstractTypeFactory<QuoteConfigurationItem>.TryCreateInstance();

        result.ProductId = cartConfigurationItem.ProductId;
        result.Name = cartConfigurationItem.Name;
        result.Sku = cartConfigurationItem.Sku;
        result.Quantity = cartConfigurationItem.Quantity;
        result.ImageUrl = cartConfigurationItem.ImageUrl;
        result.CatalogId = cartConfigurationItem.CatalogId;
        result.CategoryId = cartConfigurationItem.CategoryId;
        result.Type = cartConfigurationItem.Type;
        result.CustomText = cartConfigurationItem.CustomText;

        result.Files = cartConfigurationItem.Files?.Convert(FromCartConfigurationItemFiles);

        return result;
    }

    protected virtual IList<QuoteConfigurationItemFile> FromCartConfigurationItemFiles(ICollection<CartConfigurationItemFile> files)
    {
        return files
            .Select(FromCartConfigurationItemFile)
            .ToList();
    }

    protected virtual QuoteConfigurationItemFile FromCartConfigurationItemFile(CartConfigurationItemFile file)
    {
        var result = AbstractTypeFactory<QuoteConfigurationItemFile>.TryCreateInstance();

        result.Url = file.Url;
        result.Name = file.Name;
        result.ContentType = file.ContentType;
        result.Size = file.Size;

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

    #endregion

    #region cart converters

    protected virtual IList<CartLineItem> ToCartItems(ICollection<QuoteItem> items, QuoteRequest quote)
    {
        return items
            .Select(x => ToCartItem(x, quote))
            .ToList();
    }

    protected virtual CartLineItem ToCartItem(QuoteItem item, QuoteRequest quote)
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
        result.ProductType = item.Product?.ProductType;
        result.SalePrice = item.SalePrice;
        result.Sku = item.Sku;
        result.TaxType = item.TaxType;
        result.Quantity = item.Quantity;
        result.TaxDetails = quote.TaxDetails;
        if (item.SelectedTierPrice != null)
        {
            result.SalePrice = item.SelectedTierPrice.Price;
            result.Quantity = (int)item.SelectedTierPrice.Quantity;
        }

        if (result.ListPrice < result.SalePrice)
        {
            result.ListPrice = result.SalePrice;
        }

        result.PlacedPrice = result.ListPrice;

        result.DiscountAmount = result.ListPrice - result.SalePrice;
        result.IsReadOnly = true;

        result.IsConfigured = item.IsConfigured;
        result.ConfigurationItems = item.ConfigurationItems?.Convert(ToCartConfigurationItems);

        // Workaround for the cart to order converter
        result.Id = Guid.NewGuid().ToString();

        return result;
    }

    protected virtual IList<CartConfigurationItem> ToCartConfigurationItems(ICollection<QuoteConfigurationItem> quoteConfigurationItems)
    {
        return quoteConfigurationItems
            .Select(ToCartConfigurationItem)
            .ToList();
    }

    protected virtual CartConfigurationItem ToCartConfigurationItem(QuoteConfigurationItem quoteConfigurationItem)
    {
        var result = AbstractTypeFactory<CartConfigurationItem>.TryCreateInstance();

        result.ProductId = quoteConfigurationItem.ProductId;
        result.Name = quoteConfigurationItem.Name;
        result.Sku = quoteConfigurationItem.Sku;
        result.Quantity = quoteConfigurationItem.Quantity;
        result.ImageUrl = quoteConfigurationItem.ImageUrl;
        result.CatalogId = quoteConfigurationItem.CatalogId;
        result.CategoryId = quoteConfigurationItem.CategoryId;
        result.Type = quoteConfigurationItem.Type;
        result.CustomText = quoteConfigurationItem.CustomText;

        result.Files = quoteConfigurationItem.Files?.Convert(ToCartConfigurationItemFiles);

        return result;
    }

    protected virtual IList<CartConfigurationItemFile> ToCartConfigurationItemFiles(IList<QuoteConfigurationItemFile> quoteConfigurationItemFiles)
    {
        return quoteConfigurationItemFiles
            .Select(ToCartConfigurationItemFile)
            .ToList();
    }

    protected virtual CartConfigurationItemFile ToCartConfigurationItemFile(QuoteConfigurationItemFile quoteConfigurationItemFile)
    {
        var result = AbstractTypeFactory<CartConfigurationItemFile>.TryCreateInstance();

        result.Url = quoteConfigurationItemFile.Url;
        result.Name = quoteConfigurationItemFile.Name;
        result.ContentType = quoteConfigurationItemFile.ContentType;
        result.Size = quoteConfigurationItemFile.Size;

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
        result.MiddleName = address.MiddleName;
        result.LastName = address.LastName;
        result.Organization = address.Organization;

        return result;
    }

    protected virtual ICollection<TaxDetail> ToCartTaxDetails(ICollection<TaxDetail> details)
    {
        return details;
    }

    protected virtual async Task<IList<Shipment>> ToCartShipments(QuoteShipmentMethod shipmentMethod, ShoppingCart cart, QuoteRequest quote)
    {
        var shipment = AbstractTypeFactory<Shipment>.TryCreateInstance();

        shipment.ShipmentMethodCode = shipmentMethod.ShipmentMethodCode;
        shipment.ShipmentMethodOption = shipmentMethod.OptionName;
        shipment.Currency = cart.Currency;
        shipment.Price = await GetShipmentPrice(quote, shipmentMethod, cart);
        shipment.DeliveryAddress = cart.Addresses.FirstOrDefault(x => x.AddressType.HasFlag(AddressType.Shipping));

        return new List<Shipment> { shipment };
    }

    protected virtual async Task<decimal> GetShipmentPrice(QuoteRequest quote, QuoteShipmentMethod shipmentMethod, ShoppingCart cart)
    {
        var result = quote.ManualShippingTotal;

        if (result == 0 && quote.ShipmentMethod != null)
        {
            //calculate total by using shipment gateways
            var evalContext = new ShippingEvaluationContext(cart);

            var searchCriteria = AbstractTypeFactory<ShippingMethodsSearchCriteria>.TryCreateInstance();
            searchCriteria.StoreId = quote.StoreId;
            searchCriteria.IsActive = true;
            searchCriteria.Codes = new[] { quote.ShipmentMethod.ShipmentMethodCode };
            var storeShippingMethods = await _shippingMethodsSearchService.SearchAsync(searchCriteria);
            var rate = storeShippingMethods.Results
                .SelectMany(x => x.CalculateRates(evalContext))
                .FirstOrDefault(x => (quote.ShipmentMethod.OptionName == null) || quote.ShipmentMethod.OptionName == x.OptionName);

            result = rate != null ? rate.Rate : 0m;
        }

        return result;
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

    #endregion
}
