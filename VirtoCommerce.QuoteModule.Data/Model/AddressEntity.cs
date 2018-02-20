using System;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.QuoteModule.Data.Model
{
    public class AddressEntity : Entity
	{
		[StringLength(32)]
		public string AddressType { get; set; }
		[StringLength(64)]
		public string Organization { get; set; }
		[StringLength(3)]
		public string CountryCode { get; set; }
		[Required]
		[StringLength(64)]
		public string CountryName { get; set; }
		[Required]
		[StringLength(128)]
		public string City { get; set; }
		[StringLength(64)]
		public string PostalCode { get; set; }
		[StringLength(2048)]
		public string Line1 { get; set; }
		[StringLength(2048)]
		public string Line2 { get; set; }
		[StringLength(128)]
		public string RegionId { get; set; }
		[StringLength(128)]
		public string RegionName { get; set; }
		[Required]
		[StringLength(64)]
		public string FirstName { get; set; }
		[Required]
		[StringLength(64)]
		public string LastName { get; set; }
		[StringLength(64)]
		public string Phone { get; set; }
		[StringLength(64)]
		public string Email { get; set; }

        #region Navigation Properties

        public string QuoteRequestId { get; set; }
        public virtual QuoteRequestEntity QuoteRequest { get; set; }
        
        #endregion
       
        public virtual Address ToModel(Address address)
        {
            if (address == null)
              throw new ArgumentNullException(nameof(address));

            address.AddressType = (AddressType)Enum.Parse(typeof(AddressType), this.AddressType);
            address.Organization = this.Organization;
            address.CountryCode = this.CountryCode;
            address.CountryName = this.CountryName;
            address.City = this.City;
            address.PostalCode = this.PostalCode;
            address.Line1 = this.Line1;
            address.Line2 = this.Line2;
            address.RegionId = this.RegionId;
            address.RegionName = this.RegionName;
            address.FirstName = this.FirstName;
            address.LastName = this.LastName;
            address.Phone = this.Phone;
            address.Email = this.Email;

            return address;
        }

        public virtual AddressEntity FromModel(Address address)
        {
            if (address == null)
              throw new ArgumentNullException(nameof(address));

            this.AddressType = address.AddressType.ToString();
            this.Organization = address.Organization;
            this.CountryCode = address.CountryCode;
            this.CountryName = address.CountryName;
            this.City = address.City;
            this.PostalCode = address.PostalCode;
            this.Line1 = address.Line1;
            this.Line2 = address.Line2;
            this.RegionId = address.RegionId;
            this.RegionName = address.RegionName;
            this.FirstName = address.FirstName;
            this.LastName = address.LastName;
            this.Phone = address.Phone;
            this.Email = address.Email;
           
            return this;
        }
      
        public virtual void Patch(AddressEntity target)
        {
            target.AddressType= this.AddressType;
            target.Organization = this.Organization;
            target.CountryCode = this.CountryCode;
            target.CountryName = this.CountryName;
            target.City = this.City;
            target.PostalCode = this.PostalCode;
            target.Line1 = this.Line1;
            target.Line2 = this.Line2;
            target.RegionId = this.RegionId;
            target.RegionName = this.RegionName;
            target.FirstName = this.FirstName;
            target.LastName = this.LastName;
            target.Phone = this.Phone;
            target.Email = this.Email;
        }
    }
}
