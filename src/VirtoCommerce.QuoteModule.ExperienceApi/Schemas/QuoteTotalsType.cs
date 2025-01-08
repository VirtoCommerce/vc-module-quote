using GraphQL.Types;
using VirtoCommerce.CoreModule.Core.Currency;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using VirtoCommerce.Xapi.Core.Schemas;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Schemas;

public class QuoteTotalsType : ExtendableGraphType<QuoteTotalsAggregate>
{
    public QuoteTotalsType()
    {
        Field<NonNullGraphType<MoneyType>>(nameof(QuoteRequestTotals.OriginalSubTotalExlTax)).Resolve(context => new Money(context.Source.Model.OriginalSubTotalExlTax, context.Source.Quote.Currency));
        Field<NonNullGraphType<MoneyType>>(nameof(QuoteRequestTotals.SubTotalExlTax)).Resolve(context => new Money(context.Source.Model.SubTotalExlTax, context.Source.Quote.Currency));
        Field<NonNullGraphType<MoneyType>>(nameof(QuoteRequestTotals.ShippingTotal)).Resolve(context => new Money(context.Source.Model.ShippingTotal, context.Source.Quote.Currency));
        Field<NonNullGraphType<MoneyType>>(nameof(QuoteRequestTotals.DiscountTotal)).Resolve(context => new Money(context.Source.Model.DiscountTotal, context.Source.Quote.Currency));
        Field<NonNullGraphType<MoneyType>>(nameof(QuoteRequestTotals.TaxTotal)).Resolve(context => new Money(context.Source.Model.TaxTotal, context.Source.Quote.Currency));
        Field<NonNullGraphType<MoneyType>>(nameof(QuoteRequestTotals.AdjustmentQuoteExlTax)).Resolve(context => new Money(context.Source.Model.AdjustmentQuoteExlTax, context.Source.Quote.Currency));
        Field<NonNullGraphType<MoneyType>>(nameof(QuoteRequestTotals.GrandTotalExlTax)).Resolve(context => new Money(context.Source.Model.GrandTotalExlTax, context.Source.Quote.Currency));
        Field<NonNullGraphType<MoneyType>>(nameof(QuoteRequestTotals.GrandTotalInclTax)).Resolve(context => new Money(context.Source.Model.GrandTotalInclTax, context.Source.Quote.Currency));
    }
}
