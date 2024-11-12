using System.Collections.Generic;
using VirtoCommerce.Xapi.Core.Models;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Models;

public class NewQuoteItem
{
    public string ProductId { get; set; }

    public string Name { get; set; }

    public int Quantity { get; set; }

    public decimal? Price { get; set; }

    public string Comment { get; set; }

    public IList<DynamicPropertyValue> DynamicProperties { get; set; }
}
