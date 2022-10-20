using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.ExperienceApiModule.Core.Infrastructure;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Queries;

public class QuotesQueryHandler : IQueryHandler<QuotesQuery, QuoteAggregateSearchResult>
{
    private readonly IQuoteRequestService _quoteRequestService;
    private readonly IQuoteAggregateRepository _quoteAggregateRepository;

    public QuotesQueryHandler(
        IQuoteRequestService quoteRequestService,
        IQuoteAggregateRepository quoteAggregateRepository)
    {
        _quoteRequestService = quoteRequestService;
        _quoteAggregateRepository = quoteAggregateRepository;
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

        return criteria;
    }
}
