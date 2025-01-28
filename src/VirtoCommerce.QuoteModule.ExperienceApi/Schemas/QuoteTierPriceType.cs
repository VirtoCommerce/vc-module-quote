using GraphQL.Types;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using VirtoCommerce.Xapi.Core.Extensions;
using VirtoCommerce.Xapi.Core.Schemas;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Schemas;

public class QuoteTierPriceType : ExtendableGraphType<QuoteTierPriceAggregate>
{
    public QuoteTierPriceType()
    {
        Field(x => x.Model.Quantity, nullable: false);
        Field<NonNullGraphType<MoneyType>>(nameof(TierPrice.Price)).Resolve(context => context.Source.Model.Price.ToMoney(context.Source.Quote.Currency));
    }
}
