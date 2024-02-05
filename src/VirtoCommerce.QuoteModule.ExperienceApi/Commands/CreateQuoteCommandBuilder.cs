using System.Threading.Tasks;
using GraphQL;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.ExperienceApiModule.Core.BaseQueries;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using VirtoCommerce.QuoteModule.ExperienceApi.Authorization;
using VirtoCommerce.QuoteModule.ExperienceApi.Schemas;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

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
