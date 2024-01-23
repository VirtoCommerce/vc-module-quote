using System.Collections.Generic;
using GraphQL.Types;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class AddQuoteAttachmentsCommand : QuoteCommand
{
    public IList<string> Urls { get; set; }
}

public class AddQuoteAttachmentsCommandType : QuoteCommandType<AddQuoteAttachmentsCommand>
{
    public AddQuoteAttachmentsCommandType()
    {
        Field<NonNullGraphType<ListGraphType<StringGraphType>>>(nameof(AddQuoteAttachmentsCommand.Urls));
    }
}
