using System.Threading.Tasks;
using VirtoCommerce.QuoteModule.Core.Models;

namespace VirtoCommerce.QuoteModule.Core.Services
{
    public interface IQuoteTotalsCalculator
    {
        Task<QuoteRequestTotals> CalculateTotalsAsync(QuoteRequest quote);
    }
}
