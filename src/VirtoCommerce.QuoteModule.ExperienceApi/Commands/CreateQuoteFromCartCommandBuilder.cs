using System.Threading.Tasks;
using GraphQL;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.ExperienceApiModule.Core.BaseQueries;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using VirtoCommerce.QuoteModule.ExperienceApi.Schemas;
using VirtoCommerce.XPurchase.Authorization;
using VirtoCommerce.XPurchase.Queries;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class CreateQuoteFromCartCommandBuilder : CommandBuilder<CreateQuoteFromCartCommand, QuoteAggregate, CreateQuoteFromCartCommandType, QuoteType>
{
    private readonly IMediator _mediator;

    protected override string Name => "createQuoteFromCart";

    public CreateQuoteFromCartCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
        _mediator = mediator;
    }

    protected override async Task BeforeMediatorSend(IResolveFieldContext<object> context, CreateQuoteFromCartCommand request)
    {
        await base.BeforeMediatorSend(context, request);
        await CheckCanAccessCart(context, request.CartId);
    }

    protected virtual async Task CheckCanAccessCart(IResolveFieldContext context, string cartId)
    {
        var cart = await _mediator.Send(new GetCartByIdQuery { CartId = cartId });
        await Authorize(context, cart?.Cart, new CanAccessCartAuthorizationRequirement());
    }
}
