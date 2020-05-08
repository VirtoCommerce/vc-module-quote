using System;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.QuoteModule.Core.Models;

namespace VirtoCommerce.QuoteModule.Data.Model
{
    public class TierPriceEntity : Entity
    {
        [Column(TypeName = "Money")]
        public decimal Price { get; set; }

        public long Quantity { get; set; }

        #region Navigation Properties

        public virtual QuoteItemEntity QuoteItem { get; set; }
        public string QuoteItemId { get; set; }

        #endregion

        public TierPrice ToModel(TierPrice item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            //item.Id = Id;

            item.Price = Price;
            item.Quantity = Quantity;

            return item;
        }


        public TierPriceEntity FromModel(TierPrice item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            Price = item.Price;
            Quantity = item.Quantity;

            return this;
        }

        public virtual void Patch(TierPriceEntity target)
        {
            target.Price = Price;
            target.Quantity = Quantity;
        }
    }
}
