using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.ExperienceApiModule.Core.BaseQueries;
using VirtoCommerce.FileExperienceApi.Core.Models;
using VirtoCommerce.FileExperienceApi.Data.Schemas;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Queries;

public class QuoteAttachmentOptionsQueryBuilder : QueryBuilder<QuoteAttachmentOptionsQuery, FileUploadScopeOptions, FileUploadScopeOptionsType>
{
    protected override string Name => "QuoteAttachmentOptions";

    public QuoteAttachmentOptionsQueryBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
    }
}
