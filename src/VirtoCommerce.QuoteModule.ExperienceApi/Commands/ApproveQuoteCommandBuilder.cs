using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.Xapi.Core.BaseQueries;
using VirtoCommerce.QuoteModule.ExperienceApi.Models;
using VirtoCommerce.QuoteModule.ExperienceApi.Schemas;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class ApproveQuoteCommandBuilder : CommandBuilder<ApproveQuoteCommand, ApproveQuoteResult, ApproveQuoteCommandType, ApproveQuoteResultType>
{
    protected override string Name => "approveQuoteRequest";

    public ApproveQuoteCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
    }
}
