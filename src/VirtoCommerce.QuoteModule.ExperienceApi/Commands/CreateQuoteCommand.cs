using GraphQL.Types;
using VirtoCommerce.ExperienceApiModule.Core.Infrastructure;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class CreateQuoteCommand : ICommand<QuoteAggregate>
{
    public string StoreId { get; set; }
    public string UserId { get; set; }
    public string CurrencyCode { get; set; }
    public string CultureName { get; set; }
}

public class CreateQuoteCommandType : InputObjectGraphType<CreateQuoteCommand>
{
    public CreateQuoteCommandType()
    {
        Field(x => x.StoreId);
        Field(x => x.UserId);
        Field(x => x.CurrencyCode);
        Field(x => x.CultureName);
    }
}
