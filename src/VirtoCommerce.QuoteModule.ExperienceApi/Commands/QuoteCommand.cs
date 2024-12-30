using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using VirtoCommerce.Xapi.Core.Infrastructure;
using VirtoCommerce.Xapi.Core.Schemas;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public abstract class QuoteCommand : ICommand<QuoteAggregate>
{
    public string QuoteId { get; set; }
}

public abstract class QuoteCommandType<TCommand> : ExtendableInputGraphType<TCommand>
    where TCommand : QuoteCommand
{
    protected QuoteCommandType()
    {
        Field(x => x.QuoteId);
    }
}
