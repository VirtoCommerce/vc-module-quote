using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class AddQuoteAttachmentsCommandBuilder : QuoteCommandBuilder<AddQuoteAttachmentsCommand, AddQuoteAttachmentsCommandType>
{
    protected override string Name => "AddQuoteAttachments";

    public AddQuoteAttachmentsCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
    }

    // Validate files
}
