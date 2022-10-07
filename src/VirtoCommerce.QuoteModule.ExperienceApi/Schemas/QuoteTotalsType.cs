using GraphQL.Types;
using VirtoCommerce.CoreModule.Core.Currency;
using VirtoCommerce.ExperienceApiModule.Core.Schemas;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Schemas;

public class QuoteTotalsType : ObjectGraphType<QuoteTotalsAggregate>
{
    public QuoteTotalsType()
    {
        Field<MoneyType>(nameof(QuoteRequestTotals.OriginalSubTotalExlTax), resolve: context => new Money(context.Source.Model.OriginalSubTotalExlTax, context.Source.Quote.Currency));
        Field<MoneyType>(nameof(QuoteRequestTotals.SubTotalExlTax), resolve: context => new Money(context.Source.Model.SubTotalExlTax, context.Source.Quote.Currency));
        Field<MoneyType>(nameof(QuoteRequestTotals.ShippingTotal), resolve: context => new Money(context.Source.Model.ShippingTotal, context.Source.Quote.Currency));
        Field<MoneyType>(nameof(QuoteRequestTotals.DiscountTotal), resolve: context => new Money(context.Source.Model.DiscountTotal, context.Source.Quote.Currency));
        Field<MoneyType>(nameof(QuoteRequestTotals.TaxTotal), resolve: context => new Money(context.Source.Model.TaxTotal, context.Source.Quote.Currency));
        Field<MoneyType>(nameof(QuoteRequestTotals.AdjustmentQuoteExlTax), resolve: context => new Money(context.Source.Model.AdjustmentQuoteExlTax, context.Source.Quote.Currency));
        Field<MoneyType>(nameof(QuoteRequestTotals.GrandTotalExlTax), resolve: context => new Money(context.Source.Model.GrandTotalExlTax, context.Source.Quote.Currency));
        Field<MoneyType>(nameof(QuoteRequestTotals.GrandTotalInclTax), resolve: context => new Money(context.Source.Model.GrandTotalInclTax, context.Source.Quote.Currency));
    }
}
