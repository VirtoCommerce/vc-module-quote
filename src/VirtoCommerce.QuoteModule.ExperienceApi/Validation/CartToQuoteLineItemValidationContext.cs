using System.Collections.Generic;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.XPurchase;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Validation
{
    public class CartToQuoteLineItemValidationContext
    {
        public LineItem LineItem { get; set; }
        public IEnumerable<CartProduct> AllCartProducts { get; set; }
    }
}
