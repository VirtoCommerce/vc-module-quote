using System;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class AddQuoteAttachmentsCommandBuilder : QuoteCommandBuilder<AddQuoteAttachmentsCommand, AddQuoteAttachmentsCommandType>
{
    protected override string Name => "AddQuoteAttachments";

    public AddQuoteAttachmentsCommandBuilder(IAuthorizationService authorizationService)
        : base(authorizationService)
    {
    }

    [Obsolete("Use the constructor without IMediator. The mediator is resolved from context.RequestServices per request.", DiagnosticId = "VC0015", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public AddQuoteAttachmentsCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : this(authorizationService)
    {
    }
}
