using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class SubmitQuoteCommandBuilder : QuoteCommandBuilder<SubmitQuoteCommand, SubmitQuoteCommandType>
{
    protected override string Name => "submitQuoteRequest";

    public SubmitQuoteCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
    }
}
