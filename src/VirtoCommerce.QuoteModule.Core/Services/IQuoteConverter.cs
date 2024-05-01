using System.Threading.Tasks;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.QuoteModule.Core.Models;

namespace VirtoCommerce.QuoteModule.Core.Services;

public interface IQuoteConverter
{
    Task<QuoteRequest> ConvertFromCart(ShoppingCart cart);
    ShoppingCart ConvertToCart(QuoteRequest quote);
    ShoppingCart ConvertToCartWithTax(QuoteRequest quote);
}
