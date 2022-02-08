using VirtoCommerce.Platform.Core.Swagger;

namespace VirtoCommerce.QuoteModule.Core.Models
{
    [SwaggerSchemaId("QuoteAddress")]
    public class Address : CoreModule.Core.Common.Address
    {
        public virtual TaxModule.Core.Model.Address ToTaxModel(TaxModule.Core.Model.Address target)
        {
            target.Key = Key;

            target.CountryCode = CountryCode;
            target.CountryName = CountryName;
            target.PostalCode = PostalCode;
            target.RegionId = RegionId;
            target.RegionName = RegionName;
            target.City = City;
            target.Email = Email;
            target.FirstName = FirstName;
            target.LastName = LastName;
            target.Line1 = Line1;
            target.Line2 = Line2;
            target.Phone = Phone;
            target.AddressType = AddressType;
            target.Organization = Organization;
            target.OuterId = OuterId;

            return target;
        }
    }
}
