using System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.Xapi.Core.BaseQueries;
using VirtoCommerce.QuoteModule.ExperienceApi.Models;
using VirtoCommerce.QuoteModule.ExperienceApi.Schemas;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class ApproveQuoteCommandBuilder : CommandBuilder<ApproveQuoteCommand, ApproveQuoteResult, ApproveQuoteCommandType, ApproveQuoteResultType>
{
    protected override string Name => "approveQuoteRequest";

    public ApproveQuoteCommandBuilder(IAuthorizationService authorizationService)
        : base(authorizationService)
    {
    }

    [Obsolete("Use the constructor without IMediator. The mediator is resolved from context.RequestServices per request.", DiagnosticId = "VC0015", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public ApproveQuoteCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : this(authorizationService)
    {
    }
}
