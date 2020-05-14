using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.QuoteModule.Core.Models;

namespace VirtoCommerce.QuoteModule.Core.Services
{
    public interface IQuoteRequestService
    {
        Task<QuoteRequestSearchResult> SearchAsync(QuoteRequestSearchCriteria criteria);

        Task<IEnumerable<QuoteRequest>> GetByIdsAsync(params string[] ids);
        Task SaveChangesAsync(QuoteRequest[] quoteRequests);
        Task DeleteAsync(string[] ids);
    }
}
