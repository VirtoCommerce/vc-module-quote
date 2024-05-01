using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.ShippingModule.Core.Model.Search;
using VirtoCommerce.ShippingModule.Core.Services;
using VirtoCommerce.TaxModule.Core.Model;
using VirtoCommerce.TaxModule.Core.Model.Search;
using VirtoCommerce.TaxModule.Core.Services;

using TaxAddress = VirtoCommerce.TaxModule.Core.Model.Address;

namespace VirtoCommerce.QuoteModule.Data.Services
{
    public class DefaultQuoteTotalsCalculator : IQuoteTotalsCalculator
    {
        private readonly IShippingMethodsSearchService _shippingMethodsSearchService;
        private readonly ITaxProviderSearchService _taxProviderSearchService;
        private readonly IQuoteConverter _quoteConverter;

        public DefaultQuoteTotalsCalculator(
            IShippingMethodsSearchService shippingMethodsSearchService,
            ITaxProviderSearchService taxProviderSearchService,
            IQuoteConverter quoteConverter)
        {
            _shippingMethodsSearchService = shippingMethodsSearchService;
            _taxProviderSearchService = taxProviderSearchService;
            _quoteConverter = quoteConverter;
        }

        public virtual async Task<QuoteRequestTotals> CalculateTotalsAsync(QuoteRequest quote)
        {
            var retVal = new QuoteRequestTotals();
            var cartFromQuote = _quoteConverter.ConvertToCart(quote);

            //Calculate shipment total
            //first try to get manual amount
            retVal.ShippingTotal = quote.ManualShippingTotal;
            if (retVal.ShippingTotal == 0 && quote.ShipmentMethod != null)
            {
                //calculate total by using shipment gateways
                var evalContext = new ShippingEvaluationContext(cartFromQuote);

                var searchCriteria = AbstractTypeFactory<ShippingMethodsSearchCriteria>.TryCreateInstance();
                searchCriteria.StoreId = quote.StoreId;
                searchCriteria.IsActive = true;
                searchCriteria.Codes = new[] { quote.ShipmentMethod.ShipmentMethodCode };
                var storeShippingMethods = await _shippingMethodsSearchService.SearchAsync(searchCriteria);
                var rate = storeShippingMethods.Results
                    .SelectMany(x => x.CalculateRates(evalContext))
                    .FirstOrDefault(x => (quote.ShipmentMethod.OptionName == null) || quote.ShipmentMethod.OptionName == x.OptionName);

                retVal.ShippingTotal = rate != null ? rate.Rate : 0m;
            }

            //Calculate taxes
            var taxSearchCriteria = AbstractTypeFactory<TaxProviderSearchCriteria>.TryCreateInstance();
            taxSearchCriteria.StoreIds = new[] { quote.StoreId };
            taxSearchCriteria.Sort = nameof(TaxProvider.Priority);
            var taxProvider = (await _taxProviderSearchService.SearchAsync(taxSearchCriteria))
                .Results.FirstOrDefault(x => x.IsActive);

            if (taxProvider != null)
            {
                var taxEvalContext = ToTaxEvalContext(quote);
                retVal.TaxTotal = taxProvider.CalculateRates(taxEvalContext).Select(x => x.Rate).DefaultIfEmpty(0).Sum(x => x);
            }

            //Calculate subtotal
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

            return retVal;
        }

        protected virtual TaxEvaluationContext ToTaxEvalContext(QuoteRequest quote)
        {
            var target = AbstractTypeFactory<TaxEvaluationContext>.TryCreateInstance();
            target.Id = quote.Id;
            target.Code = quote.Number;
            target.Currency = quote.Currency;
            target.Address = quote.Addresses?.FirstOrDefault()?.ToTaxModel(AbstractTypeFactory<TaxAddress>.TryCreateInstance());
            target.Type = quote.GetType().Name;
            foreach (var quoteItem in quote.Items)
            {
                var line = new TaxLine
                {
                    Id = quoteItem.Id,
                    Code = quoteItem.Sku,
                    Name = quoteItem.Name,
                    TaxType = quoteItem.TaxType,
                    Amount = quoteItem.SelectedTierPrice.Price * quoteItem.SelectedTierPrice.Quantity
                };
                target.Lines.Add(line);
            }
            return target;
        }
    }
}
