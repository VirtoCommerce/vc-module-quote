using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class AddQuoteItemsCommandBuilder : QuoteCommandBuilder<AddQuoteItemsCommand, AddQuoteItemsCommandType>
{
    protected override string Name => "addQuoteItems";

    public AddQuoteItemsCommandBuilder(IMediator mediator, IAuthorizationService authorizationService) : base(mediator, authorizationService)
    {
    }
}
