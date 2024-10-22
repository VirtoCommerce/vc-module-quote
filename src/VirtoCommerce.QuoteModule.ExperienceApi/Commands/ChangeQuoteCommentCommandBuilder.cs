using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.Xapi.Core.Infrastructure;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

[SubSchemaName("Quote")]
public class ChangeQuoteCommentCommandBuilder : QuoteCommandBuilder<ChangeQuoteCommentCommand, ChangeQuoteCommentCommandType>
{
    protected override string Name => "changeQuoteComment";

    public ChangeQuoteCommentCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
    }
}
