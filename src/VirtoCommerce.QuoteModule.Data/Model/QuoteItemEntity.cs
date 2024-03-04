using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.QuoteModule.Core.Models;

namespace VirtoCommerce.QuoteModule.Data.Model
{
    public class QuoteItemEntity : Entity
    {
        public QuoteItemEntity()
        {
            ProposalPrices = new NullCollection<TierPriceEntity>();
        }

        [Required]
        [StringLength(3)]
        public string Currency { get; set; }

        [Column(TypeName = "Money")]
        public decimal ListPrice { get; set; }
        [Column(TypeName = "Money")]
        public decimal SalePrice { get; set; }

        [Required]
        [StringLength(64)]
        public string ProductId { get; set; }
        [Required]
        [StringLength(64)]
        public string Sku { get; set; }

        [Required]
        [StringLength(64)]
        public string CatalogId { get; set; }

        [StringLength(64)]
        public string CategoryId { get; set; }
        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(2048)]
        public string Comment { get; set; }

        [StringLength(1028)]
        public string ImageUrl { get; set; }

        [StringLength(64)]
        public string TaxType { get; set; }

        public int Quantity { get; set; }

        #region Navigation Properties

        public virtual QuoteRequestEntity QuoteRequest { get; set; }
        public string QuoteRequestId { get; set; }

        public virtual ObservableCollection<TierPriceEntity> ProposalPrices { get; set; }

        #endregion

        public virtual QuoteItem ToModel(QuoteItem target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            var _ = this;
            target.Id = _.Id;

            target.ListPrice = _.ListPrice;
            target.SalePrice = _.SalePrice;
            target.TaxType = _.TaxType;
            target.ImageUrl = _.ImageUrl;
            target.ProductId = _.ProductId;
            target.Comment = _.Comment;
            target.CatalogId = _.CatalogId;
            target.CategoryId = _.CategoryId;
            target.Currency = _.Currency;
            target.Name = _.Name;
            target.Sku = _.Sku;
            target.Quantity = _.Quantity;
            target.ProposalPrices = ProposalPrices.Select(x => x.ToModel(AbstractTypeFactory<TierPrice>.TryCreateInstance())).ToList();

            return target;
        }

        public virtual QuoteItemEntity FromModel(QuoteItem _, PrimaryKeyResolvingMap pkMap)
        {
            if (_ == null)
                throw new ArgumentNullException(nameof(_));

            pkMap.AddPair(_, this);

            var target = this;
            target.Id = _.Id;

            target.ListPrice = _.ListPrice;
            target.SalePrice = _.SalePrice;
            target.TaxType = _.TaxType;
            target.ImageUrl = _.ImageUrl;
            target.ProductId = _.ProductId;
            target.Comment = _.Comment;
            target.CatalogId = _.CatalogId;
            target.CategoryId = _.CategoryId;
            target.Currency = _.Currency;
            target.Name = _.Name;
            target.Sku = _.Sku;
            target.Quantity = _.Quantity;

            if (_.ProposalPrices != null)
            {
                ProposalPrices = new ObservableCollection<TierPriceEntity>(_.ProposalPrices.Select(x => AbstractTypeFactory<TierPriceEntity>.TryCreateInstance().FromModel(x)));
            }

            return this;
        }

        public virtual void Patch(QuoteItemEntity target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            var _ = this;
            target.ListPrice = _.ListPrice;
            target.SalePrice = _.SalePrice;
            target.TaxType = _.TaxType;
            target.ImageUrl = _.ImageUrl;
            target.ProductId = _.ProductId;
            target.Comment = _.Comment;
            target.CatalogId = _.CatalogId;
            target.CategoryId = _.CategoryId;
            target.Currency = _.Currency;
            target.Name = _.Name;
            target.Sku = _.Sku;
            target.Quantity = _.Quantity;

            if (!ProposalPrices.IsNullCollection())
            {
                var tierPriceComparer = AnonymousComparer.Create((TierPriceEntity x) => x.Quantity + "-" + x.Price);
                ProposalPrices.Patch(target.ProposalPrices, tierPriceComparer, (sourceTierPrice, targetTierPrice) => { });
            }
        }
    }
}
