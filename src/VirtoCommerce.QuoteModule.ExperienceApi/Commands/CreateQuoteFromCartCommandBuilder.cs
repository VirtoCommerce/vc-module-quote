using System.Threading.Tasks;
using GraphQL;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.ExperienceApiModule.Core.BaseQueries;
using VirtoCommerce.ExperienceApiModule.Core.Extensions;
using VirtoCommerce.ExperienceApiModule.Core.Infrastructure.Authorization;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using VirtoCommerce.QuoteModule.ExperienceApi.Schemas;
using VirtoCommerce.XPurchase.Authorization;
using VirtoCommerce.XPurchase.Queries;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class CreateQuoteFromCartCommandBuilder : CommandBuilder<CreateQuoteFromCartCommand, QuoteAggregate, CreateQuoteFromCartCommandType, QuoteType>
{
    private readonly IMediator _mediator;
    private readonly IAuthorizationService _authorizationService;

    protected override string Name => "createQuoteFromCart";

    public CreateQuoteFromCartCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
        _mediator = mediator;
        _authorizationService = authorizationService;
    }

    protected override async Task BeforeMediatorSend(IResolveFieldContext<object> context, CreateQuoteFromCartCommand request)
    {
        await base.BeforeMediatorSend(context, request);
        await CheckCanAccessCart(context, request.CartId);
    }

    protected virtual async Task CheckCanAccessCart(IResolveFieldContext context, string cartId)
    {
        var cart = await _mediator.Send(new GetCartByIdQuery { CartId = cartId });

        var authorizationResult = await _authorizationService.AuthorizeAsync(
            context.GetCurrentPrincipal(),
            cart?.Cart,
            new CanAccessCartAuthorizationRequirement());

        if (!authorizationResult.Succeeded)
        {
            throw new AuthorizationError("Access denied");
        }
    }
}
