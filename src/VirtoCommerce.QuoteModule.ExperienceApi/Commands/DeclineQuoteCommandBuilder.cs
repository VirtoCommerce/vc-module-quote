using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class DeclineQuoteCommandBuilder : QuoteCommandBuilder<DeclineQuoteCommand, DeclineQuoteCommandType>
{
    protected override string Name => "declineQuoteRequest";

    public DeclineQuoteCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
    }
}
