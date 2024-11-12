using GraphQL.Types;
using VirtoCommerce.QuoteModule.ExperienceApi.Models;
using VirtoCommerce.QuoteModule.ExperienceApi.Schemas;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class AddQuoteItemsCommand : QuoteCommand
{
    public NewQuoteItem[] NewQuoteItems { get; set; }
}

public class AddQuoteItemsCommandType : QuoteCommandType<AddQuoteItemsCommand>
{
    public AddQuoteItemsCommandType()
    {
        Field<NonNullGraphType<ListGraphType<InputNewQuoteItemType>>>(nameof(AddQuoteItemsCommand.NewQuoteItems));
    }
}
