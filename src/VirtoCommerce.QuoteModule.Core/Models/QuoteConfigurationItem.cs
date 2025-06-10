using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.QuoteModule.Core.Models;

public class QuoteConfigurationItem : AuditableEntity
{
    public string LineItemId { get; set; }

    public string ProductId { get; set; }

    public string Name { get; set; }

    public string Sku { get; set; }

    public int Quantity { get; set; }

    public string ImageUrl { get; set; }

    public string CatalogId { get; set; }

    public string CategoryId { get; set; }

    public string Type { get; set; }

    public string CustomText { get; set; }

    public IList<QuoteConfigurationItemFile> Files { get; set; }

    public object Clone()
    {
        var result = (QuoteConfigurationItem)MemberwiseClone();

        result.Files = Files?.Select(x => x.CloneTyped()).ToList();

        return result;
    }
}
