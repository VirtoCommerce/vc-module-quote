using System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.Xapi.Core.BaseQueries;
using VirtoCommerce.FileExperienceApi.Core.Models;
using VirtoCommerce.FileExperienceApi.Data.Schemas;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Queries;

public class QuoteAttachmentOptionsQueryBuilder : QueryBuilder<QuoteAttachmentOptionsQuery, FileUploadScopeOptions, FileUploadScopeOptionsType>
{
    protected override string Name => "QuoteAttachmentOptions";

    public QuoteAttachmentOptionsQueryBuilder(IAuthorizationService authorizationService)
        : base(authorizationService)
    {
    }

    [Obsolete("Use the constructor without IMediator. The mediator is resolved from context.RequestServices per request.", DiagnosticId = "VC0015", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public QuoteAttachmentOptionsQueryBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : this(authorizationService)
    {
    }
}
