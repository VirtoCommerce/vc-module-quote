using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class UpdateQuoteAttachmentsCommandBuilder : QuoteCommandBuilder<UpdateQuoteAttachmentsCommand, UpdateQuoteAttachmentsCommandType>
{
    protected override string Name => "UpdateQuoteAttachments";

    public UpdateQuoteAttachmentsCommandBuilder(IMediator mediator, IAuthorizationService authorizationService) : base(mediator, authorizationService)
    {
    }

    // Validate files
}
