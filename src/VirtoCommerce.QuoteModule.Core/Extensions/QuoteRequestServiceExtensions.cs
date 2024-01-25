using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;

namespace VirtoCommerce.QuoteModule.Core.Extensions;

public static class QuoteRequestServiceExtensions
{
    public static async Task<QuoteRequest> GetByIdAsync(this IQuoteRequestService quoteRequestService, string id)
    {
        var quotes = await quoteRequestService.GetByIdsAsync(id);
        return quotes.FirstOrDefault();
    }
}
