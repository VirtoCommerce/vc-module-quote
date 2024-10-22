using System.Threading.Tasks;
using GraphQL;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using VirtoCommerce.QuoteModule.ExperienceApi.Authorization;
using VirtoCommerce.QuoteModule.ExperienceApi.Schemas;
using VirtoCommerce.Xapi.Core.BaseQueries;
using VirtoCommerce.Xapi.Core.Infrastructure;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

[SubSchemaName("Quote")]
public class CreateQuoteCommandBuilder : CommandBuilder<CreateQuoteCommand, QuoteAggregate, CreateQuoteCommandType, QuoteType>
{
    protected override string Name => "CreateQuote";

    public CreateQuoteCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
    }

    protected override async Task BeforeMediatorSend(IResolveFieldContext<object> context, CreateQuoteCommand request)
    {
        await base.BeforeMediatorSend(context, request);
        await CheckCanCreateQuote(context, request.UserId);
    }

    protected virtual Task CheckCanCreateQuote(IResolveFieldContext<object> context, string userId)
    {
        return Authorize(context, userId, new QuoteAuthorizationRequirement());
    }
}
