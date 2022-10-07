using System.Collections.Generic;
using VirtoCommerce.CoreModule.Core.Currency;
using VirtoCommerce.QuoteModule.Core.Models;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

public class QuoteAggregate
{
    public QuoteRequest Model { get; set; }
    public Currency Currency { get; set; }
    public QuoteTotalsAggregate Totals { get; set; }
    public IList<QuoteItemAggregate> Items { get; set; }
    public QuoteShipmentMethodAggregate ShipmentMethod { get; set; }
    public IList<QuoteTaxDetailAggregate> TaxDetails { get; set; }
}
