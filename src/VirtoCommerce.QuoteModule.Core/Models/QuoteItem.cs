using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.CatalogModule.Core.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.QuoteModule.Core.Models
{
    public class QuoteItem : Entity, ICloneable
    {
        public string Currency { get; set; }
        /// <summary>
        /// Base catalog price
        /// </summary>
        public decimal ListPrice { get; set; }
        /// <summary>
        /// Sale price for buyer
        /// </summary>
        public decimal SalePrice { get; set; }

        public string Sku { get; set; }
        public string ProductId { get; set; }
        public CatalogProduct Product { get; set; }

        public string CatalogId { get; set; }
        public string CategoryId { get; set; }

        public string Name { get; set; }

        public string Comment { get; set; }

        public string ImageUrl { get; set; }

        public string TaxType { get; set; }
        /// <summary>
        /// Requested quote item qty
        /// </summary>
        public int Quantity { get; set; }

        public bool IsConfigured { get; set; }

        private TierPrice _selectedTierPrice;
        public TierPrice SelectedTierPrice
        {
            get
            {
                if (_selectedTierPrice == null && ProposalPrices != null)
                {
                    _selectedTierPrice = ProposalPrices.FirstOrDefault();
                }
                return _selectedTierPrice;
            }

            set
            {
                _selectedTierPrice = value;
            }
        }

        public ICollection<TierPrice> ProposalPrices { get; set; }

        public IList<QuoteConfigurationItem> ConfigurationItems { get; set; }

        public object Clone()
        {
            var result = (QuoteItem)MemberwiseClone();

            result.ConfigurationItems = ConfigurationItems?.Select(x => x.Clone()).OfType<QuoteConfigurationItem>().ToList();
            result.ProposalPrices = ProposalPrices?.Select(x => x.Clone()).OfType<TierPrice>().ToList();

            if (_selectedTierPrice != null)
            {
                result._selectedTierPrice = result.ProposalPrices?.FirstOrDefault(x => _selectedTierPrice.Equals(x));
            }

            return result;
        }
    }
}
