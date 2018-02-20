using System;
using System.Collections.Generic;
using dataModel = VirtoCommerce.QuoteModule.Data.Model;

namespace VirtoCommerce.QuoteModule.Data.Converters
{
    public class AddressComparer : IEqualityComparer<dataModel.AddressEntity>
	{
		#region IEqualityComparer<Discount> Members

		public bool Equals(dataModel.AddressEntity x, dataModel.AddressEntity y)
		{
			return GetHashCode(x) == GetHashCode(y);
		}

		public int GetHashCode(dataModel.AddressEntity obj)
		{
			var result = String.Join(":", obj.Organization, obj.City, obj.CountryCode, obj.CountryName,
										  obj.Email, obj.FirstName, obj.LastName, obj.Line1, obj.Line2, obj.Phone, obj.PostalCode, obj.RegionId, obj.RegionName);
			return result.GetHashCode();
		}

		#endregion
	}
}
