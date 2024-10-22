using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.Xapi.Core.Infrastructure;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

[SubSchemaName("Quote")]
public class SubmitQuoteCommandBuilder : QuoteCommandBuilder<SubmitQuoteCommand, SubmitQuoteCommandType>
{
    protected override string Name => "submitQuoteRequest";

    public SubmitQuoteCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
    }
}
