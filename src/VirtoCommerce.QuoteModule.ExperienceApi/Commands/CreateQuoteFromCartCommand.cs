using GraphQL.Types;
using VirtoCommerce.ExperienceApiModule.Core.Infrastructure;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class CreateQuoteFromCartCommand : ICommand<QuoteAggregate>
{
    public string CartId { get; set; }
    public string Comment { get; set; }
}

public class CreateQuoteFromCartCommandType : InputObjectGraphType<CreateQuoteFromCartCommand>
{
    public CreateQuoteFromCartCommandType()
    {
        Field(x => x.CartId);
        Field(x => x.Comment);
    }
}
