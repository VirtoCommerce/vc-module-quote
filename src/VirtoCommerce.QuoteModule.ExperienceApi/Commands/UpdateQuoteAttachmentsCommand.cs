using System.Collections.Generic;
using GraphQL.Types;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class UpdateQuoteAttachmentsCommand : QuoteCommand
{
    public IList<string> Urls { get; set; }
}

public class UpdateQuoteAttachmentsCommandType : QuoteCommandType<UpdateQuoteAttachmentsCommand>
{
    public UpdateQuoteAttachmentsCommandType()
    {
        Field<NonNullGraphType<ListGraphType<StringGraphType>>>(nameof(UpdateQuoteAttachmentsCommand.Urls));
    }
}
