using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.ShippingModule.Core.Services;
using VirtoCommerce.TaxModule.Core.Model;
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
            var cartFromQuote = await _quoteConverter.ConvertToCartWithTax(quote);

            retVal.SubTotalExlTax = cartFromQuote.SubTotal;
            retVal.OriginalSubTotalExlTax = cartFromQuote.SubTotal + cartFromQuote.SubTotalDiscount;

            //Calculate shipment total
            retVal.ShippingTotal = cartFromQuote.ShippingTotal;

            //Calculate taxes
            retVal.TaxTotal = cartFromQuote.TaxTotal;

            //Calculate subtotal
            retVal.DiscountTotal = cartFromQuote.DiscountTotal;

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
