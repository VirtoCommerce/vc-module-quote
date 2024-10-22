using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.Xapi.Core.Infrastructure;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

[SubSchemaName("Quote")]
public class DeclineQuoteCommandBuilder : QuoteCommandBuilder<DeclineQuoteCommand, DeclineQuoteCommandType>
{
    protected override string Name => "declineQuoteRequest";

    public DeclineQuoteCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
    }
}
