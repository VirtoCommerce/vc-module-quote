using System.Linq;
using FluentValidation;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.XCart.Core.Validators;

namespace VirtoCommerce.QuoteModule.ExperienceApi.Validation
{
    public sealed class CartToQuoteLineItemValidator : AbstractValidator<CartToQuoteLineItemValidationContext>
    {
        public CartToQuoteLineItemValidator()
        {
            RuleFor(x => x).Custom((lineItemContext, context) =>
            {
                var cartProduct = lineItemContext.AllCartProducts.FirstOrDefault(x => x.Id.EqualsIgnoreCase(lineItemContext.LineItem.ProductId));
                if (cartProduct == null)
                {
                    context.AddFailure(CartErrorDescriber.ProductUnavailableError(lineItemContext.LineItem));
                }
            });
        }
    }
}
