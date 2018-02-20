using Omu.ValueInjecter;
using cartCoreModel = VirtoCommerce.Domain.Cart.Model;
using coreModel = VirtoCommerce.Domain.Quote.Model;

namespace VirtoCommerce.QuoteModule.Data.Converters
{
    public static class QuoteItemConverter
	{
        public static cartCoreModel.LineItem ToCartModel(this coreModel.QuoteItem quoteItem)
        {
            var retVal = new cartCoreModel.LineItem();
            retVal.InjectFrom(quoteItem);
            retVal.Sku = quoteItem.Sku;
            if (quoteItem.SelectedTierPrice != null)
            {
                retVal.SalePrice = quoteItem.SelectedTierPrice.Price;
                retVal.Quantity = (int)quoteItem.SelectedTierPrice.Quantity;
             }
            return retVal;
        }
	}
}
