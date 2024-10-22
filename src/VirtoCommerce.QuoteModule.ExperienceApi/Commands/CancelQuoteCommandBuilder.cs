using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.Xapi.Core.Infrastructure;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

[SubSchemaName("Quote")]
public class CancelQuoteCommandBuilder : QuoteCommandBuilder<CancelQuoteCommand, CancelQuoteCommandType>
{
    protected override string Name => "cancelQuoteRequest";

    public CancelQuoteCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
    }
}
