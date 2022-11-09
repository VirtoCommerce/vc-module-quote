using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class ChangeQuoteCommentCommandBuilder : QuoteCommandBuilder<ChangeQuoteCommentCommand, ChangeQuoteCommentCommandType>
{
    protected override string Name => "changeQuoteComment";

    public ChangeQuoteCommentCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
    }
}
