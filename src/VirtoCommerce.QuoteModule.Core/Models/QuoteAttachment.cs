using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.QuoteModule.Core.Models
{
    public class QuoteAttachment : AuditableEntity
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string MimeType { get; set; }
        public long Size { get; set; }
    }
}
