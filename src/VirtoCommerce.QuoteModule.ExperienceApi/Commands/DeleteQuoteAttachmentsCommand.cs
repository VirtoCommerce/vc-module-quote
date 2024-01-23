using System.Collections.Generic;
using GraphQL.Types;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class DeleteQuoteAttachmentsCommand : QuoteCommand
{
    public IList<string> Urls { get; set; }
}

public class DeleteQuoteAttachmentsCommandType : QuoteCommandType<DeleteQuoteAttachmentsCommand>
{
    public DeleteQuoteAttachmentsCommandType()
    {
        Field<NonNullGraphType<ListGraphType<StringGraphType>>>(nameof(DeleteQuoteAttachmentsCommand.Urls));
    }
}
