using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.Xapi.Core.Infrastructure;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

[SubSchemaName("Quote")]
public class RemoveQuoteItemCommandBuilder : QuoteCommandBuilder<RemoveQuoteItemCommand, RemoveQuoteItemCommandType>
{
    protected override string Name => "removeQuoteItem";

    public RemoveQuoteItemCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
    }
}
