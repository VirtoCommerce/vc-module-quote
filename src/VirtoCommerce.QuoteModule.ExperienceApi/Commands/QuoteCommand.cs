using GraphQL.Types;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using VirtoCommerce.Xapi.Core.Infrastructure;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public abstract class QuoteCommand : ICommand<QuoteAggregate>
{
    public string QuoteId { get; set; }
}

public abstract class QuoteCommandType<TCommand> : InputObjectGraphType<TCommand>
    where TCommand : QuoteCommand
{
    protected QuoteCommandType()
    {
        Field(x => x.QuoteId);
    }
}
