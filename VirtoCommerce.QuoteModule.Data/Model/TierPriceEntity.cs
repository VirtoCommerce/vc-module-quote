using System;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.QuoteModule.Data.Model
{
    public class TierPriceEntity : Entity
	{
		[Column(TypeName = "Money")]
		public decimal Price { get; set; }
		public long Quantity { get; set; }

        #region Navigation Properties
       
        public string QuoteItemId { get; set; }
        public virtual QuoteItemEntity QuoteItem { get; set; }

        #endregion
       
        public virtual TierPriceEntity ToModel(TierPriceEntity tierPrice)
        {
          if (tierPrice == null)
            throw new ArgumentNullException(nameof(tierPrice));
       
          tierPrice.Id = this.Id;
          tierPrice.Price = this.Price;
          tierPrice.Quantity = this.Quantity;
          tierPrice.QuoteItemId = this.QuoteItemId;
       
          return tierPrice;
        }
       
        public virtual TierPriceEntity FromModel(TierPriceEntity tierPrice)
        {
          if (tierPrice == null)
            throw new ArgumentNullException(nameof(tierPrice));
       
          this.Id = tierPrice.Id;
          this.Price = tierPrice.Price;
          this.Quantity = tierPrice.Quantity;
          this.QuoteItemId = tierPrice.QuoteItemId;
       
          return this;
        }
       
        public virtual void Patch(TierPriceEntity target)
        {
          target.Price = this.Price;
          target.Quantity = this.Quantity;
        }
  }
}
