using System;
using System.Threading.Tasks;
using GraphQL;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using VirtoCommerce.QuoteModule.ExperienceApi.Schemas;
using VirtoCommerce.Xapi.Core.BaseQueries;
using VirtoCommerce.Xapi.Core.Extensions;
using VirtoCommerce.XCart.Core.Queries;
using VirtoCommerce.XCart.Data.Authorization;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class CreateQuoteFromCartCommandBuilder : CommandBuilder<CreateQuoteFromCartCommand, QuoteAggregate, CreateQuoteFromCartCommandType, QuoteType>
{
    protected override string Name => "createQuoteFromCart";

    public CreateQuoteFromCartCommandBuilder(IAuthorizationService authorizationService)
        : base(authorizationService)
    {
    }

    [Obsolete("Use the constructor without IMediator. The mediator is resolved from context.RequestServices per request.", DiagnosticId = "VC0015", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public CreateQuoteFromCartCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : this(authorizationService)
    {
    }

    protected override async Task BeforeMediatorSend(IResolveFieldContext<object> context, CreateQuoteFromCartCommand request)
    {
        await base.BeforeMediatorSend(context, request);
        await CheckCanAccessCart(context, request.CartId);
    }

    protected virtual async Task CheckCanAccessCart(IResolveFieldContext context, string cartId)
    {
        var cart = await context.GetMediator().Send(new GetCartByIdQuery { CartId = cartId });
        await Authorize(context, cart?.Cart, new CanAccessCartAuthorizationRequirement());
    }
}
