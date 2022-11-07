using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class RemoveQuoteItemCommandBuilder : QuoteCommandBuilder<RemoveQuoteItemCommand, RemoveQuoteItemCommandType>
{
    protected override string Name => "removeQuoteItem";

    public RemoveQuoteItemCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
    }
}
