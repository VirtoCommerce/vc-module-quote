using System;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Domain.Quote.Model;
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
       
        public virtual TierPrice ToModel(TierPrice tierPrice)
        {
          if (tierPrice == null)
            throw new ArgumentNullException(nameof(tierPrice));
       
          tierPrice.Price = this.Price;
          tierPrice.Quantity = this.Quantity;
       
          return tierPrice;
        }
       
        public virtual TierPriceEntity FromModel(TierPrice tierPrice)
        {
          if (tierPrice == null)
            throw new ArgumentNullException(nameof(tierPrice));
         
          this.Price = tierPrice.Price;
          this.Quantity = tierPrice.Quantity;
       
          return this;
        }
       
        public virtual void Patch(TierPriceEntity target)
        {
          target.Price = this.Price;
          target.Quantity = this.Quantity;
        }
  }
}
