using System.Collections.Generic;
using VirtoCommerce.QuoteModule.Core.Models;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

public class QuoteItemAggregate
{
    public QuoteItem Model { get; set; }
    public QuoteAggregate Quote { get; set; }
    public QuoteTierPriceAggregate SelectedTierPrice { get; set; }
    public IList<QuoteTierPriceAggregate> ProposalPrices { get; set; }
}
