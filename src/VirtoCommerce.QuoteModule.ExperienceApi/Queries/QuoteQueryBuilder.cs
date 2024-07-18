using System.Threading.Tasks;
using GraphQL;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.Xapi.Core.BaseQueries;
using VirtoCommerce.Xapi.Core.Extensions;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using VirtoCommerce.QuoteModule.ExperienceApi.Authorization;
using VirtoCommerce.QuoteModule.ExperienceApi.Schemas;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Queries;

public class QuoteQueryBuilder : QueryBuilder<QuoteQuery, QuoteAggregate, QuoteType>
{
    protected override string Name => "quote";

    public QuoteQueryBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
    }

    protected override async Task BeforeMediatorSend(IResolveFieldContext<object> context, QuoteQuery request)
    {
        await base.BeforeMediatorSend(context, request);
        context.CopyArgumentsToUserContext();
    }

    protected override async Task AfterMediatorSend(IResolveFieldContext<object> context, QuoteQuery request, QuoteAggregate response)
    {
        await base.AfterMediatorSend(context, request, response);
        await Authorize(context, response, new QuoteAuthorizationRequirement());
    }
}
