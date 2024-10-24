using GraphQL.Types;
using VirtoCommerce.QuoteModule.ExperienceApi.Models;
using VirtoCommerce.Xapi.Core.Schemas;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Schemas;

public class InputNewQuoteItemType: InputObjectGraphType<NewQuoteItem>
{
    public InputNewQuoteItemType()
    {
        Field(x => x.ProductId, nullable: true);
        Field(x => x.Name, nullable: true);
        Field(x => x.Quantity, nullable: false);
        Field(x => x.Price, nullable: true);
        Field(x => x.Comment, nullable: true);

        Field<ListGraphType<InputDynamicPropertyValueType>>("dynamicProperties");
    }
}
