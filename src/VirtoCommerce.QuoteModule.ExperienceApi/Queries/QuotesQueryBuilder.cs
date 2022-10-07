using System.Threading.Tasks;
using GraphQL;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.ExperienceApiModule.Core.BaseQueries;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using VirtoCommerce.QuoteModule.ExperienceApi.Authorization;
using VirtoCommerce.QuoteModule.ExperienceApi.Schemas;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Queries;

public class QuotesQueryBuilder : SearchQueryBuilder<QuotesQuery, QuoteAggregateSearchResult, QuoteAggregate, QuoteType>
{
    protected override string Name => "quotes";

    public QuotesQueryBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
    }

    protected override async Task BeforeMediatorSend(IResolveFieldContext<object> context, QuotesQuery request)
    {
        await base.BeforeMediatorSend(context, request);
        await Authorize(context, request, new QuoteAuthorizationRequirement());
    }
}
