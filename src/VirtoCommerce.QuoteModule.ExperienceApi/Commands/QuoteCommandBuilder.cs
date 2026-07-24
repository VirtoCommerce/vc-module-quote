using System;
using System.Threading.Tasks;
using GraphQL;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.Xapi.Core.BaseQueries;
using VirtoCommerce.Xapi.Core.Extensions;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using VirtoCommerce.QuoteModule.ExperienceApi.Authorization;
using VirtoCommerce.QuoteModule.ExperienceApi.Queries;
using VirtoCommerce.QuoteModule.ExperienceApi.Schemas;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public abstract class QuoteCommandBuilder<TCommand, TCommandGraphType> : CommandBuilder<TCommand, QuoteAggregate, TCommandGraphType, QuoteType>
    where TCommand : QuoteCommand
    where TCommandGraphType : QuoteCommandType<TCommand>
{
    protected QuoteCommandBuilder(IAuthorizationService authorizationService)
        : base(authorizationService)
    {
    }

    [Obsolete("Use the constructor without IMediator. The mediator is resolved from context.RequestServices per request.", DiagnosticId = "VC0015", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    protected QuoteCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : this(authorizationService)
    {
    }

    protected override async Task BeforeMediatorSend(IResolveFieldContext<object> context, TCommand request)
    {
        await base.BeforeMediatorSend(context, request);
        await CheckCanAccessQuote(context, request.QuoteId);
    }

    protected virtual async Task CheckCanAccessQuote(IResolveFieldContext<object> context, string quoteId)
    {
        var quote = await context.GetMediator().Send(new QuoteQuery { Id = quoteId });
        await Authorize(context, quote, new QuoteAuthorizationRequirement());
    }
}
