using GraphQL.Types;
using VirtoCommerce.CoreModule.Core.Currency;
using VirtoCommerce.CoreModule.Core.Tax;
using VirtoCommerce.Xapi.Core.Schemas;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Schemas;

public class QuoteTaxDetailType : ExtendableGraphType<QuoteTaxDetailAggregate>
{
    public QuoteTaxDetailType()
    {
        Field<NonNullGraphType<MoneyType>>(nameof(TaxDetail.Rate), resolve: context => new Money(context.Source.Model.Rate, context.Source.Quote.Currency));
        Field<NonNullGraphType<MoneyType>>(nameof(TaxDetail.Amount), resolve: context => new Money(context.Source.Model.Amount, context.Source.Quote.Currency));
        Field(x => x.Model.Name, nullable: true);
    }
}
