using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.Xapi.Core.Infrastructure;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

[SubSchemaName("Quote")]
public class AddQuoteAttachmentsCommandBuilder : QuoteCommandBuilder<AddQuoteAttachmentsCommand, AddQuoteAttachmentsCommandType>
{
    protected override string Name => "AddQuoteAttachments";

    public AddQuoteAttachmentsCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
    }
}
