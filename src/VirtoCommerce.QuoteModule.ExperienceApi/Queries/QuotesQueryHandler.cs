using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Xapi.Core.Index;
using VirtoCommerce.Xapi.Core.Infrastructure;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using VirtoCommerce.SearchModule.Core.Model;
using VirtoCommerce.SearchModule.Core.Services;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Queries;

public class QuotesQueryHandler : IQueryHandler<QuotesQuery, QuoteAggregateSearchResult>
{
    private readonly IQuoteRequestService _quoteRequestService;
    private readonly IQuoteAggregateRepository _quoteAggregateRepository;
    private readonly ISearchPhraseParser _phraseParser;

    public QuotesQueryHandler(
        IQuoteRequestService quoteRequestService,
        IQuoteAggregateRepository quoteAggregateRepository,
        ISearchPhraseParser phraseParser)
    {
        _quoteRequestService = quoteRequestService;
        _quoteAggregateRepository = quoteAggregateRepository;
        _phraseParser = phraseParser;
    }

    public virtual async Task<QuoteAggregateSearchResult> Handle(QuotesQuery request, CancellationToken cancellationToken)
    {
        var criteria = GetSearchCriteria(request);
        var searchResult = await _quoteRequestService.SearchAsync(criteria);

        var result = AbstractTypeFactory<QuoteAggregateSearchResult>.TryCreateInstance();
        result.TotalCount = searchResult.TotalCount;
        result.Results = await _quoteAggregateRepository.ToQuoteAggregates(searchResult.Results);

        return result;
    }

    protected virtual QuoteRequestSearchCriteria GetSearchCriteria(QuotesQuery request)
    {
        var criteria = request.GetSearchCriteria<QuoteRequestSearchCriteria>();
        criteria.StoreId = request.StoreId;
        criteria.CustomerId = request.UserId;
        criteria.Currency = request.CurrencyCode;
        criteria.LanguageCode = request.CultureName;

        // parse Filter argument
        if (string.IsNullOrEmpty(request.Filter))
        {
            return criteria;
        }

        var parseResult = _phraseParser.Parse(request.Filter);

        // search keyword only in number
        criteria.NumberKeyword = parseResult.Keyword;

        foreach (var term in parseResult.Filters.OfType<TermFilter>())
        {
            term.MapTo(criteria);
        }

        // custom CreatedDate filter
        var createdDateRange = parseResult.Filters.OfType<RangeFilter>().FirstOrDefault(x => x.FieldName.EqualsInvariant("CreatedDate"));
        var range = createdDateRange?.Values?.FirstOrDefault();
        if (range != null)
        {
            if (DateTime.TryParse(range.Lower, out var startDate))
            {
                criteria.StartDate = startDate;
            }

            if (DateTime.TryParse(range.Upper, out var endDate))
            {
                criteria.EndDate = endDate;
            }
        }

        return criteria;
    }
}
