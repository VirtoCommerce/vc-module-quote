using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.QuoteModule.Data.Model
{
    public class QuoteDynamicPropertyObjectValueEntity : DynamicPropertyObjectValueEntity
    {
        public virtual QuoteRequestEntity QuoteRequest { get; set; }
    }
}
