using VirtoCommerce.QuoteModule.Core.Models;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

public class QuoteTierPriceAggregate
{
    public TierPrice Model { get; set; }
    public QuoteAggregate Quote { get; set; }
}
