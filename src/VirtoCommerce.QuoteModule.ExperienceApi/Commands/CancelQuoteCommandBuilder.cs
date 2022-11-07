using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class CancelQuoteCommandBuilder : QuoteCommandBuilder<CancelQuoteCommand, CancelQuoteCommandType>
{
    protected override string Name => "cancelQuoteRequest";

    public CancelQuoteCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
    }
}
