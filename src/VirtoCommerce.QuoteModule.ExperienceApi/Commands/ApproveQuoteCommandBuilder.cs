using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.QuoteModule.ExperienceApi.Models;
using VirtoCommerce.QuoteModule.ExperienceApi.Schemas;
using VirtoCommerce.Xapi.Core.BaseQueries;
using VirtoCommerce.Xapi.Core.Infrastructure;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

[SubSchemaName("Quote")]
public class ApproveQuoteCommandBuilder : CommandBuilder<ApproveQuoteCommand, ApproveQuoteResult, ApproveQuoteCommandType, ApproveQuoteResultType>
{
    protected override string Name => "approveQuoteRequest";

    public ApproveQuoteCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
    }
}
