using GraphQL.Types;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using VirtoCommerce.Xapi.Core.Infrastructure;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Commands;

public class CreateQuoteCommand : ICommand<QuoteAggregate>
{
    public string StoreId { get; set; }
    public string UserId { get; set; }
    public string CurrencyCode { get; set; }
    public string CultureName { get; set; }

    // set by the builder
    public string CurrentOrganizationId { get; set; }
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
