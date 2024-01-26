using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class DeleteQuoteAttachmentsCommandBuilder : QuoteCommandBuilder<DeleteQuoteAttachmentsCommand, DeleteQuoteAttachmentsCommandType>
{
    protected override string Name => "DeleteQuoteAttachments";

    public DeleteQuoteAttachmentsCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
    }
}
