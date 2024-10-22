using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.Xapi.Core.Infrastructure;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

[SubSchemaName("Quote")]
public class DeleteQuoteAttachmentsCommandBuilder : QuoteCommandBuilder<DeleteQuoteAttachmentsCommand, DeleteQuoteAttachmentsCommandType>
{
    protected override string Name => "DeleteQuoteAttachments";

    public DeleteQuoteAttachmentsCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
    }
}
