using System.Threading.Tasks;
using GraphQL;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.ExperienceApiModule.Core.BaseQueries;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using VirtoCommerce.QuoteModule.ExperienceApi.Authorization;
using VirtoCommerce.QuoteModule.ExperienceApi.Queries;
using VirtoCommerce.QuoteModule.ExperienceApi.Schemas;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class CancelQuoteCommandBuilder : CommandBuilder<CancelQuoteCommand, QuoteAggregate, CancelQuoteCommandType, QuoteType>
{
    private readonly IMediator _mediator;

    protected override string Name => "cancelQuoteRequest";

    public CancelQuoteCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
        _mediator = mediator;
    }

    protected override async Task BeforeMediatorSend(IResolveFieldContext<object> context, CancelQuoteCommand request)
    {
        await base.BeforeMediatorSend(context, request);
        await CheckCanAccessQuote(context, request.QuoteId);
    }

    protected virtual async Task CheckCanAccessQuote(IResolveFieldContext<object> context, string quoteId)
    {
        var quote = await _mediator.Send(new QuoteQuery { Id = quoteId });
        await Authorize(context, quote, new QuoteAuthorizationRequirement());
    }
}
