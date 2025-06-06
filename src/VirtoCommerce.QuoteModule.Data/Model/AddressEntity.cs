using System;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.QuoteModule.Core.Models;

namespace VirtoCommerce.QuoteModule.Data.Model
{
    public class AddressEntity : Entity, IHasOuterId
    {
        [StringLength(64)]
        public string AddressType { get; set; }

        [StringLength(512)]
        public string Organization { get; set; }

        [StringLength(3)]
        public string CountryCode { get; set; }

        [Required]
        [StringLength(128)]
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

        [StringLength(128)]
        public string FirstName { get; set; }

        [StringLength(128)]
        public string MiddleName { get; set; }

        [StringLength(128)]
        public string LastName { get; set; }

        [StringLength(64)]
        public string Phone { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        [StringLength(128)]
        public string OuterId { get; set; }

        public virtual QuoteRequestEntity QuoteRequest { get; set; }
        public string QuoteRequestId { get; set; }



        public virtual Address ToModel(Address target)
        {
            target.Key = Id;

            target.CountryCode = CountryCode;
            target.CountryName = CountryName;
            target.PostalCode = PostalCode;
            target.RegionId = RegionId;
            target.RegionName = RegionName;
            target.City = City;
            target.Email = Email;
            target.FirstName = FirstName;
            target.MiddleName = MiddleName;
            target.LastName = LastName;
            target.Line1 = Line1;
            target.Line2 = Line2;
            target.Phone = Phone;
            target.AddressType = EnumUtility.SafeParseFlags(AddressType, VirtoCommerce.CoreModule.Core.Common.AddressType.BillingAndShipping);
            target.Organization = Organization;
            target.OuterId = OuterId;
            return target;
        }

        public virtual AddressEntity FromModel(Address address)
        {
            if (address == null)
                throw new ArgumentNullException(nameof(address));

            Id = address.Key;

            CountryCode = address.CountryCode;
            CountryName = address.CountryName;
            PostalCode = address.PostalCode;
            RegionId = address.RegionId;
            RegionName = address.RegionName;
            City = address.City;
            Email = address.Email;
            FirstName = address.FirstName;
            MiddleName = address.MiddleName;
            LastName = address.LastName;
            Line1 = address.Line1;
            Line2 = address.Line2;
            Phone = address.Phone;
            AddressType = address.AddressType.ToString();
            Organization = address.Organization;
            OuterId = address.OuterId;

            return this;
        }

        public virtual void Patch(AddressEntity target)
        {
            target.CountryCode = CountryCode;
            target.CountryName = CountryName;
            target.Phone = Phone;
            target.PostalCode = PostalCode;
            target.RegionId = RegionId;
            target.RegionName = RegionName;
            target.AddressType = AddressType;
            target.City = City;
            target.Email = Email;
            target.FirstName = FirstName;
            target.MiddleName = MiddleName;
            target.LastName = LastName;
            target.Line1 = Line1;
            target.Line2 = Line2;
            target.Organization = Organization;
            target.OuterId = OuterId;
        }
    }
}
