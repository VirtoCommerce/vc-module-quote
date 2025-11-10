using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.QuoteModule.Core.Models;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

public interface IQuoteAggregateRepository
{
    Task<QuoteAggregate> GetById(string id);

    [Obsolete("Use ToQuoteAggregates(IEnumerable<QuoteRequest> quotes, string cultureName) instead. CultureName argument can be null.", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    Task<IList<QuoteAggregate>> ToQuoteAggregates(IEnumerable<QuoteRequest> quotes);

    Task<IList<QuoteAggregate>> ToQuoteAggregates(IEnumerable<QuoteRequest> quotes, string cultureName);

    Task UpdateQuoteAttachmentsAsync(QuoteRequest quote, IList<string> urls);
}
