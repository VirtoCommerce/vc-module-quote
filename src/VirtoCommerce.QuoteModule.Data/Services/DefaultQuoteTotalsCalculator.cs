using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.GenericCrud;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.ShippingModule.Core.Model;
using VirtoCommerce.ShippingModule.Core.Model.Search;
using VirtoCommerce.ShippingModule.Core.Services;
using VirtoCommerce.TaxModule.Core.Model;
using VirtoCommerce.TaxModule.Core.Model.Search;
using VirtoCommerce.TaxModule.Core.Services;

namespace VirtoCommerce.QuoteModule.Data.Services
{
    public class DefaultQuoteTotalsCalculator : IQuoteTotalsCalculator
    {
        private readonly ISearchService<ShippingMethodsSearchCriteria, ShippingMethodsSearchResult, ShippingMethod> _shippingMethodsSearchService;
        private readonly ISearchService<TaxProviderSearchCriteria, TaxProviderSearchResult, TaxProvider> _taxProviderSearchService;

        public DefaultQuoteTotalsCalculator(
            IShippingMethodsSearchService shippingMethodsSearchService,
            ITaxProviderSearchService taxProviderSearchService)
        {
            _shippingMethodsSearchService = (ISearchService<ShippingMethodsSearchCriteria, ShippingMethodsSearchResult, ShippingMethod>)shippingMethodsSearchService;
            _taxProviderSearchService = (ISearchService<TaxProviderSearchCriteria, TaxProviderSearchResult, TaxProvider>)taxProviderSearchService;
        }

        #region IQuoteTotalsCalculator Members
        public virtual async Task<QuoteRequestTotals> CalculateTotalsAsync(QuoteRequest quote)
        {
            var retVal = new QuoteRequestTotals();
            var cartFromQuote = quote.ToCartModel(AbstractTypeFactory<ShoppingCart>.TryCreateInstance());

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
            var taxProvider = (await _taxProviderSearchService.SearchAsync(taxSearchCriteria)).Results.FirstOrDefault();

            if (taxProvider != null)
            {
                var taxEvalContext = quote.ToTaxEvalContext(AbstractTypeFactory<TaxEvaluationContext>.TryCreateInstance());
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
        #endregion
    }
}
