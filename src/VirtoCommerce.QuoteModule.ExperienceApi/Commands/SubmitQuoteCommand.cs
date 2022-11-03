using GraphQL.Types;
using VirtoCommerce.ExperienceApiModule.Core.Infrastructure;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class SubmitQuoteCommand : ICommand<QuoteAggregate>
{
    public string QuoteId { get; set; }
    public string Comment { get; set; }
}

public class SubmitQuoteCommandType : InputObjectGraphType<SubmitQuoteCommand>
{
    public SubmitQuoteCommandType()
    {
        Field(x => x.QuoteId);
        Field(x => x.Comment);
    }
}
