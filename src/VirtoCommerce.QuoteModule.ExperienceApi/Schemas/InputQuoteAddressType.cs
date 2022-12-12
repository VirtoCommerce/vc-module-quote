using GraphQL.Types;
using VirtoCommerce.QuoteModule.Core.Models;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Schemas;

public class InputQuoteAddressType : InputObjectGraphType<Address>
{
    public InputQuoteAddressType()
    {
        Field<IntGraphType>(nameof(Address.AddressType));
        Field<StringGraphType>("id", resolve: context => context.Source.Key, description: "ID");

        Field(x => x.Key, nullable: true);
        Field(x => x.OuterId, nullable: true);
        Field(x => x.Name, nullable: true);
        Field(x => x.CountryCode, nullable: true);
        Field(x => x.CountryName, nullable: false);
        Field(x => x.PostalCode, nullable: true);
        Field(x => x.RegionId, nullable: true);
        Field(x => x.RegionName, nullable: true);
        Field(x => x.City, nullable: false);
        Field(x => x.Line1, nullable: true);
        Field(x => x.Line2, nullable: true);
        Field(x => x.Email, nullable: true);
        Field(x => x.Phone, nullable: true);
        Field(x => x.FirstName, nullable: false);
        Field(x => x.LastName, nullable: false);
        Field(x => x.Organization, nullable: true).Description("Company name");
    }
}
