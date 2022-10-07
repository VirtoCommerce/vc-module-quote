using VirtoCommerce.CoreModule.Core.Tax;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

public class QuoteTaxDetailAggregate
{
    public TaxDetail Model { get; set; }
    public QuoteAggregate Quote { get; set; }
}
