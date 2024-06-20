using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VirtoCommerce.Xapi.Core.Infrastructure;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Queries;

public class QuoteQueryHandler : IQueryHandler<QuoteQuery, QuoteAggregate>
{
    private readonly IQuoteAggregateRepository _quoteAggregateRepository;
    private readonly IMediator _mediator;

    public QuoteQueryHandler(IQuoteAggregateRepository quoteAggregateRepository, IMediator mediator)
    {
        _quoteAggregateRepository = quoteAggregateRepository;
        _mediator = mediator;
    }

    public async Task<QuoteAggregate> Handle(QuoteQuery request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(request.Id))
        {
            return await _quoteAggregateRepository.GetById(request.Id);
        }

        var quotesQuery = new QuotesQuery
        {
            Take = 1,
            UserId = request.UserId,
            StoreId = request.StoreId,
            CultureName = request.CultureName,
            CurrencyCode = request.CurrencyCode,
        };

        var searchResult = await _mediator.Send(quotesQuery, cancellationToken);

        return searchResult.Results.FirstOrDefault();
    }
}
