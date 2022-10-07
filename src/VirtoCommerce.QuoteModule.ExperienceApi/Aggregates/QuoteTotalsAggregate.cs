using VirtoCommerce.QuoteModule.Core.Models;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

public class QuoteTotalsAggregate
{
    public QuoteRequestTotals Model { get; set; }
    public QuoteAggregate Quote { get; set; }
}
