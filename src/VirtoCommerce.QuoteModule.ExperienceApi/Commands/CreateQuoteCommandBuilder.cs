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

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class CreateQuoteCommandBuilder : CommandBuilder<CreateQuoteCommand, QuoteAggregate, CreateQuoteCommandType, QuoteType>
{
    protected override string Name => "CreateQuote";

    public CreateQuoteCommandBuilder(IAuthorizationService authorizationService)
        : base(authorizationService)
    {
    }

    [Obsolete("Use the constructor without IMediator. The mediator is resolved from context.RequestServices per request.", DiagnosticId = "VC0015", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public CreateQuoteCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : this(authorizationService)
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

    protected override CreateQuoteCommand GetRequest(IResolveFieldContext<object> context)
    {
        var command = base.GetRequest(context);

        if (command != null)
        {
            command.CurrentOrganizationId = context.GetCurrentOrganizationId();
        }

        return command;
    }
}
