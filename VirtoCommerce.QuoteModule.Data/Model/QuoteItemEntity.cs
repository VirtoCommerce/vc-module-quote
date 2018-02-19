using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;

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

        #region Navigation Properties
       
        public string QuoteRequestId { get; set; }
        public virtual QuoteRequestEntity QuoteRequest { get; set; }
		public virtual ObservableCollection<TierPriceEntity> ProposalPrices { get; set; }

       #endregion
     
       public virtual QuoteItemEntity ToModel(QuoteItemEntity quoteItem)
       {
         if (quoteItem == null)
           throw new ArgumentNullException(nameof(quoteItem));

         quoteItem.Id = this.Id;
         quoteItem.Currency = this.Currency;
         quoteItem.ListPrice= this.ListPrice;
         quoteItem.SalePrice = this.SalePrice;
         quoteItem.ProductId = this.ProductId;
         quoteItem.Sku = this.Sku;
         quoteItem.CatalogId = this.CatalogId;
         quoteItem.CategoryId = this.CategoryId;
         quoteItem.Name = this.Name;
         quoteItem.Comment= this.Comment;
         quoteItem.ImageUrl = this.ImageUrl;
         quoteItem.TaxType = this.TaxType;
         quoteItem.QuoteRequestId = this.QuoteRequestId;

         quoteItem.ProposalPrices = new ObservableCollection<TierPriceEntity>(this.ProposalPrices.Select(x => x.ToModel(AbstractTypeFactory<TierPriceEntity>.TryCreateInstance())));

      return quoteItem;
       }
     
       public virtual QuoteItemEntity FromModel(QuoteItemEntity quoteItem)
       {
         if (quoteItem == null)
           throw new ArgumentNullException(nameof(quoteItem));
     
         this.Id = quoteItem.Id;
         this.Currency = quoteItem.Currency;
         this.ListPrice = quoteItem.ListPrice;
         this.SalePrice = quoteItem.SalePrice;
         this.ProductId = quoteItem.ProductId;
         this.Sku = quoteItem.Sku;
         this.CatalogId = quoteItem.CatalogId;
         this.CategoryId = quoteItem.CategoryId;
         this.Name = quoteItem.Name;
         this.Comment = quoteItem.Comment;
         this.ImageUrl = quoteItem.ImageUrl;
         this.TaxType = quoteItem.TaxType;
         this.QuoteRequestId = quoteItem.QuoteRequestId;

         if (quoteItem.ProposalPrices != null)
         {
           this.ProposalPrices = new ObservableCollection<TierPriceEntity>(quoteItem.ProposalPrices.Select(x => AbstractTypeFactory<TierPriceEntity>.TryCreateInstance().FromModel(x)));
         }
      
         return this;
       }
     
       public virtual void Patch(QuoteItemEntity target)
       {
         target.Currency = this.Currency;
         target.ListPrice = this.ListPrice;
         target.SalePrice = this.SalePrice;
         target.ProductId = this.ProductId;
         target.Sku = this.Sku;
         target.CatalogId = this.CatalogId;
         target.CategoryId = this.CategoryId;
         target.Name = this.Name;
         target.Comment = this.Comment;
         target.ImageUrl = this.ImageUrl;
         target.TaxType = this.TaxType;

         if (!this.ProposalPrices.IsNullCollection())
         {
           this.ProposalPrices.Patch(target.ProposalPrices, (sourceProposalPrices, targetProposalPrices) => sourceProposalPrices.Patch(targetProposalPrices));
         }
       }
    }
}
