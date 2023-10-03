using GraphQL.Types;
using VirtoCommerce.ExperienceApiModule.Core.Extensions;
using VirtoCommerce.ExperienceApiModule.Core.Schemas;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Schemas;

public class QuoteTierPriceType : ObjectGraphType<QuoteTierPriceAggregate>
{
    public QuoteTierPriceType()
    {
        Field(x => x.Model.Quantity, nullable: false);
        Field<NonNullGraphType<MoneyType>>(nameof(TierPrice.Price), resolve: context => context.Source.Model.Price.ToMoney(context.Source.Quote.Currency));
    }
}
