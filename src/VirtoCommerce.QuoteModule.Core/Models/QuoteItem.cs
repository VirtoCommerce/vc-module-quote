using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.CatalogModule.Core.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.QuoteModule.Core.Models
{
    public class QuoteItem : Entity
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

        [Obsolete("Use IQuoteConverter instead", DiagnosticId = "VC0008", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        public LineItem ToCartModel(LineItem target)
        {
            var _ = this;
            target.Id = _.Id;

            target.ListPrice = _.ListPrice;
            target.SalePrice = _.SalePrice;
            target.TaxType = _.TaxType;
            target.ImageUrl = _.ImageUrl;
            target.ProductId = _.ProductId;
            target.CatalogId = _.CatalogId;
            target.CategoryId = _.CategoryId;
            target.Currency = _.Currency;
            target.Name = _.Name;
            target.Sku = _.Sku;
            target.Quantity = _.Quantity;
            target.TaxPercentRate = 0.13m;

            if (SelectedTierPrice != null)
            {
                target.SalePrice = SelectedTierPrice.Price;
                target.Quantity = (int)SelectedTierPrice.Quantity;
            }

            if (target.SalePrice > 0 && target.ListPrice > target.SalePrice)
            {
                target.DiscountAmount = target.ListPrice - target.SalePrice;
            }
            return target;
        }
    }
}
