using GraphQL.Types;
using VirtoCommerce.CoreModule.Core.Currency;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.ExperienceApi.Aggregates;
using VirtoCommerce.Xapi.Core.Schemas;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Schemas;

public class QuoteShipmentMethodType : ExtendableGraphType<QuoteShipmentMethodAggregate>
{
    public QuoteShipmentMethodType()
    {
        Field(x => x.Model.ShipmentMethodCode, nullable: false);
        Field(x => x.Model.OptionName, nullable: true);
        Field(x => x.Model.LogoUrl, nullable: true);
        Field(x => x.Model.TypeName, nullable: true);
        Field<NonNullGraphType<CurrencyType>>(nameof(ShipmentMethod.Currency)).Resolve(context => context.Source.Quote.Currency);
        Field<NonNullGraphType<MoneyType>>(nameof(ShipmentMethod.Price)).Resolve(context => new Money(context.Source.Model.Price, context.Source.Quote.Currency));
    }
}
