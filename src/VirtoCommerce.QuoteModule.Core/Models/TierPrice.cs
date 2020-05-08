using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.QuoteModule.Core.Models
{
    public class TierPrice : ValueObject
    {
        public decimal Price { get; set; }
        public long Quantity { get; set; }
    }
}
