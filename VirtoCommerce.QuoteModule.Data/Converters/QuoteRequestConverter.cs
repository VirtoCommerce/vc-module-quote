using Omu.ValueInjecter;
using System.Collections.Generic;
using System.Linq;
using cartCoreModel = VirtoCommerce.Domain.Cart.Model;
using coreModel = VirtoCommerce.Domain.Quote.Model;
using taxCoreModel = VirtoCommerce.Domain.Tax.Model;

namespace VirtoCommerce.QuoteModule.Data.Converters
{
    public static class QuoteRequestConverter
	{
        public static taxCoreModel.TaxEvaluationContext ToTaxEvalContext(this coreModel.QuoteRequest quoteRequest)
        {
            var retVal = new taxCoreModel.TaxEvaluationContext();
            retVal.Id = quoteRequest.Id;
            retVal.Code = quoteRequest.Number;
            retVal.Currency = quoteRequest.Currency;
            retVal.Address = quoteRequest.Addresses != null ? quoteRequest.Addresses.FirstOrDefault() : null;
            retVal.Type = quoteRequest.GetType().Name;
            foreach(var quoteItem in quoteRequest.Items)
            {
                var line = new taxCoreModel.TaxLine
                {
                    Id = quoteItem.Id,
                    Code = quoteItem.Sku,
                    Name = quoteItem.Name,
                    TaxType = quoteItem.TaxType,
                    Amount = quoteItem.SelectedTierPrice.Price * quoteItem.SelectedTierPrice.Quantity
                };
                retVal.Lines.Add(line);
            }
            return retVal;
        }

        public static cartCoreModel.ShoppingCart ToCartModel(this coreModel.QuoteRequest quoteRequest)
        {
            var retVal = new cartCoreModel.ShoppingCart();
            retVal.InjectFrom(quoteRequest);
            retVal.Currency = quoteRequest.Currency;
            if (quoteRequest.Items != null)
            {
                retVal.Items = quoteRequest.Items.Select(x => x.ToCartModel()).ToList();
            }

            if(quoteRequest.ShipmentMethod != null)
            {
                var shipment = new cartCoreModel.Shipment
                {
                    Currency = quoteRequest.Currency,
                    ShipmentMethodCode = quoteRequest.ShipmentMethod.ShipmentMethodCode,
                    ShipmentMethodOption = quoteRequest.ShipmentMethod.OptionName                     
                };
                retVal.Shipments = new List<cartCoreModel.Shipment>(new[] { shipment });
            }

            return retVal;
        }
	}
}
