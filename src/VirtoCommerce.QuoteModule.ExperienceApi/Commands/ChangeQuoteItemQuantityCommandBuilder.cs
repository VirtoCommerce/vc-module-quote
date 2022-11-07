using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class ChangeQuoteItemQuantityCommandBuilder : QuoteCommandBuilder<ChangeQuoteItemQuantityCommand, ChangeQuoteItemQuantityCommandType>
{
    protected override string Name => "changeQuoteItemQuantity";

    public ChangeQuoteItemQuantityCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
    }
}
