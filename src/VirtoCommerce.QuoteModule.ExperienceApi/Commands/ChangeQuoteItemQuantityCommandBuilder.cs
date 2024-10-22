using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.Xapi.Core.Infrastructure;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

[SubSchemaName("Quote")]
public class ChangeQuoteItemQuantityCommandBuilder : QuoteCommandBuilder<ChangeQuoteItemQuantityCommand, ChangeQuoteItemQuantityCommandType>
{
    protected override string Name => "changeQuoteItemQuantity";

    public ChangeQuoteItemQuantityCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
    }
}
