using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.Xapi.Core.Infrastructure;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

[SubSchemaName("Quote")]
public class UpdateQuoteAttachmentsCommandBuilder : QuoteCommandBuilder<UpdateQuoteAttachmentsCommand, UpdateQuoteAttachmentsCommandType>
{
    protected override string Name => "UpdateQuoteAttachments";

    public UpdateQuoteAttachmentsCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
    }
}
