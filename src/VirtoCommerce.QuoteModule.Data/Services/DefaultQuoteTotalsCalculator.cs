using System.Threading.Tasks;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;

namespace VirtoCommerce.QuoteModule.Data.Services
{
    public class DefaultQuoteTotalsCalculator : IQuoteTotalsCalculator
    {
        private readonly IQuoteConverter _quoteConverter;

        public DefaultQuoteTotalsCalculator(IQuoteConverter quoteConverter)
        {
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
    }
}
