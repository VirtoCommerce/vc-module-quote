using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.QuoteModule.Core.Models
{
    public class ShipmentMethod : ValueObject
    {
        public string ShipmentMethodCode { get; set; }
        public string OptionName { get; set; }
        public string LogoUrl { get; set; }
        public string Currency { get; set; }
        public decimal Price { get; set; }
    }
}
