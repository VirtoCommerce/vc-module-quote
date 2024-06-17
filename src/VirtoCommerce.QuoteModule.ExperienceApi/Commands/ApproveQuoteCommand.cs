using GraphQL.Types;
using VirtoCommerce.Xapi.Core.Infrastructure;
using VirtoCommerce.QuoteModule.ExperienceApi.Models;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class ApproveQuoteCommand : ICommand<ApproveQuoteResult>
{
    public string QuoteId { get; set; }
}

public class ApproveQuoteCommandType : InputObjectGraphType<ApproveQuoteCommand>
{
    public ApproveQuoteCommandType()
    {
        Field(x => x.QuoteId);
    }
}
