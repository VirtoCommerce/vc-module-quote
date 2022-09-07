using VirtoCommerce.QuoteModule.Core.Models;

namespace VirtoCommerce.QuoteExtensionsModule.Web.Models
{
    public class QuoteRequestExt : QuoteRequest
    {
        public decimal? ShippingCost { get; set; }
    }
}
