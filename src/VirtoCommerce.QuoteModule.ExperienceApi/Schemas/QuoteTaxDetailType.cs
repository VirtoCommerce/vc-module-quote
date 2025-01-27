using GraphQL.Types;
using VirtoCommerce.CoreModule.Core.Currency;
using VirtoCommerce.CoreModule.Core.Tax;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using VirtoCommerce.Xapi.Core.Schemas;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Schemas;

public class QuoteTaxDetailType : ExtendableGraphType<QuoteTaxDetailAggregate>
{
    public QuoteTaxDetailType()
    {
        Field<NonNullGraphType<MoneyType>>(nameof(TaxDetail.Rate)).Resolve(context => new Money(context.Source.Model.Rate, context.Source.Quote.Currency));
        Field<NonNullGraphType<MoneyType>>(nameof(TaxDetail.Amount)).Resolve(context => new Money(context.Source.Model.Amount, context.Source.Quote.Currency));
        Field(x => x.Model.Name, nullable: true);
    }
}
