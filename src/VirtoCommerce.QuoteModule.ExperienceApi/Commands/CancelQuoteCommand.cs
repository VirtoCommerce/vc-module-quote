using GraphQL.Types;
using VirtoCommerce.ExperienceApiModule.Core.Infrastructure;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class CancelQuoteCommand : ICommand<QuoteAggregate>
{
    public string QuoteId { get; set; }
    public string Comment { get; set; }
}

public class CancelQuoteCommandType : InputObjectGraphType<CancelQuoteCommand>
{
    public CancelQuoteCommandType()
    {
        Field(x => x.QuoteId);
        Field(x => x.Comment);
    }
}
