using System;
using System.Threading.Tasks;
using GraphQL;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using VirtoCommerce.QuoteModule.ExperienceApi.Authorization;
using VirtoCommerce.QuoteModule.ExperienceApi.Schemas;
using VirtoCommerce.Xapi.Core.BaseQueries;
using VirtoCommerce.Xapi.Core.Extensions;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Queries;

public class QuotesQueryBuilder : SearchQueryBuilder<QuotesQuery, QuoteAggregateSearchResult, QuoteAggregate, QuoteType>
{
    protected override string Name => "quotes";

    public QuotesQueryBuilder(IAuthorizationService authorizationService)
        : base(authorizationService)
    {
    }

    [Obsolete("Use the constructor without IMediator. The mediator is resolved from context.RequestServices per request.", DiagnosticId = "VC0015", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public QuotesQueryBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : this(authorizationService)
    {
    }

    protected override async Task BeforeMediatorSend(IResolveFieldContext<object> context, QuotesQuery request)
    {
        await base.BeforeMediatorSend(context, request);
        await Authorize(context, request, new QuoteAuthorizationRequirement());
        context.CopyArgumentsToUserContext();
    }
}
